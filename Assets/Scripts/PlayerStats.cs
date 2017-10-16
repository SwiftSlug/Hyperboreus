using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{
	public RectTransform healthBar;

	[Tooltip("Player max health")]
	[SyncVar]
	public int maxHealth = 100; //Max health of the player, should only change through

	[Tooltip("Current player health")]
	[SyncVar(hook = "ChangeHealth")]
	public int currentHealth; //The player's current health, can go down from being damaged or up from being healed or regenerating.

	[Tooltip("Boolean for player death check")]
	[SyncVar]
	public bool isDead = false; //Used when determining if the player is dead and if they should be revived or respawned

	[Tooltip("Time code at which player was damaged")]
	[SyncVar] 
	public float timeDamaged = 0.0f; //Time code at which player was damaged
	[Tooltip("Regen rate. Quarter of a second Default before each regen tick")]
	[SyncVar]
	public float regenHealthSpeed = 0.25f; //Regen rate. Quarter of a second Default before each regen tick
	[Tooltip("Time in (seconds)before regen kicks in. 10 Second Default")]
	[SyncVar]
    public float regenHealthDelay = 10.0f; //Time in (seconds) before regen kicks in. 10 Second Default

	void Start()
	{
		if (!isLocalPlayer)
		{
			Canvas playerUI = gameObject.GetComponentInChildren<Canvas>();

			if (playerUI != null)
			{
				playerUI.enabled = false;
			}

			return;
		}

        currentHealth = maxHealth;

        CmdStartRegen();
    }

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

        if (Input.GetKeyDown(KeyCode.O))
        {
            CmdDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CmdHeal(20);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            CmdKill();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            CmdRevive();
        }
    }

    void OnTriggerEnter(Collider sphere)
    {
        if (sphere.CompareTag("NetworkedPlayer"))
        {
            if (sphere.GetComponent<PlayerStats>().isDead == true)
            {
                sphere.GetComponent<PlayerStats>().CmdRevive();
            }
        }
    }

	[Command]
	public void CmdDamage(int amount)
	{
		if(!isServer)
		{
			return;
		}

        currentHealth -= amount;

		timeDamaged = Time.time;

		if (currentHealth <= 0)
		{
            CmdKill();
		}
	}

	[Command]
	public void CmdHeal(int amount)
	{
		if (!isServer)
		{
			return;
		}

        if (isDead)
        {
            return;
        }

		currentHealth += amount;

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
		}
	}

    [Command]
    public void CmdKill()
    {
        if (!isServer)
        {
            return;
        }

        isDead = true;

        timeDamaged = Time.time;

        currentHealth = 0;
    }

    [Command]
    public void CmdRevive()
    {
        if (!isServer)
        {
            return;
        }

        isDead = false;

        timeDamaged = Time.time;

        currentHealth = 10;
    }

    [Command]
    public void CmdStartRegen()
    {
        if (!isServer)
        {
            return;
        }

        InvokeRepeating("CmdRegenHealth", 0.0f, regenHealthSpeed);
    }

	[Command]
	public void CmdRegenHealth()
	{
        if (!isServer)
        {
            return;
        }

        if (timeDamaged != 0.0)
        {
            if (currentHealth < maxHealth && Time.time > (timeDamaged + regenHealthDelay) && isDead == false)
            {
                CmdHeal(1);
            }
        }
	}

	void ChangeHealth(int currentHealth)
	{
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
	}
}