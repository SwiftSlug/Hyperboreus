using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponShooting : NetworkBehaviour
{
    //public bool blep = true;
    //public WeaponType selectedWeapon;

    //  Private reference to the specfic wepaon script
    private WeaponType selectedWeapon;
    //  Game object that the weapon script is attached to
    public GameObject equippedWeapon;


    Ray shootRay = new Ray();                       //Ray from the gun.
    RaycastHit shootHit;                            //Raycast hit to determine what was hit.
    ParticleSystem gunParticles;                    //Reference to the particle system.
    public ParticleSystem gunShootingParticles;
    //LineRenderer gunLine = new LineRenderer();      //Reference to the line renderer.
    Light gunLight = new Light();                   //Reference to the guns light source.
    LineRenderer gunLine;

    public Transform gunTransform;
    public Transform gunEnd;

    public AudioSource audioSource;
    public AudioClip clipReload;

    public AudioSync audioSync;

    //  Init function for 
    public void shootInit()
    {
        if (equippedWeapon)
        {
            //  Set all starting references if equipped weapon is true
            selectedWeapon = equippedWeapon.GetComponent<WeaponType>();

            gunParticles = equippedWeapon.GetComponentInChildren<ParticleSystem>();
            gunLine = equippedWeapon.GetComponentInChildren<LineRenderer>();
            gunLight = equippedWeapon.GetComponentInChildren<Light>();
            
        }
        if (gunLight)
        {
            //  Ensure gunlight is off at start
            gunLight.enabled = false;
        }

    }

    private void Awake()
    {
    }

    void Start()
    {
        shootInit();

        audioSync = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioSync>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selectedWeapon.currentAmmo);
        if (!isLocalPlayer)
        {
            //  Ensure only local player can run
            return;
        }
        if (!selectedWeapon)
        {
            //  Ensure script can only run if there is a selected weapon
            return;
        }

        //  Increase timer value
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

        /*
        //If 'r' key is pressed, and ammo is not already at max value...
        if (Input.GetButton("Reload") && selectedWeapon.currentAmmo != selectedWeapon.maxAmmo)
        {
            StartReload();
            return;
        }
        */
        /*
        //If 'Fire1' button is pressed, and the time between shots has not exceeded the timer...
        if (Input.GetButton("Fire1") && selectedWeapon.timer >= selectedWeapon.timeBetweenShots && Time.timeScale != 0)
        {
            Shoot();
            CmdServerShoot();
        }
        */
        /*
        //  Controller Fire Axis Detection
        if ((Input.GetAxis("Fire1") > 0) && selectedWeapon.timer >= selectedWeapon.timeBetweenShots && Time.timeScale != 0)
        {
            Shoot();
            CmdServerShoot();
        }
        */

        // If the timer has exceeded the proportion of timeBetweenBullets and the effects...
        if (selectedWeapon.timer >= selectedWeapon.timeBetweenShots + selectedWeapon.effectsDisplayTime)
        {
            //  Disable Effects
            CmdDisableMuzzleEffects();
        }
    }

    public void Fire()
    {
        //  Ensure that functionality only runs if a weapon is equipped
        if (selectedWeapon == null)
        {
            return;
        }

        if (selectedWeapon.reloading == false)
        {
            //  Player is not realoading to continue with firing

            //  Only fire if firing cooldown has been completed
            if (selectedWeapon.timer >= selectedWeapon.timeBetweenShots && Time.timeScale != 0)
            {
                //Darrell's Non-Modular Audio
                if (selectedWeapon.weaponName == "Bullpup")
                {
                    audioSync.PlaySound(this.gameObject, 3);
                }
                else if (selectedWeapon.weaponName == "Light Machine Gun")
                {
                    audioSync.PlaySound(this.gameObject, 4);
                }
                else if (selectedWeapon.weaponName == "Sub Machine Gun")
                {
                    audioSync.PlaySound(this.gameObject, 5);
                }

                Shoot();
            }
            
        }

    }

    public void StartReload()
    {
        if (selectedWeapon != null)
        {
            if (selectedWeapon.reloading == false)
            {
                //Debug.Log("Reloading !");
                audioSource.PlayOneShot(clipReload, 1.0f);

                audioSync.ResetSound();

                CmdDisableMuzzleEffects();
                Invoke("Reload", selectedWeapon.reloadTime);
            }
            selectedWeapon.reloading = true;
        }
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

        //  Draw firing effects
        CmdShootEffects();

        //  Set the shootRay so it traces in front of the player
        shootRay.origin = gunTransform.transform.position;
        shootRay.direction = gunEnd.transform.forward;

        //  Set shootray layer to 1
        int rayLayer = 1;
        
        //  Raytrace for hit objects
        if (Physics.Raycast(shootRay, out shootHit, selectedWeapon.range, rayLayer))
        {
            if (shootHit.collider.gameObject.GetComponent<AIStats>())
            {
                // Raytrace has hit a gameobject with AIStats attached

                CmdHit(shootHit.collider.gameObject, selectedWeapon.weaponDamage);   //  Call damage on the hit AI              
            }

            // Hit effect goes here !
            //GameObject impactObject = Instantiate(impactEffect, shootHit.point, Quaternion.LookRotation(shootHit.normal));
        }

    }

    void ShootEffects()
    {
        //  Handles all the firing effects without touching the ammo or damage counts
        //  This stops multiple damage calls and ammo reductions being called per shot

        /*
        //  Draw Line renderer
        if (gunLine)
        {
            gunLine.enabled = true;
            gunLine.SetPosition(0, gunEnd.transform.position);
            gunLine.SetPosition(1, gunEnd.transform.position + (shootRay.direction * selectedWeapon.range));
        }
        */
        if (gunParticles)
        {
            //  Create Particles for muzzle effect
            var newParticles = Instantiate(gunParticles, gunEnd.transform.position, Quaternion.identity);
            newParticles.Play();
            if (isServer)
            {
                //  Only server to destroy the object
                Destroy(newParticles.gameObject, newParticles.main.duration);   //  Destroy particles after their duration is up        
            }
        }
        
        if (gunShootingParticles)
        {
            //  Create Particles for bullet
            var newBulletParticle = Instantiate(gunShootingParticles, gunEnd.transform.position, gunEnd.transform.rotation);
            newBulletParticle.Play();
            if (isServer)
            {
                //  Only server to destroy the object
                Destroy(newBulletParticle.gameObject, newBulletParticle.main.duration);   //  Destroy particles after their duration is up 
            }
            //Debug.Log(newBulletParticle.main.duration);
            //gunShootingParticles.Play();
        }
        //  Flash muzzle light
        float flashDuration = 0.1f;

        gunLight.enabled = true;
        Invoke("MuzzleFlashToggle", flashDuration);
    }

    [Command]
    void CmdShootEffects()
    {
        ShootEffects();
        //  Sync effects to all connected clients
        RpcShootEffects();
    }

    [ClientRpc]
    void RpcShootEffects()
    {
        ShootEffects();
    }

    void MuzzleFlashToggle()
    {
        if(gunLight.enabled == true)
        {
            gunLight.enabled = false;
        }
        else
        {
            gunLight.enabled = true;
        }
    }

    [Command]
    void CmdHit(GameObject hitObject, int damageAmount)
    {
        if (hitObject != null)
        {
            hitObject.GetComponent<AIStats>().CmdDamage(damageAmount);
        }
    }

    [Command]
    public void CmdDisableMuzzleEffects()
    {
        DisableMuzzleEffects();
        //  Ensure that all clients disable the firing effect
        RpcDisableMuzzleEffects();
    }

    [ClientRpc]
    public void RpcDisableMuzzleEffects()
    {
        DisableMuzzleEffects();
    }

    void DisableMuzzleEffects()
    {
        // Disable the line renderer and the light.
        if (gunLine)
        {
            gunLine.enabled = false;
        }
        if (gunLight)
        {
            gunLight.enabled = false;
        }
    }

}