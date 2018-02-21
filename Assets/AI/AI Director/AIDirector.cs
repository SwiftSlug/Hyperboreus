using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEditor.AI;
using UnityEngine.AI;

public class AIDirector : NetworkBehaviour
{
    //public bool blep;

    public bool shouldAIDebug = false;          //  Debug flag for all debugging logs
    public bool isDay = true;                   //  Boolean that defines if it is day or night
    //GameObject[] EnemyUnits;
    public GameObject enemyToSpawn;             //  Enemy type to spawn, limited to one for this stage of the game

    List<GameObject> enemyUnits;                //  List of all enemy units within the game
    List<GameObject> players;                   //  List of all players in the game

    List<Vector3> spawnLocations;               //  List of avalible spawn locations for the AI

    int maxAiCount = 100;                       //  The max amount of AI that can be active before group spawning stops

    float spawnBufferSize = 2.0f;               //  The area size that must be free of objects to count as an AI spawn location

    //GameObject[] Players;
    public int targetIntensityLevelDay = 20;    //  The intensity level the director aims to keep players at during the day
    public int targetIntensityLevelNight = 200; //  The intensity level the director aims to keep players at during the night
    public float waveCooldown;                  //  The cooldown time inbetween waves

    public float playerProximitySize = 50;      //  The area size around the player that detects nearby enemies for intensity checks
    public int intensityPerAI = 10;             //  The amount of intensity each AI unit adds to the player
    float intensityIncreasePercentage = 0.2f;   //  The percentage of the new intensity level added per update
    public int intensityDecreaseAmount = 20;            //  The amount of intensity that is decreased when its not increasing
        
    //  Timing Varaibles
    public float intensityUpdateInterval = 3.0f;    //  The time interval between updating the player intensity level
    float intensityLastRunTime = 0.0f;              //  The last time the intensity update was ran


