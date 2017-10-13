using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{
	[Tooltip("Player max health")] 
	public int maxHealth = 100; //Max health of the player, should only change through

	[Tooltip("Current player health")]
	[SyncVar]
	public int currentHealth; //The player's current health, can go down from being damaged or up from being healed or regenerating.

	[Tooltip("Boolean for player death check")]
	public bool isDead = false; //Used when determining if the player is dead and if they should be revived or respawned

	[Tooltip("Time code at which player was damaged")] 
	public float timeDamaged = 0.0f; //Time code at which player was damaged
	[Tooltip("Regen rate. Quarter of a second Default before each regen tick")]
	public float regenHealthSpeed = 0.25f; //Regen rate. Quarter of a second Default before each regen tick
	[Tooltip("Time in (seconds)before regen kicks in. 10 Second Default")]
	public float regenHealthDelay = 10.0f; //Time in (seconds) before regen kicks in. 10 Second Default

	void Start()
	{
		currentHealth = maxHealth;

        InvokeRepeating("RegenHealth", 0.0f, regenHealthSpeed);
	}

	void Update()
	{
		if (currentHealth <= 0)
		{
			isDead = true;
		}
	}

	public void Damage(int amount)
	{
		currentHealth -= amount;
		timeDamaged = Time.time;

		if (currentHealth <= 0)
		{
			currentHealth = 0;
		}
	}

	public void Heal(int amount)
	{
		currentHealth += amount;

		if (currentHealth >= maxHealth && isDead == false)
		{
			currentHealth = maxHealth;
		}
	}

	public void RegenHealth()
	{
		if (currentHealth < maxHealth && Time.time > (timeDamaged + regenHealthDelay) && isDead == false)
		{
			Heal(1);
		}
	}
}