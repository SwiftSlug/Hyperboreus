using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BarbedWireTrapSkill : Skill
{
    //  Time it takes to charge the barbed wire trap
    public float chargeTime;
    //  The current time spent charging the barbed wire trap
    float currentChargeTime;

    public bool check = false;

    public Transform TrapPos;

	public Image BuilderIcon;
	public Image AssaultIcon;
	public Image MedicIcon;
	public Image SkillRecharge;

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
            chargeTime = 1.0f;
        }

		BuilderIcon.enabled = true;
		AssaultIcon.enabled = false;
		MedicIcon.enabled = false;
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
            if (Time.time > lastUsedTime + cooldown)
            {
                currentChargeTime += Time.deltaTime;

                if (currentChargeTime > chargeTime)
                {
                    CmdSpawnBarbedWireTrap(TrapPos.position, transform.rotation);                     //, playerReference);

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
    }

    [Command]
    void CmdSpawnBarbedWireTrap(Vector3 spawnPosition, Quaternion spawnRotation) //, GameObject currentPlayerReference)
    {
        GameObject BarbedWireTrap = Resources.Load("BarbedWireTrap", typeof(GameObject)) as GameObject;

        GameObject BarbedWireTrapRef = (GameObject)Instantiate(BarbedWireTrap, spawnPosition, spawnRotation);

        NetworkServer.Spawn(BarbedWireTrapRef);

    }

}