    public float spawnInterval = 5.0f;           //  The time interval between spawning groups of enemies
    public float spawnLast = 0.0f;               //  The last time the AI were spawned
    public int aiSpawnGroupSizeNight = 10;       //  The amount of AI to spawn per group at day
    public int aiSpawnGroupSizeDay = 1;          //  The amount of AI to spawn per group at night

    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);   //  Debug cube used as a marker
    public GameObject cube; 

    // Use this for initialization
    void Start () {

        Debug.Log("Director Alive !");

        //  Init all lists ready for use

        enemyUnits = new List<GameObject>();    // Init AI list
        players = new List<GameObject>();       // Init player list
        spawnLocations = new List<Vector3>();   // Init spawn Location list

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

        
        

        Debug.Log("Director Init Complete");

    }
	
	// Update is called once per frame
	void Update () {

        //  Update List with new enemy count
        rescanForAI();
        
        //  Spawning

        //  Run spawning function if cooldown is up
        if (Time.time > (spawnLast + spawnInterval))
        {
            spawnEnemies();
            spawnLast = Time.time;
        }        

        //  Intensity Updates

        //  Run intensity update if the update interval has been passed   
        if (Time.time > (intensityLastRunTime + intensityUpdateInterval))
        {
            //Debug.Log("Player Intensity Updating");
            updatePlayerIntensity();
            intensityLastRunTime = Time.time;
        }
        //  Spawn enemies if the spawn interval has been passed
        


        //  Debug Stuff

        /*
        if (Input.GetKeyDown("y"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("NetworkedPlayer");

            scanSpawnAreas(player.transform.position, 60, 25, 5);
        }
        */


    }

    //  Scan Spawn Areas
    //
    //  This function is used to scan a defined area to generate a list of spawnable locations for the AI units. It does this by first generating a random location
    //  vector within a set area and ignoring the inner part of the area (this stops at from spawning too close to the players). This found area is then checked for
    //  any obsticles (aside from floors), if none area found the area is clear to spawn. If a collider is found then another random location is generated and checked.
    //  This is limited up to a defined amount (maxRunAttemps) to stop areas that cann be spawned in cuasing infite loops

    bool scanSpawnAreas(Vector3 areaCentre, float areaSize, float centerIgnoreSize, int numberOfSpawnLocatoins, int maxRunAttempts = 200)
    {

        int maxRunCounter = 0;
        //  Remove all old spawn locations
        spawnLocations.Clear();

        for(int i = 0; i < numberOfSpawnLocatoins; i++)
        {
            //  Only run this loop up to maxRunAttemps, prevents an unspawnable area causing an infinite loop
            maxRunCounter = 0;
            while (maxRunCounter < maxRunAttempts)
            {
                //  Generate a random position offset within the input area size
                float xPos = Random.Range(-areaSize, areaSize);
                float zPos = Random.Range(-areaSize, areaSize);

                // Ensure position cannot be inside of ingore radius
                // *************** This causes the spawning to ignore a + shape within worldspace

                if (xPos > 0)
                {
                    xPos += centerIgnoreSize;
                }
                else
                {
                    xPos -= centerIgnoreSize;
                }

                if (zPos > 0)
                {
                    zPos += centerIgnoreSize;
                }
                else
                {
                    zPos -= centerIgnoreSize;
                }

                //  Add offset positions to the area center
                xPos += areaCentre.x;
                zPos += areaCentre.z;

                //  Create new spawn location from generated values above
                Vector3 spawnLocation = new Vector3(xPos, 0.0f, zPos);             
                

                //  Find all colliders at the random location
                Collider[] hitColliders = Physics.OverlapSphere(spawnLocation, spawnBufferSize);

                //  This section may be improved to use a layer mask rather than comparing tags for each hit

                //  Run through all found colliders
                bool areaClear = true;
                int hits = 0;
                while (hits < hitColliders.Length)
                {
                    // if anything other than the floor is found then the area is not suitable
                    if (!hitColliders[hits].CompareTag("floor"))
                    {
                        areaClear = false;
                    }                    
                    hits++;
                }
                if (areaClear == true)
                {                    

                    //Debug.Log("Clear spawn area found !");
                    //  Spawn debug cube at spawn location
                    //GameObject debugCube = Instantiate(cube, spawnLocation, Quaternion.identity);
                    //debugCube.GetComponent<SphereCollider>().radius = spawnBufferSize;

                    NavMeshHit navMeshHit;
                    

                    //  Scan for a navmesh position at the random location
                    if(NavMesh.SamplePosition(spawnLocation, out navMeshHit, spawnBufferSize, NavMesh.AllAreas))
                    {
                        //Debug.Log("Location with navmesh found !");
                        //NavMesh foundNavMesh = navMeshHit;
                        bool canPathToPlayer = true;
                        //  Can the path reach player 0
                        NavMeshPath pathToPlayer = new NavMeshPath();
                        //NavMesh.CalculatePath(navMeshHit.position, players[0].transform.position, NavMesh.AllAreas, pathToPlayer);

                        foreach(GameObject player in players)
                        {
                            NavMesh.CalculatePath(navMeshHit.position, players[0].transform.position, NavMesh.AllAreas, pathToPlayer);
                            if (pathToPlayer.status != NavMeshPathStatus.PathComplete)
                            {
                                canPathToPlayer = false;
                            }
    
                        }

                        if (canPathToPlayer)
                        {
                            //Debug.Log("Area can navigate to player");
                            //  All checks passed, add found spawn location to spawn list
                            spawnLocations.Add(spawnLocation);
                            // Break out of while loop as spawn location has been found
                            maxRunCounter = maxRunAttempts;

                            //GameObject debugCube = Instantiate(cube, spawnLocation, Quaternion.identity);
                        }
                        
                    }


                    

                }
                maxRunCounter++;
            }
            if (maxRunCounter == maxRunAttempts)
            {
                //  Spawnable area list could not be populated 
                Debug.Log("Area could not be found !");
                return false;
            }
        }
        return false;
    }

    void rescanForAI()
    {
        //  Search for AI units and store their gameobjects in the list

        enemyUnits.Clear(); //  Clear list so no old objects are kept

        foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
        {
            if (foundAI.CompareTag("Enemy"))
            {
                enemyUnits.Add(foundAI.gameObject); //  Add found units to list
            }
        }

    }

    //  Spawn enemies near the players
    void spawnEnemies()
    {        
        int activeAICount = enemyUnits.Count;

        if (activeAICount < maxAiCount)
        {
            //  Spawn more AI units near the player
            foreach (GameObject player in players)
            {
                scanSpawnAreas(player.transform.position, 60, 25, 5);
                if (isDay)
                {
                    //  Day time spawning
                    if (player.GetComponent<PlayerStats>().intensity < targetIntensityLevelDay)
                    {
                        for (int i = 0; i < aiSpawnGroupSizeNight; i++)
                        {
                            int randomLocation = Random.Range(0, spawnLocations.Count);
                            if (spawnLocations[randomLocation] != null)
                            {
                                spawnUnits(aiSpawnGroupSizeDay, spawnLocations[randomLocation], player);
                            }
                        }
                    }
                }
                else
                {
                    //  Night time spawning
                    if (player.GetComponent<PlayerStats>().intensity < targetIntensityLevelNight)
                    {
                        spawnUnits(aiSpawnGroupSizeNight, player.transform.position, player);
                    }
                }

            }

        }
    }


    //  Spawn a number of AI units around a position and set them to target the provided player
    void spawnUnits(int number, Vector3 position, GameObject targetPlayer)
    {

        // Spawn the units within random locations near the defined position
        for (int i =0; i < number; i++)
        {
            float xOffset = Random.Range(-10.0f, -10.0f);   //  Random x offset
            float zOffset = Random.Range(-10.0f, -10.0f);   //  Random y offset

            Vector3 spawnPosition = new Vector3(position.x + xOffset, position.y + 0.5f, position.z + zOffset);    //  Generate spawn location

            var spawnedEnemy = (GameObject)Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);   //  Create new AI units
            spawnedEnemy.GetComponent<StateController>().moveCommandLocation = targetPlayer.transform.position;
            spawnedEnemy.GetComponent<StateController>().target = targetPlayer;
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

            //int ammoIntensity =                                       //  ******************* Ammo values to be added later ****************
            
            float healthIntensity = ((float)healthLost / 100) + 1;      //  1.0 + value that multiplies the intensity based on how low the players health is


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

            //  Player debug -----------------------------------------------

            string playerText = string.Format("Player Intensity Debug \n\n");
            int playerNumber = 0;

            foreach (GameObject player in players)
            {
                //  Get reference to player stats
                PlayerStats statsRef = player.GetComponent<PlayerStats>();

                //  Add player intensity to the print string
                playerText += "Player " + playerNumber.ToString() + "\n";
                playerText += "-------------------" + "\n";
                playerText += "Overall Intensity = " + string.Format(statsRef.intensity.ToString() + "\n");
                //text += "Health Intensity = " + string.Format(statsRef.intensity.ToString() + "\n");

                playerNumber++;
            }

            float playerHeight = 400;
            float playerWidth = 200;
            GUI.Label(new Rect(Screen.width - playerWidth, 0, playerWidth, playerHeight), playerText);


            //  Enemy Debug -----------------------------------------------

            string enemyText = string.Format("Enemy AI Debug \n\n");
            int enemyNumber = 0;

            foreach (GameObject enemy in enemyUnits)
            {                            
                enemyNumber++;
            }

            enemyText += "Active enemy = " + enemyNumber + "\n";

            float enemyHeight = 400;
            float enemyWidth = 200;

            GUI.Label(new Rect(Screen.width - enemyWidth - playerWidth, 0, enemyWidth, enemyHeight), enemyText);


            //  Spawn Location Debug Printout --------------------------------------

            string spawnLocationText = string.Format("Enemy Spawn Locations \n\n");
            int locationCount = 0;
            foreach (Vector3 location in spawnLocations)
            {
                spawnLocationText += "Spawn Location " + locationCount + " " + location + "\n";

                locationCount++;
            }

            float locationHeight = 400;
            float locationWidth = 250;
            float locationGap = 200;

            GUI.Label(new Rect(Screen.width - locationWidth - locationGap - playerWidth, 0, locationWidth, locationHeight), spawnLocationText);

        }

    }

    //  OnDrawGizmos do work, but only within the scene view !
    void OnDrawGizmos()
    {
        /*
        foreach (GameObject player in players)
        {
            //Debug.Log("Drawing Sphere");
            //  Get reference to player stats
            PlayerStats statsRef = player.GetComponent<PlayerStats>();

            //Debug.DrawSphere
            //GameObject.CreatePrimitive(PrimitiveType.Sphere);


            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(player.transform.position, 10);
        }
        */
        

    }

}