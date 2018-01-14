using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponShooting : NetworkBehaviour
{

    //public WeaponType selectedWeapon;

    //  Private reference to the specfic wepaon script
    private WeaponType selectedWeapon;
    //  Game object that the weapon script is attached to
    public GameObject equippedWeapon;

   
    Ray shootRay = new Ray();                       //Ray from the gun.
    RaycastHit shootHit;                            //Raycast hit to determine what was hit.
    ParticleSystem gunParticles;                    //Reference to the particle system.
    LineRenderer gunLine = new LineRenderer();      //Reference to the line renderer.
    Light gunLight = new Light();                   //Reference to the guns light source.


    public Transform gunTransform;
    public Transform gunEnd;


    public void shootInit()
    {

        //Bullpup gun = new Bullpup();
        if (equippedWeapon)
        {
            selectedWeapon = equippedWeapon.GetComponent<WeaponType>();
        }

        gunParticles = GetComponentInChildren<ParticleSystem>();
        gunLine = GetComponentInChildren<LineRenderer>();
        gunLight = GetComponentInChildren<Light>();

    }

    private void Awake()
    {

        //selectedWeapon.Init();

        //Debug.Log("gun name = " + selectedWeapon.weaponName);

        //selectedWeapon = gameObject.AddComponent<Bullpup>( );
        
    }

    void Start()
    {
        //Starts off game with the magazine at max value.
        //selectedWeapon.currentAmmo = selectedWeapon.maxAmmo;
        //Debug.Log(selectedWeapon.weaponName);
        /*
        if (GetComponentInChildren<ParticleSystem>())
        {
            Debug.Log("Partcle System Found");
        }
        else
        {
            Debug.Log("Partcle System Not Found");
        }
        if (GetComponentInChildren<LineRenderer>())
        {
            Debug.Log("LineRenderer System Found");
        }
        else
        {
            Debug.Log("LineRenderer System Not Found");
        }
        if (GetComponentInChildren<Light>())
        {
            Debug.Log("Light System Found");
        }
        else
        {
            Debug.Log("Light System Not Found");
        }
        */
        shootInit();


        //gunEnd = equippedWeapon.transform.Find("GunEnd").transform;


        //Debug.Log("BEBELBEBLEBLEBLLE" + gunEnd);

        //this.transform.parent.GetComponent<ParticleSystem>;

        //selectedWeapon.reloading = false;                          //sets reloading to false when a new weapon is enabled.
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selectedWeapon.currentAmmo);
        if (!isLocalPlayer)
        {
            return;
        }
        if (!selectedWeapon)
        {
            return;
        }

        //CmdGunEndCheck();

        //timer to help with weapon rate of fire.
        selectedWeapon.timer += Time.deltaTime;
        /*
        //stops other statements from running while reloading.
        if (reloading)
        {
            //return;
            ReloadCheck();
        }
        */
        //If the guns current ammo is less than or equal to nothing...
        if (selectedWeapon.currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        //If 'r' key is pressed, and ammo is not already at max value...
        if (Input.GetKeyDown("r") && selectedWeapon.currentAmmo != selectedWeapon.maxAmmo)
        {
            StartReload();
            return;
        }

        //If 'Fire1' button is pressed, and the time between shots has not exceeded the timer...
        if (Input.GetButton("Fire1") && selectedWeapon.timer >= selectedWeapon.timeBetweenShots && Time.timeScale != 0)
        {
            Shoot();
            CmdServerShoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets and the effects...
        if (selectedWeapon.timer >= selectedWeapon.timeBetweenShots * selectedWeapon.effectsDisplayTime)
        {
            //  Disable Effects
            CmdDisableMuzzleEffects();
        }
    }

    void StartReload()
    {
        if (selectedWeapon.reloading == false)
        {
            //Debug.Log("Reloading !");
            CmdDisableMuzzleEffects();
            Invoke("Reload", selectedWeapon.reloadTime);
        }
        selectedWeapon.reloading = true;
    }

    void Reload()
    {
        //Debug.Log("Reloading Done!");
        selectedWeapon.currentAmmo = selectedWeapon.maxAmmo;
        selectedWeapon.reloading = false;
    }


    void Shoot()
    {
        //  Resets the timer.
        selectedWeapon.timer = 0f;

        //  Reduce ammo by 1
        selectedWeapon.currentAmmo--;


        //gunAudio.Play();
        gunLight.enabled = true;

        //  Restart particles
        gunParticles.Stop();
        gunParticles.Play();

        //  Draw Line renderer
        gunLine.enabled = true;
        gunLine.SetPosition(0, gunEnd.transform.position);

        //  Sets the shootRay so it starts at the gun and points forward.
        shootRay.origin = gunTransform.transform.position;
        shootRay.direction = gunEnd.transform.forward;

        //Debug.Log("Fire the raycast !");

        int rayLayer = 1;

        //Perform the raycast against game objects, and if it hits...
        if (Physics.Raycast(shootRay, out shootHit, selectedWeapon.range, rayLayer))
        {
            //Debug.Log(shootHit.transform.name);
            //line renderer ends where it hits something.
            gunLine.SetPosition(1, shootHit.point);

            //shootHit.collider.gameObject.GetComponent<AIStats>().CmdDie();
            if (shootHit.collider.gameObject.GetComponent<AIStats>())
            {
                //Debug.Log("AI Stats found");
                CmdHit(shootHit.collider.gameObject, 25);
                //Debug.Log(shootHit.collider.gameObject.GetComponent<AIStats>().enemyHealth);
            }

            // Hit effect goes here !
            //GameObject impactObject = Instantiate(impactEffect, shootHit.point, Quaternion.LookRotation(shootHit.normal));
        }
        //If nothing gets hit by the raycast...
        else
        {
            //Debug.Log("Not hit anything");
            //...then draw the line render anyway at its max range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * selectedWeapon.range);
        }
    }

    void ShootEffect()
    {
        //  Handles all the firing effects without touching the ammo or damage counts
        //  This stops multiple damage calls and ammo reductions being called per shot

        //gunAudio.Play();
        gunLight.enabled = true;

        //  Restart particles
        gunParticles.Stop();
        gunParticles.Play();

        //  Draw Line renderer
        gunLine.enabled = true;
        gunLine.SetPosition(0, gunEnd.transform.position);

        //  Sets the shootRay so it starts at the gun and points forward.
        shootRay.origin = gunEnd.transform.position;
        shootRay.direction = gunEnd.transform.forward;

        //Perform the raycast against game objects, and if it hits...
        if (Physics.Raycast(shootRay, out shootHit, selectedWeapon.range))
        {
            //Debug.Log("Effect Called !");
            //line renderer ends where it hits something.
            gunLine.SetPosition(1, shootHit.point);

            // Hit effect goes here !
            //GameObject impactObject = Instantiate(impactEffect, shootHit.point, Quaternion.LookRotation(shootHit.normal));
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * selectedWeapon.range);
        }
    }

    [Command]
    void CmdHit(GameObject hitObject, int damageAmount)
    {
        hitObject.GetComponent<AIStats>().CmdDamage(damageAmount);
    }

    [Command]
    public void CmdServerShoot()
    {
        if (isServer)
        {
            ShootEffect();
            RpcClientsShoot();
        }
    }

    [ClientRpc]
    public void RpcClientsShoot()
    {
        ShootEffect();
    }

    [Command]
    public void CmdDisableMuzzleEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        gunLight.enabled = false;
        gunLight.enabled = false;

        RpcDisableMuzzleEffects();
    }

    [ClientRpc]
    public void RpcDisableMuzzleEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        gunLight.enabled = false;
        gunLight.enabled = false;
    }
}