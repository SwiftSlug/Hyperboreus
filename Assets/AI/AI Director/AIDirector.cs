using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AIDirector : NetworkBehaviour
{
    public bool blep;

    public bool shouldAIDebug = true;           //  Debug flag for all debugging logs
    public bool isDay = true;                   //  Boolean that defines if it is day or night
    //GameObject[] EnemyUnits;
    public GameObject enemyToSpawn;             //  Enemy type to spawn, limited to one for this stage of the game

    List<GameObject> enemyUnits;                //  List of all enemy units within the game
    List<GameObject> players;                   //  List of all players in the game

    //GameObject[] Players;
    int targetIntensityLevelDay;                //  The intensity level the director aims to keep players at during the day
    int targetIntensityLevelNight;              //  The intensity level the director aims to keep players at during the night
    float waveCooldown;                         //  The cooldown time inbetween waves

    public float playerProximitySize = 50;      //  The area size around the player that detects nearby enemies for intensity checks
    public int intensityPerAI = 10;             //  The amount of intensity each AI unit adds to the player
    float intensityIncreasePercentage = 0.2f;   //  The percentage of the new intensity level added per update
    int intensityDecreaseAmount = 2;            //  The amount of intensity that is decreased when its not increasing
        
    //  Timing Varaibles
    public float intensityUpdateInterval = 3.0f;
    float intensityLastRunTime = 0.0f;
    


    // Use this for initialization
    void Start () {

        Debug.Log("Director Alive !");

        enemyUnits = new List<GameObject>();    // Init AI list
        players = new List<GameObject>();       // Init player list


        //  Search for AI units and store their gameobjects in the list
        int aiCount = 0;
        foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
        {
            enemyUnits.Add(foundAI.gameObject);
            aiCount++;
            //Debug.Log("Enemy Unit Found by Director !");
        }
        if (shouldAIDebug)
        {
            Debug.Log(aiCount + " AI Unit(s) Found");
        }


        int playerCount = 0;
        foreach (PlayerStats foundAI in FindObjectsOfType<PlayerStats>())
        {
            players.Add(foundAI.gameObject);
            playerCount++;
            //Debug.Log("Player Found by director !");
        }
   
        if (shouldAIDebug)
        {
            Debug.Log(playerCount + " Player(s) Found");
        }


        //Invoke("updatePlayerIntensity", 1);
        //Invoke("updatePlayerIntensity", 5);
        //Invoke("updatePlayerIntensity", 15);
        //Invoke("updatePlayerIntensity", 20);
        //Invoke("updatePlayerIntensity", 25);



    }
	
	// Update is called once per frame
	void Update () {

        //intensityLastRunTime = Time.time;
        if (Time.time > (intensityLastRunTime + intensityUpdateInterval))
        {
            //Debug.Log("Player Intensity Updating");
            updatePlayerIntensity();
            intensityLastRunTime = Time.time;
        }
        

    }
    
    void spawnUnits(int number, Vector3 position, GameObject targetPlayer)
    {

        // Spawn the units within random locations near the defined position
        for (int i =0; i < number; i++)
        {
            float xOffset = Random.Range(-10.0f, -10.0f);   //  Random x offset
            float yOffset = Random.Range(-10.0f, -10.0f);   //  Random y offset

            Vector3 spawnPosition = new Vector3(position.x + xOffset, position.y + yOffset, position.y);    //  Generate spawn location

            var spawnedEnemy = (GameObject)Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);   //  Create new AI units
            NetworkServer.Spawn(spawnedEnemy);  //  Add spawned unit to server list
        }

    }


    //  This calculates and updates the intensity level for all players currently within the game
    void updatePlayerIntensity()
    {
        
        int foundAI = 0;        //  The number of AI that are nearby the player
        int trackingAI = 0;     //  The number of AI that are currently targetting the player

        foreach (GameObject player in players)
        {
            PlayerStats statsRef = player.GetComponent<PlayerStats>();

            //  Find all enemies that are near the player
            Collider[] hitCollider = Physics.OverlapSphere(player.transform.position, playerProximitySize);
            for (int i = 0; i < hitCollider.Length; i++)
            {
                if (hitCollider[i].CompareTag("Enemy"))
                {
                    //Debug.Log("Enemy Found Near Player");
                    foundAI++;                    
                }
            }

            //  Find all enemies that are targeting the player
            GameObject[] enemyAI = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemyAI.Length; i++)
            {
                if (enemyAI[i].GetComponent<StateController>().target == player)
                {
                    //Debug.Log("Enemy Tracking Player");
                    trackingAI++;
                }
            }
        


            float intensityLevel = 0.0f;                                //  Overall intensity level

            int nearbyEnemyIntensity = (foundAI * intensityPerAI);      //  0 - infinity based on number of enemies near player
            int healthLost = (100 - statsRef.currentHealth);            //  0 - 100 based on how much health has been lost from 100
            //int ammoIntensity = 


            float healthIntensity = ((float)healthLost / 100) + 1;      //  1.0 + value that multiplies the intensity based on how low the players health is

            //Debug.Log("Health Intensity = " + healthIntensity);

            intensityLevel = nearbyEnemyIntensity * healthIntensity;    //  Finial intensity level based on above atributes


            //  Apply Intensity to player

            if (statsRef.intensity < intensityLevel)
            {
                //  Increase intensity up to intensityLevel per update
                statsRef.intensity += intensityIncreasePercentage * intensityLevel;
            }
            else if (statsRef.intensity > 0)
            {
                //  Decrease instensity gradually back to 0
                statsRef.intensity -= intensityDecreaseAmount;
            }

            //  Ensure that intensity does not fall below 0
            if(statsRef.intensity < 0.0f)
            {
                statsRef.intensity = 0.0f;
            }


        }

    }

    //  Debug functions below ---------------------------------

    void OnGUI()
    {
        if (shouldAIDebug)
        {

            string text = string.Format("Player Intensity Debug \n\n");
            int playerNumber = 0;

            foreach (GameObject player in players)
            {
                //  Get reference to player stats
                PlayerStats statsRef = player.GetComponent<PlayerStats>();

                //  Add player intensity to the print string
                text += "Player " + playerNumber.ToString() + "\n";
                text += "-------------------" + "\n";
                text += "Overall Intensity = " + string.Format(statsRef.intensity.ToString() + "\n");
                //text += "Health Intensity = " + string.Format(statsRef.intensity.ToString() + "\n");

                playerNumber++;
            }

            float height = 400;
            float width = 200;
            GUI.Label(new Rect(Screen.width - width, 0, width, height), text);

            //GUI.Label(new Rect(500, 50, 1000, 1000), text);
        }

    }

    //  This does not appear to work with NetworkBehaviour :|
    void OnDrawGizmos()
    {

        foreach (GameObject player in players)
        {
            Debug.Log("Drawing Sphere");
            //  Get reference to player stats
            PlayerStats statsRef = player.GetComponent<PlayerStats>();

            //Debug.DrawSphere
            //GameObject.CreatePrimitive(PrimitiveType.Sphere);


            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(player.transform.position, 10);
        }

        

    }

}