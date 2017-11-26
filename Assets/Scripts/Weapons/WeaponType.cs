using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType {

    public float damagePerShot;                 //Damage each bullet deals to enemies.
    public float timeBetweenShots;              //The time between each shot.
    public float range;                         //The range that the gun can fire.
    public float weaponDamage;

    public float reloadTime;                    //Time taken to reload gun back to max ammo.
    public int maxAmmo;                         //Value for maximum ammo per 'magazine'.

    public Transform weaponSwitchTransform;
    public Transform gunEnd;

    public GameObject weaponModel;

    public string weaponName;

    public float effectsDisplayTime;           //The proportion of the timeBetweenShots that the effects which display for.
    public int currentAmmo;                    //Value for the ammo currently in the magazine.
    public bool reloading;                     //Bool showing if the player is reloading or not.

    public float timer;                                //Timer to know when you can shoot (used for 'timeBetweenShots').

    //public Animator animator;
    //public GameObject impactEffect;           //Particle system at bullet point of impact.
    //public Ray shootRay;                   //Ray from the gun.
    //public RaycastHit shootHit;                        //Raycast hit to determine what was hit.          
    //public ParticleSystem gunParticles;                //Reference to the particle system.
    //public LineRenderer gunLine;                       //Reference to the line renderer.
    //public Light gunLight;                             //Reference to the guns light source.

    /*
    void Awake()
    {
        // Set up the references.
        
        gunParticles = GetComponentInChildren<ParticleSystem>();
        gunLine = GetComponentInChildren<LineRenderer>();
        gunLight = GetComponentInChildren<Light>();
             

        //gunAudio = GetComponent<AudioSource>();
    }
    */
}
