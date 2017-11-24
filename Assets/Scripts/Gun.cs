using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : NetworkBehaviour
{
 
    public float damagePerShot = 10f;               //Damage each bullet deals to enemies.
    public float timeBetweenShots = 0.15f;          //The time between each shot.
    public float range = 100f;                      //The range that the gun can fire.

    public float reloadTime = 1.0f;                   //Time taken to reload gun back to max ammo.
    public int maxAmmo = 10;                        //Value for maximum ammo per 'magazine'.

    public Transform weaponSwitchTransform;
    public Transform GunEnd;

    float effectsDisplayTime = 0.2f;                //The proportion of the timeBetweenShots that the effects which display for.
    private int currentAmmo;                        //Value for the ammo currently in the magazine.
    private bool reloading;                         //Bool showing if the player is reloading or not.
    
    float timer;                                    //Timer to know when you can shoot (used for 'timeBetweenShots').

    //public Animator animator;
    //public GameObject impactEffect;                 //Particle system at bullet point of impact.
    Ray shootRay = new Ray();                       //Ray from the gun.
    RaycastHit shootHit;                            //Raycast hit to determine what was hit.          
    ParticleSystem gunParticles;                    //Reference to the particle system.
    LineRenderer gunLine;                           //Reference to the line renderer.
    Light gunLight;                                 //Reference to the guns light source.
    //AudioSource gunAudio;

    void Awake()
    {
        // Set up the references.
        gunParticles = GetComponentInChildren<ParticleSystem>();
        gunLine = GetComponentInChildren<LineRenderer>();
        gunLight = GetComponentInChildren<Light>();

        //gunAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        //
        //weaponSwitchTransform = gameObject.transform.GetChild(2);

        //CmdGunEndCheck();

        //GunEnd = weaponSwitchTransform;

        //Starts off game with the magazine at max value.
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        reloading = false;                          //sets reloading to false when a new weapon is enabled.
        //animator.SetBool("Reloading", false);       //sets the animation for reloading to false when a new weapon is enabled.
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //CmdGunEndCheck();

        //timer to help with weapon rate of fire.
        timer += Time.deltaTime;
        /*
        //stops other statements from running while reloading.
        if (reloading)
        {
            //return;
            ReloadCheck();
        }
        */
        //If the guns current ammo is less than or equal to nothing...
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        //If 'r' key is pressed, and ammo is not already at max value...
        if (Input.GetKeyDown("r") && currentAmmo != maxAmmo)
        {
            StartReload();
            return;
        }

        //If 'Fire1' button is pressed, and the time between shots has not exceeded the timer...
        if (Input.GetButton("Fire1") && timer >= timeBetweenShots && Time.timeScale != 0)
        {
            if (isServer)
            {
                RpcShooting();
            }
            else
            {
                CmdShooting();
            }
        }

        // If the timer has exceeded the proportion of timeBetweenBullets and the effects...
        if (timer >= timeBetweenShots * effectsDisplayTime)
        {
            //.. then disable the effects.
            CmdDisableMuzzleEffects();
        }
    }

    void StartReload()
    {        
        if (reloading == false)
        {
            Debug.Log("Invoking Reload");
            CmdDisableMuzzleEffects();
            Invoke("Reload", reloadTime);
        }
        reloading = true;
    }

    void Reload()
    {
        Debug.Log("Reloading Done!");
        currentAmmo = maxAmmo;
        reloading = false;
    }


    [Command]
    public void CmdDisableMuzzleEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        //Resets the timer.
        timer = 0f;

        //Reduce ammo by 1 each time the gun shoots.
        currentAmmo--;

        //gunAudio.Play();

        //Turns on gun flash (light source).
        gunLight.enabled = true;

        //Stops particles playing if they were already, then starts playing again.
        gunParticles.Stop();
        gunParticles.Play();

        //Gunshot();

        //Enable the line renderer and set its position to the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, GunEnd.position);

        //Sets the shootRay so it starts at the gun and points forward.
        shootRay.origin = GunEnd.position;
        shootRay.direction = GunEnd.forward;

        //Perform the raycast against game objects, and if it hits...
        if (Physics.Raycast(shootRay, out shootHit, range))
        {
            //code in place for damaging enemy health... uncomment and adjust when enemy health is implemented
            //EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            //if (enemyHealth != null)
            //{
            //    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            //}

            //line renderer ends where it hits something.
            gunLine.SetPosition(1, shootHit.point);
            //GameObject impactObject = Instantiate(impactEffect, shootHit.point, Quaternion.LookRotation(shootHit.normal));
            //Destroy(impactObject, 2f);
        }
        //If nothing gets hit by the raycast...
        else
        {
            //...then draw the line render anyway at its max range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
    
    [ClientRpc]
    void RpcShooting()
    {
        if (isLocalPlayer)
        {
            Shoot();
        }
        
    }    
    
    [Command]
    void CmdShooting()
    {
        if (!isLocalPlayer)
        {
            Shoot();
        }
    }
    
}