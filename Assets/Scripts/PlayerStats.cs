using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{
	public static int maxHealth = 100;

	[SyncVar]
	public int currentHealth = maxHealth;

	public bool isDead = false;

	public void Damage(int amount)
	{
		currentHealth -= amount;

		if(currentHealth <= 0)
		{
			currentHealth = 0;
			isDead = true;
		}
	}

	public void Heal(int amount)
	{
		currentHealth += amount;

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
		}
	}
}
