﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : NetworkBehaviour
{
    [SyncVar]
    public float damagePerShot = 10f;               //Damage each bullet deals to enemies.
    [SyncVar]
    public float timeBetweenShots = 0.15f;          //The time between each shot.
    [SyncVar]
    public float range = 100f;                      //The range that the gun can fire.
    [SyncVar]
    public float reloadTime = 1f;                   //Time taken to reload gun back to max ammo.
    [SyncVar]
    public int maxAmmo = 10;                        //Value for maximum ammo per 'magazine'.
    [SyncVar]
    float effectsDisplayTime = 0.2f;                //The proportion of the timeBetweenShots that the effects which display for.
    [SyncVar]
    private int currentAmmo;                        //Value for the ammo currently in the magazine.
    [SyncVar]
    private bool reloading;                         //Bool showing if the player is reloading or not.
    [SyncVar]
    private Transform GunEnd;
    [SyncVar]
    private Transform weaponSwitchTransform;
    [SyncVar]
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
        weaponSwitchTransform = gameObject.transform.GetChild(2);

        CmdGunEndCheck();

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

        CmdGunEndCheck();

        //timer to help with weapon rate of fire.
        timer += Time.deltaTime;

        //stops other statements from running while reloading.
        if (reloading)
        {
            return;
        }

        //If the guns current ammo is less than or equal to nothing...
        if (currentAmmo <= 0)
        {
            //...start reloading.
            StartCoroutine(Reload());
            return;
        }

        //If 'r' key is pressed, and ammo is not already at max value...
        if (Input.GetKeyDown("r") && currentAmmo != maxAmmo)
        {
            //...start reloading.
            StartCoroutine(Reload());
            return;
        }

        //If 'Fire1' button is pressed, and the time between shots has not exceeded the timer...
        if (Input.GetButton("Fire1") && timer >= timeBetweenShots && Time.timeScale != 0)
        {
            //...then the gun shoots.

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

    IEnumerator Reload()
    {
        reloading = true;

        Debug.Log("Reloading...");

        //animator.SetBool("Reloading", true);                    //Starts reloading animation.
        CmdDisableMuzzleEffects();                                 //Disables all muzzle effects (e.g. light and particles) while reloading.
        yield return new WaitForSeconds(reloadTime - 0.25f);    //Waits until reload time is up.
        //animator.SetBool("Reloading", false);                   //Animation for reloading is ending and going back to idle.
        yield return new WaitForSeconds(0.25f);                 //0.25 second wait to alow for animation to go back to its idle state before anything else continues.

        //Resets the current ammo to maximum ammo.
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

    void Shooting()
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
        if (!isLocalPlayer)
        {
            return;
        }
        Shooting();
    }

    [Command]
    void CmdShooting()
    {
        RpcShooting();
    }

    [Command]
    void CmdGunEndCheck()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        foreach (Transform weapon in weaponSwitchTransform)
        {
            int i = 0;
            //Checks which gun is active, and sets a transform for the end of the gun.
            if (weaponSwitchTransform.GetChild(i).gameObject.activeSelf)
            {
                GunEnd = weaponSwitchTransform.GetChild(i).GetChild(0);
            }
            //increment i to check through each child.
            i++;
        }
    }

}