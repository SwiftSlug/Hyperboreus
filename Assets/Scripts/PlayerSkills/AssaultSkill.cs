using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssaultSkill : Skill
{
    public float chargeTime;            // Time it takes to charge the syringe shot

    public float airstrikeTime;         // Time taken before airstrike hits

    float currentChargeTime;            // The current time spent charging the ability

    RaycastHit markerY;                 // Raycast hit point

    Ray markerRay = new Ray();          // Raycast for marker location point

    public ParticleSystem smokeMarker;  // Particle system for smoke marker

    public Transform MarkerLocation;    // Transform of marker location (structure placement object to be used as point)

	public Image BuilderIcon;
	public Image AssaultIcon;
	public Image MedicIcon;
	public Image SkillRecharge;

	//void Update()
	//{
	//    Debug.Log("INIT");
	//    Vector3 mousePosUpdate = Input.mousePosition;
	//}

	public override void Init()
    {
        if (cooldown == 0.0f)
        {
            //  Sets the cooldown to 2 seconds if no other value is set
            cooldown = 2.0f;
        }

        if (chargeTime == 0.0f)
        {
            //  Sets the chargetime to 2 seconds if no other value is set
            chargeTime = 0.5f;
        }

		AssaultIcon.enabled = true;
		MedicIcon.enabled = false;
		BuilderIcon.enabled = false;
	}
	void Update()
	{
		if (SkillRecharge.fillAmount <= 1.0f)
		{
			SkillRecharge.fillAmount = (Time.time - (lastUsedTime + cooldown));
		}
	}

	public override bool SkillAction()
    {
        if (isLocalPlayer)
        {
            // Check if the cooldown period has finished
            if (Time.time > lastUsedTime + cooldown)
            {
                currentChargeTime += Time.deltaTime;            // Get current time value when charge up starts

                // draw marker on mouse location update
                //Vector3 mousePosUpdate = Input.mousePosition;
                //mousePosUpdate = mousePosUpdate.y + 50;
                //markerRay.origin = mousePosUpdate;
                //markerRay.direction = -(transform.up);

                markerRay.origin = MarkerLocation.position + new Vector3(0, 350f, 0);   // Set origin of the raycast to desired X+Z location + large Y value 
                markerRay.direction = -(gameObject.transform.up);                       // Set raycast direcion downwards

                if (Physics.Raycast(markerRay.origin, markerRay.direction, out markerY, 100))
                {
                    Vector3 targetLocation = markerY.point;     // Set raycast hit location for where smoke marker is placed
                    //Debug.Log(targetLocation);
                }

                //Debug.Log(mousePosUpdate.x);
                //Debug.Log(mousePosUpdate.y);
                //Debug.Log(mousePosUpdate.z);
                //Debug.Log("drawing marker");


                // When ability has finished charging
                if (currentChargeTime > chargeTime)
                {
                    //smokeMarker.SetPosition(1, targetLocation);
                    //particleMarker.Stop();
                    //particleMarker.Play();
                    //Vector3 mousePos = Input.mousePosition;

                    // wait for air strike time
                    // delete marker

                    //RpcSmokeMarker();

                    CmdSpawnStrike(markerRay.origin, transform.rotation, playerReference);  // Call command for spawning missile

                    //Debug.Log("BOOM");
                    currentChargeTime = 0.0f;   //  Reset the current charge time
                    lastUsedTime = Time.time;   //  Set last firing time
                    return true;
                }
            }
        }
        return false;
    }

    public override void buttonRelease()
    {
        currentChargeTime = 0.0f;       //  Reset the current charge time  
        //Debug.Log("release");
    }

    [Command]
    void CmdSpawnStrike(Vector3 spawnPosition, Quaternion spawnRotation, GameObject currentPlayerReference)
    {
        //Debug.Log("strike");
        //smokeMarker.Play();
        //RpcSmokeMarker();
        GameObject missile = Resources.Load("Missile", typeof(GameObject)) as GameObject;

        GameObject missileRef = Instantiate(missile, spawnPosition, spawnRotation);

        //  Assign player reference on scripts
        //MissileRef.GetComponentInChildren<MedicalSyringeScript>().player = currentPlayerReference;
        NetworkServer.Spawn(missileRef);
    }

    [Command]
    public void CmdSmokeMarker()
    {
        if (isServer)
        {
            smokeMarker.Play();
            RpcSmokeMarker();
        }
    }

    [ClientRpc]
    public void RpcSmokeMarker()
    {
        smokeMarker.Play();
    }
}