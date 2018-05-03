using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DayNightCycle : NetworkBehaviour
{
    public Light directionalLight;

    [HideInInspector]
    public Color lightColor;

    [HideInInspector]
    public float lightIntensity;

    //Amount of days the player has survived
    [SyncVar]
    public int daysSurvived = 0;

    //Value in seconds for the length of day
    [SyncVar]
    public float dayLength = 180.0f; //Seconds

    //Value in seconds for the length of day
    [SyncVar]
    public float nightLength = 180.0f; //Seconds

    [SyncVar]
    public float currentTime = 0.40f;

    [SyncVar]
    public float daySpeed = 1.0f;
    [SyncVar]
    public float nightSpeed = 1.0f;
	[SyncVar]
	public float globalSpeed = 1.0f;

    [SyncVar]
    public bool isDay = true;
    [SyncVar]
    public bool isNight = false;

    bool directorUpdated = false;    //  Flag to stop director calls triggering per frame

    AIDirector directorReference;    //  Reference to director
	[SyncVar]
	private bool dirtyFlag = true;

	// Use this for initialization
	void Start ()
    {

        lightColor = directionalLight.color;
        lightIntensity = directionalLight.intensity;

    }

    private void Awake()
    {
        directorReference = FindObjectOfType<AIDirector>(); //  Find and set director reference
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateLighting();

        UpdateTime();

        DirectorUpdate();

    }

    void UpdateLighting()
    {
        float lightMultiplier = 1.0f;

        directionalLight.transform.localRotation = Quaternion.Euler((currentTime * 360.0f) - 90, 170, 0);

        if (currentTime < 0.23f || currentTime > 0.75f)
        {
            lightMultiplier = 0.25f;
        }
        else if (currentTime < 0.25f)
        {
            lightMultiplier = Mathf.Clamp01((currentTime - 0.23f) * (1.0f / 0.02f));
        }
        else if (currentTime > 0.73f)
        {
            lightMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.73f) * (1.0f / 0.02f)));
        }


        directionalLight.color = lightColor;
        directionalLight.intensity = lightIntensity * lightMultiplier;
    }

    void UpdateTime()
    {
        if (currentTime >= 1.0f)
        {
            currentTime = 0.0f;
        }

        if (currentTime >= 0.25f && currentTime <= 0.75f)
        {
			if (!dirtyFlag)
			{
				daysSurvived++;
				dirtyFlag = true;
			}

			isDay = true;
            isNight = false;

        }
        else if (currentTime > 0.75f || currentTime < 0.25f)
        {
			dirtyFlag = false;
            isNight = true;
            isDay = false;

        }
        else
        {
            Debug.LogError("Error in DayNightCycle. Cannot set boolean for time of day. 'currentTime' may be wrong.");
        }

        if (isDay)
        {
            currentTime += (((Time.deltaTime / dayLength) / 2) * daySpeed) * globalSpeed;
        }
        else if (isNight)
        {
            currentTime += (((Time.deltaTime / nightLength) / 2) * nightSpeed) * globalSpeed;
        }
        else
        {
            Debug.LogError("Error in DayNightCycle. Time is neither day or night.");
        }
    }

    void DirectorUpdate()
    {

        //Debug.LogError("DirectorUpdate Script Ran");

        //if (!isServer)
        //{
        //    //  Only the server should update the director
        //    return;
        //}

        if (directorReference == null)
        {
            //  Clients will not be able to get a director reference so do not continue
            return;
        }

        //Debug.Log("Director isDay = " + directorReference.isDay);
        //Debug.Log("isDay = " + isDay);
        //Debug.Log("isNIght = " + isNight);

        if (isDay)
        {
            //  Day time

            if (directorReference.isDay != isDay)
            {
                //  Director not set to day so update
                directorReference.SetDay();
            }
        }
        else
        {
            //  Night time

            if(directorReference.isDay != isDay)
            {
                //  Director is still set to day so switch to night 
                
                directorReference.SetNight();
            }
        }
        
    }

}