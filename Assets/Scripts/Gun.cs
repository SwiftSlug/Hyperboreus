using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damagePerShot = 10f;               //Damage each bullet deals to enemies.
    public float timeBetweenShots = 0.15f;          //The time between each shot.
    public float range = 100f;                      //The range that the gun can fire.
    public int maxAmmo = 45;
    public float reloadTime = 1f;
    public Animator animator;
    //public GameObject impactEffect;                 //Particle system at bullet point of impact.


    private int currentAmmo;
    private bool reloading;

    float timer;                                    //Timer to know when you can shoot (used for 'timeBetweenShots').
    Ray shootRay = new Ray();                       //Ray from the gun.
    RaycastHit shootHit;                            //Raycast hit to determine what was hit.                           
    ParticleSystem gunParticles;                    //Reference to the particle system.
    LineRenderer gunLine;                           //Reference to the line renderer.
    Light gunLight;                                 //Reference to the guns light source.
    float effectsDisplayTime = 0.2f;                //The proportion of the timeBetweenShots that the effects which display for.
    //AudioSource gunAudio;

    void Awake()
    {
        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        //gunAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        reloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (reloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && timer >= timeBetweenShots && Time.timeScale != 0)
        {
            Shooting();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets and the effects...
        if (timer >= timeBetweenShots * effectsDisplayTime)
        {
            //.. then disable the effects.
            DisableMuzzleEffects();
        }
    }

    IEnumerator Reload()
    {
        reloading = true;

        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo;

        reloading = false;
    }

    public void DisableMuzzleEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shooting()
    {
        //Resets the timer.
        timer = 0f;

        currentAmmo--;

        //gunAudio.Play();

        //Turns on gun flash (light source).
        gunLight.enabled = true;

        //Stops particles playing if they were already, then starts playing again.
        gunParticles.Stop();
        gunParticles.Play();

        //Enable the line renderer and set its position to the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        //Sets the shootRay so it starts at the gun and points forward.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        //Perform the raycast 
        if (Physics.Raycast(shootRay, out shootHit, range))
        {
            //code in place for damaging enemy health... uncomment and adjust when enemy 
            //EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            //if (enemyHealth != null)
            //{
            //    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            //}
            gunLine.SetPosition(1, shootHit.point);
            //GameObject impactObject = Instantiate(impactEffect, shootHit.point, Quaternion.LookRotation(shootHit.normal));
            //Destroy(impactObject, 2f);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
