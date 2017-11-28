using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullpup : WeaponType {

    //public override void Init()
    public void Awake()
    {
        weaponName = "Bullpup";

        damagePerShot = 10.0f;
        timeBetweenShots = 0.15f;
        range = 100.0f;
        reloadTime = 1.0f;
        maxAmmo = 10;
        weaponDamage = 25;
        currentAmmo = maxAmmo;

        effectsDisplayTime = 0.1f;

        //gunEnd = new GameObject().transform;
        //gunEnd.position = new Vector3(0.29f, 1.654f, 1.322f);

        //weaponSwitchTransform = new GameObject().transform;
        //weaponSwitchTransform.position 


        //Debug.Log("Awake Called !");
        
        //weaponSwitchTransform = this.transform;
        //gunEnd = this.transform;

        //shootRay = new Ray();
        //shootHit = new RaycastHit();

        //gunParticles = new ParticleSystem();
        //gunLine = new LineRenderer();
        //gunLight = new Light();
    }


}
