using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullpup : WeaponType {

    private void Awake()
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
        //weaponSwitchTransform = this.transform;
        //gunEnd = this.transform;

        //shootRay = new Ray();
        //shootHit = new RaycastHit();

        //gunParticles = new ParticleSystem();
        //gunLine = new LineRenderer();
        //gunLight = new Light();
    }

}
