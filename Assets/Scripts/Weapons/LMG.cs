using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMG : WeaponType
{
    //public override void Init()
    public void Awake()
    {
        weaponName = "Light Machine Gun";

        //damagePerShot = 50.0f;
        timeBetweenShots = 0.05f;
        range = 100.0f;
        reloadTime = 2.0f;
        maxAmmo = 100;
        weaponDamage = 50;
        currentAmmo = maxAmmo;

        effectsDisplayTime = 0.06f;

        //gunEnd = new GameObject().transform;
        //gunEnd.position = new Vector3(0.29f, 1.654f, 1.322f);

        //weaponSwitchTransform = new GameObject().transform;
        //weaponSwitchTransform.position 


        Debug.Log("Awake Called !");

        //weaponSwitchTransform = this.transform;
        //gunEnd = this.transform;

        //shootRay = new Ray();
        //shootHit = new RaycastHit();

        //gunParticles = new ParticleSystem();
        //gunLine = new LineRenderer();
        //gunLight = new Light();
    }


}
