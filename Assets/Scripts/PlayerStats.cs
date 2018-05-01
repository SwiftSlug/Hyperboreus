using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
	public Image healthBar; //Store a reference to the health bar transform so it can change in fill amount
	public Text healthText; //The text displayed for player's current health
	public Text healthBackground; //The white stroke to our text makes it stand out some more

	public Image TimerHand;
	public Image CenterPoint;

	public Text WoodText; //The text displayed for player's current wood amount
	public Text WoodTextBackground; //The white stroke to our text makes it stand out some more
	public Text StoneText; //The text displayed for player's current stone amount
	public Text StoneTextBackground; //The white stroke to our text makes it stand out some more
	public Text MetalText; //The text displayed for player's current metal amount
	public Text MetalTextBackground; //The white stroke to our text makes it stand out some more

	private GameObject DayNightController;

	public RectTransform gameOverOverlay; //Game Over screen to be displayed when all players are downed

	[Tooltip("Player maximum health")]
	[SyncVar]
	public int maxHealth = 100; //Max health of the player, should only change through framework

	[Tooltip("Current player health")]
	[SyncVar(hook = "ChangeHealth")]
	public int currentHealth; //The player's current health, can go down from being damaged or up from being healed or regenerating

	[Tooltip("Boolean for player death check")]
	[SyncVar]
	public bool isDead = false; //Used when determining if the player is dead and if they should be revived or respawned

	[Tooltip("Time code at which player was damaged")]
	[SyncVar] 
	public float timeDamaged = 0.0f; //Time code at which player was damaged

	[Tooltip("Regen rate. Quarter of a second Default before each regen tick")]
	[SyncVar]
	public float regenHealthSpeed = 0.25f; //Regen rate. Quarter of a second Default before each regen tick

	[Tooltip("Time in (seconds) before regen kicks in. 10 Second Default")]
	[SyncVar]
    public float regenHealthDelay = 10.0f; //Time in (seconds) before regen kicks in. 10 Second Default

    [Tooltip("Time taken to revive another player. 5 Second Default")]
    [SyncVar]
    public float revivalTime = 5.0f; //Time in (seconds) before the receiving player is revive. 5 Second Default

    [Tooltip("Can this player revive another player?")]
    [SyncVar]
    public bool canRevive = false; //Boolean to check whether we can revive a player using the CmdRevive method

    [Tooltip("Store the player we collided with so we can revive them")]
    [SyncVar]
    GameObject collidedPlayer = null; //Placeholder to store a reference with a collided player, we use this to call revive on them

    public GameObject manager = null;

    public float intensity;

	// player inventory //
	//resource inventory
	public int WoodInInventory = 20; //Player's WoodCount
	public int StoneInInventory = 30; //Player's StoneCount
	public int MetalInInventory = 70; //Player's MetalCount
    public int ResourceChoice = 0;

    //weapon & ammo inventory
    public GameObject pistol = null;
    public GameObject rifle = null;
    public GameObject shotgun = null;
    public GameObject sniper = null;
    public GameObject rocketLauncher = null;

    public int pistolAmmo = 0;
    public int rifleAmmo = 0;
    public int shotgunAmmo = 0;
    public int sniperAmmo = 0;
    public int rocketAmmo = 0;

    void Start()
	{
        //If we are not the local player then disable all other canvas' so we do not see what they see
		if (!isLocalPlayer)
		{
			Canvas playerUI = gameObject.GetComponentInChildren<Canvas>();

			if (playerUI != null)
			{
				playerUI.enabled = false;
			}

			return;
		}

        currentHealth = maxHealth; //Set the player's health to their maximum health locally

        CmdStartRegen(); //Invoke our regen function on the server which will check if enough time has passed to start regenerating player health

		DayNightController = GameObject.FindGameObjectWithTag("DayNightController");

		WoodText.text = "" + WoodInInventory;
		WoodTextBackground.text = "" + WoodInInventory;
		StoneText.text = "" + StoneInInventory;
		StoneTextBackground.text = "" + StoneInInventory;
		MetalText.text = "" + MetalInInventory;
		MetalTextBackground.text = "" + MetalInInventory;
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (DayNightController != null)
		{
			TimerHand.rectTransform.RotateAround(CenterPoint.rectTransform.position, Vector3.back, DayNightController.GetComponent<DayNightCycle>().globalSpeed * Time.deltaTime);
		}

		//DEBUG USE
		if (Input.GetKeyDown(KeyCode.O)) //Damage Key
        {
            CmdDamage(20); //Call the damage method on the server to damage the player by 20
        }
        if (Input.GetKeyDown(KeyCode.P)) //Heal Key
        {
            CmdHeal(20); //Call the heal method on the server to heal the player by 20
        }
        if (Input.GetKeyDown(KeyCode.K)) //Kill Key
        {
            CmdKill(); //Call the kill method on the server to down the player
        }
        if (Input.GetKeyDown(KeyCode.L)) //Revive Self Key
        {
            CmdRevive(); //Call the revive method on the server to revive the downed player with 10 health
        }
        if (Input.GetKey(KeyCode.E)) //Interact Key is being held down
        {
            CmdPlayerRevive(); //If the 'E' key is being held down, call the player revival method which will try and revive another player if conditions are met
        }



        //Check if player is dead locally
        if (isDead)
        {
            GetComponent<PlayerController>().enabled = false; //Disable the player's movement locally
        }
        else
        {
            GetComponent<PlayerController>().enabled = true; //Enable the player's movement locally
        }
    }

    //Check if trigger colliding sphere is overlapping with another collider
    void OnTriggerStay(Collider otherPlayer)
    {
        //If the overlap is of a tagged network player
        if (otherPlayer.CompareTag("NetworkedPlayer"))
        {
            //And the player is dead
            if (otherPlayer.GetComponent<PlayerStats>().isDead == true)
            {
                collidedPlayer = otherPlayer.gameObject; //Store the other player so we can call revive on them
                    
                canRevive = true; //Allow this current player to revive them if needed
            }
            else
            {
                canRevive = false;
            }
        }
    }

    //Call this on the server
    [Command]
    public void CmdPlayerRevive()
    {
        if (!isServer)
        {
            return;
        }
        
        //If the player trying to revive another is dead, don't allow this
        if (isDead)
        {
            return;
        }

        //If we are allowed to revive the player
        if (canRevive == true)
        {
            revivalTime -= Time.deltaTime; //Count down from the set revival time using time between frames

            //When the countdown reaches 0
            if (revivalTime <= 0)
            {
                collidedPlayer.GetComponent<PlayerStats>().CmdRevive(); //Revive the other player using our stored reference.

                revivalTime = 5.0f; //Reset our revival time in case their is another player to revive
            }
        }
        else
        {
            revivalTime = 5.0f;
        }
    }

    //Call this for the player on the server
    [Command]
	public void CmdDamage(int amount)
	{
		if (!isServer)
		{
			return;
		}

        currentHealth -= amount; //Set the server player's health to a reduced amount given via argument to the method

		timeDamaged = Time.time; //Set a timestamp at current time for the server player, used for calculating regen time.

        //If the server player's health reaches 0
		if (currentHealth <= 0 && isDead == false)
		{
            CmdKill(); //Send the command to the server to kill/down the player
        }
	}

    //Call this for the player on the server
	[Command]
	public void CmdHeal(int amount)
	{
		if (!isServer)
		{
			return;
		}

        //If the player is dead then we should not be able to heal them
        if (isDead)
        {
            return;
        }

		currentHealth += amount; //Increase the player's health by incremented amount given via argument to the method

        //Make sure the player's health does not exceed their maximum
		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
		}
	}

    //Call this command for the player on the server
    [Command]
    public void CmdKill()
    {
        if (!isServer)
        {
            return;
        }

        isDead = true; //Set our boolean to show that player is dead

        GetComponent<GameOver>().CmdCheckDead();

        GetComponent<PlayerController>().enabled = false; //Disable the player's movement server side

        currentHealth = 0; //Set the player's current health to 0 on the server
    }

    //Call this command for the player on the server
    [Command]
    public void CmdRevive()
    {
        if (!isServer)
        {
            return;
        }

        GetComponent<PlayerController>().enabled = true; //Enable the player's movement server side

        isDead = false; //Reset our boolean so the player is "alive"

        timeDamaged = Time.time; //Timestamp this so we can start regeneration when needed

        currentHealth = 10; //Set the player's revived health
    }

    //Call the regen on the server for the player
    [Command]
    public void CmdStartRegen()
    {
        if (!isServer)
        {
            return;
        }

        InvokeRepeating("CmdRegenHealth", 0.0f, regenHealthSpeed); //Call the server regen health method that repeats each tick depending on regenHealthSpeed value
    }

    //Regenerate health on the server
    [Command]
    public void CmdRegenHealth()
    {
        if (!isServer)
        {
            return;
        }

        //If we have a timestamp (prevents regen starting needlessly starting when the game starts)
        if (timeDamaged != 0.0)
        {
            //If; on the server, the player's current health is less than their maximum & the time has increased by the timestamped amount + a specified delay (10 seconds) & the player is not dead
            if (currentHealth < maxHealth && Time.time > (timeDamaged + regenHealthDelay) && isDead == false)
            {
                CmdHeal(1); //Heal the player for 1 each call of this method
            }
        }
    }

    //Using the SyncVar hook, this method is called each time the current health value is synchronized between server and client
    void ChangeHealth(int currentHealth)
    {
		healthBar.fillAmount = currentHealth / 100.0f; //Update the health bar for the player, this will change the radial fill of the image
		healthText.text = "" + currentHealth;
		healthBackground.text = "" + currentHealth;
	}

	[Command]
    public void CmdDebugResourceValue(int ResourceNeeded)
    {
        switch(ResourceNeeded)
        {
            case 0:
                Debug.Log("Amount of wood: " + WoodInInventory);
                WoodInInventory = WoodInInventory + 10;
                break;
            case 1:
                Debug.Log("Amount of Stone: " + StoneInInventory);
                StoneInInventory = StoneInInventory + 10;
                break;
            case 2:
                Debug.Log("Amount of Metal: " + MetalInInventory);
                MetalInInventory = MetalInInventory + 10;
                break;
        }
    }
}