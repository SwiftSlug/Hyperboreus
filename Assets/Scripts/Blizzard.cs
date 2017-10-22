using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : MonoBehaviour {

    [Tooltip("Boolean to determin if blizzard effect should be running or not")]
    public bool blizzardActive = false;

    [Tooltip("Maximum intensity that the fog effect should increase to")]
    public float maxFogIntensity = 0.1f;

    [Tooltip("The speed that the fog will change over time")]
    public float fogChangeRate = 0.005f;

    [Tooltip("Maximum emmision rate of the snow particle system")]
    public int maxEmissionRate = 1000;

    [Tooltip("The speed that the emission rate will change over time")]
    public float particleChangeRate = 50.0f;

    [Tooltip("Reference to the player")]
    public GameObject player;
    
    [Tooltip("Should the effect follow the player")]
    public bool followPlayer = false;

    ParticleSystem particles;

    // Use this for initialization
    void Start () {

        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.0f;
        RenderSettings.fog = false;

        particles = GetComponentInChildren<ParticleSystem>();

        var em = particles.emission;
        em.rateOverTimeMultiplier = 0;

        if (followPlayer)
        {
            player = GameObject.FindGameObjectWithTag("NetworkedPlayer");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            if (player)
            {
                transform.position = player.transform.position;
            }
        }

        var em = particles.emission;    //  Reference to the particle emitter

        //Debug.Log("Emmision rate = " + em.rateOverTimeMultiplier);

        //Debug.Log("Blep");

        if (blizzardActive)
        {
            RenderSettings.fog = true;
            particles.Play();

            //  Increase fog density and emission rate each frame if less than max value
            if (RenderSettings.fogDensity < maxFogIntensity)
            {
                RenderSettings.fogDensity += fogChangeRate * Time.deltaTime;
            }
            if (em.rateOverTimeMultiplier < maxEmissionRate)
            {
                em.rateOverTimeMultiplier += (particleChangeRate * Time.deltaTime) * 10;                
            }

        }
        else
        {
            //  Decrease fog density and emission rate each frame if greater than 0
            if (RenderSettings.fogDensity > 0.0f)
            {
                RenderSettings.fogDensity -= fogChangeRate * Time.deltaTime;
            }
            else
            {
                RenderSettings.fogDensity = 0.0f;
                RenderSettings.fog = false;
            }
            if (em.rateOverTimeMultiplier > 0)
            {
                em.rateOverTimeMultiplier -= (particleChangeRate * Time.deltaTime) * 10;
            }
            else
            {
                em.rateOverTimeMultiplier = 0.0f;
                particles.Stop();
            }
        }

        //  **************** Debug **************************

        if (Input.GetButtonDown("Debug2"))
        {
            if (blizzardActive)
            {
                Deactivate();
                Debug.Log("Fog off");
            }
            else
            {
                Activate();
                Debug.Log("Fog on");
            }
        }        

        if (Input.GetButtonDown("FogIncrease"))
        {
            RenderSettings.fogDensity += 0.1f;
            Debug.Log("Fog Density = " + RenderSettings.fogDensity);
        }

        if (Input.GetButtonDown("FogDecrease"))
        {
            RenderSettings.fogDensity -= 0.1f;
            Debug.Log("Fog Density = " + RenderSettings.fogDensity);
        }

        // ***************************************************

    }

    public void Activate()
    {
        blizzardActive = true;
    }

    public void Deactivate()
    {
        blizzardActive = false;
    }

}
