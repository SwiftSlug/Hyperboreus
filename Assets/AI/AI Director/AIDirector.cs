using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEditor.AI;
using UnityEngine.AI;

public class AIDirector : NetworkBehaviour
{
    //public bool blep;
    public bool active = false;
    
    public bool shouldAIDebug = false;          //  Debug flag for all debugging logs
    public bool shouldAIDrawDebug = true;       //  Debug flag for drawing debug spheres
    public bool shouldAICreateSpawnDebug = true;    //  Debug flag for drawing spawn area cubes
    public bool isDay = true;                   //  Boolean that defines if it is day or night
    //GameObject[] EnemyUnits;
    public GameObject enemyToSpawn;             //  Enemy type to spawn, limited to one for this stage of the game

    List<GameObject> enemyUnits;                //  List of all enemy units within the game
    List<GameObject> players;                   //  List of all players in the game

    List<Vector3> spawnLocations;               //  List of avalible spawn locations for the AI

    int maxAiCount = 100;                       //  The max amount of AI that can be active before group spawning stops
    int numerOfSpawnGroups = 5;                 //  The number of groups that are spawned per wave
    public int aiSpawnGroupSizeNight = 5;      //  The amount of AI to spawn per group at day
    public int aiSpawnGroupSizeDay = 1;         //  The amount of AI to spawn per group at night
    public int numberOfSpawnLocations = 10;     //  The number of spawn locaitons generated per search

    //  Area size definitions
    public float spawnBufferSize = 2.0f;             //  The area size that must be free of objects to count as an AI spawn location
    public float playerProximitySize = 50.0f;        //  The area size around the player that detects nearby enemies for intensity checks
    public float maxDistanceFromPlayers = 100.0f;    //  The max distance an AI unit can be from the player before being deleted

    public int targetIntensityLevelDay = 40;    //  The intensity level the director aims to keep players at during the day
    public int targetIntensityLevelNight = 400; //  The intensity level the director aims to keep players at during the night
    public float waveCooldown;                  //  The cooldown time inbetween waves

    //  Intensity variables
    public int intensityPerAI = 5;             //  The amount of intensity each AI unit adds to the player
    public int intensityPerTrackingAI = 10;      //  The amount of intensity each AI unit tracking the player applies 
    float intensityIncreasePercentage = 0.2f;   //  The percentage of the new intensity level added per update
    public int intensityDecreaseAmount = 20;    //  The amount of intensity that is decreased when its not increasing
    

    //  Timing Varaibles
    public float intensityUpdateInterval = 3.0f;    //  The time interval between updating the player intensity level
    float intensityLastRunTime = 0.0f;              //  The last time the intensity update was ran

    public float cleanupInterval = 10.0f;       //  The time interval between runnning AI cleanups
    float cleanupLastRunTime = 0.0f;            //  The last time cleanup was ran

    public float spawnInterval = 5.0f;           //  The time interval between spawning groups of enemies
    public float spawnLast = 0.0f;               //  The last time the AI were spawned
    

    // Debug Variables    

    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);   //  Debug cube used as a marker
    public GameObject debugSpawnCube;
    public GameObject cube;

    int nearbyEnemyIntensity;          //  0 - infinity based on number of enemies near player
    int trackingIntensity;    //  Intensity level based on the number of enemy tracking the player
    int healthLost;                //  0 - 100 based on how much health has been lost from 100
    float healthIntensity;      //  1.0 + value that multiplies the intensity based on how low the players health is




    // Use this for initialization
    void Start () {

        if (shouldAIDebug)
        {
            //Debug.Log("Director Alive !");
        }
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

        int playerCount = 0;
        foreach (PlayerStats foundAI in FindObjectsOfType<PlayerStats>())
        {
            players.Add(foundAI.gameObject);
            playerCount++;
            //Debug.Log("Player Found by director !");
        }
   
        if (shouldAIDebug)
        {
            //Debug.Log(aiCount + " AI Unit(s) Found");
            //Debug.Log(playerCount + " Player(s) Found");
            //Debug.Log("Director Init Complete");
        } 

    }
	
	// Update is called once per frame
	void Update () {

        if (isServer)
        {
            if (active)
            {

                //  Update List with new enemy count
                rescanForAI();

                //  Intensity updates   ---------------------------------------------

                //  Run intensity update if the update interval has been passed   
                if (Time.time > (intensityLastRunTime + intensityUpdateInterval))
                {
                    //Debug.Log("Player Intensity Updating");
                    updatePlayerIntensity();
                    intensityLastRunTime = Time.time;
                }


                //  Spawning    ------------------------------------------------------


                //  Run spawning function if cooldown is up
                if (Time.time > (spawnLast + spawnInterval))
                {
                    spawnEnemies();
                    spawnLast = Time.time;
                }


                //  AI Cleanup  ------------------------------------------------------

                if (Time.time > (cleanupLastRunTime + cleanupInterval))
                {
                    cleaupAI();
                    cleanupLastRunTime = Time.time;
                }



            }

            //  Debug to disable and enable director

            if (Input.GetKeyDown("m"))
            {
                if (active)
                {
                    active = false;
                    //if (shouldAIDebug)
                    //{
                    Debug.Log("Director Inactive");
                    //}
                }
                else
                {
                    active = true;
                    //if (shouldAIDebug)
                    //{
                    Debug.Log("Director Active");
                    //}
                }
            }


            //  Debug change from night to day

            if (Input.GetKeyDown("n"))
            {
                if (isDay)
                {
                    //  Is currently day so switch to night
                    isDay = false;
                    //if (shouldAIDebug)
                    //{
                    Debug.Log("Switched To Night Time");
                    //}
                }
                else
                {
                    //  Is currently night so switch to day
                    isDay = true;
                    //if (shouldAIDebug)
                    //{
                    Debug.Log("Switched To Day Time");
                    //}
                }
            }
            if (Input.GetKeyDown(","))
            {
                foreach (GameObject player in players)
                {
                    scanSpawnAreas(player.transform.position, 60, 25, numberOfSpawnLocations);
                }
            }
            if (Input.GetKeyDown("."))
            {
                foreach (DebugCubeScript foundCube in FindObjectsOfType<DebugCubeScript>())
                {
                    Destroy(foundCube.transform.gameObject);
                }
            }
        }

    }

    //  Scan Spawn Areas
    //
    //  This function is used to scan a defined area to generate a list of spawnable locations for the AI units. It does this by first generating a random location
    //  vector within a set area and ignoring the inner part of the area (this stops at from spawning too close to the players). This found area is then checked for
    //  any obsticles (aside from floors), if none area found the area is clear to spawn. If a collider is found then another random location is generated and checked.
    //  This is limited up to a defined amount (maxRunAttemps) to stop areas that cann be spawned in cuasing infite loops

    bool scanSpawnAreas(Vector3 areaCentre, float areaSize, float centerIgnoreSize, int numberOfSpawnLocatoins, int maxRunAttempts = 10)
    {

        int maxRunCounter = 0;
        //  Remove all old spawn locations
        spawnLocations.Clear();
        bool areaFound = false;

        for(int i = 0; i < numberOfSpawnLocatoins; i++)
        {
            areaFound = false;
            //  Only run this loop up to maxRunAttemps, prevents an unspawnable area causing an infinite loop
            maxRunCounter = 0;
            while ((maxRunCounter < maxRunAttempts) && (areaFound == false))
            {               

                //  Generate a random position offset within the input area size
                float xPos = Random.Range(-areaSize, areaSize);
                float zPos = Random.Range(-areaSize, areaSize);

                // Ensure position cannot be inside of ingore radius

                //  Create new spawn location from generated values above
                Vector3 spawnLocation = new Vector3(xPos, 0.0f, zPos);
                //  Create a vector of distance centreIgnoreLine in the direction of the random vector
                Vector3 innerArea = Vector3.Normalize(spawnLocation) * centerIgnoreSize;    
                //  Add the ignore area to the random location to push spawn points away from the character
                spawnLocation = spawnLocation + innerArea;

                bool otherPlayerProximity = false;

                foreach (GameObject player in players)
                {
                    float distance = (spawnLocation - player.transform.position).magnitude; //  Distance from player to spawn location

                    //Debug.Log("Distance From player " + player + " = " + distance);

                    //  Check if spawn location is too close to other players
                    if (distance < playerProximitySize)
                    {
                        //  Spawn location too close set flag to true
                        otherPlayerProximity = true;
                        //Debug.Log("Too Close to player");
                    }
                }

                //  Only run the rest of the check if the location is not within the buffer distance of all other players
                if (otherPlayerProximity != true)
                {

                    //  Find all colliders at the random location
                    Collider[] hitColliders = Physics.OverlapSphere(spawnLocation, spawnBufferSize);


                    //  Run through all found colliders
                    bool areaClear = true;
                    int hits = 0;
                    while (hits < hitColliders.Length)
                    {
                        // if anything other than the floor is found then the area is not suitable
                        if (!hitColliders[hits].CompareTag("floor"))
                        {
                            areaClear = false;
                            //Debug.Log("Object " + i + " - Hit collider = " + hitColliders[hits].name);

                            //  Debug
                            if (shouldAICreateSpawnDebug)
                            {
                                GameObject debugCube = Instantiate(debugSpawnCube, spawnLocation, Quaternion.identity);
                                debugCube.GetComponent<DebugCubeScript>().gizmoColour = Color.red;
                            }
                        }
                        hits++;
                    }

                    if (areaClear == true)
                    {

                        //Debug.Log("Clear spawn area found !");
                        //  Spawn debug cube at spawn location
                        //GameObject debugCube = Instantiate(cube, spawnLocation, Quaternion.identity);
                        //debugCube.GetComponent<SphereCollider>().radius = spawnBufferSize;
                        //Destroy(debugCube, 10);

                        NavMeshHit navMeshHit;


                        //  Scan for a navmesh position at the random location
                        if (NavMesh.SamplePosition(spawnLocation, out navMeshHit, spawnBufferSize, NavMesh.AllAreas))
                        {
                            //Debug.Log("Location with navmesh found !");
                            //NavMesh foundNavMesh = navMeshHit;
                            bool canPathToPlayer = true;
                            //  Can the path reach player 0
                            NavMeshPath pathToPlayer = new NavMeshPath();
                            //NavMesh.CalculatePath(navMeshHit.position, players[0].transform.position, NavMesh.AllAreas, pathToPlayer);

                            foreach (GameObject player in players)
                            {
                                NavMesh.CalculatePath(navMeshHit.position, players[0].transform.position, NavMesh.AllAreas, pathToPlayer);
                                if (pathToPlayer.status != NavMeshPathStatus.PathComplete)
                                {
                                    //  Cant find path to player
                                    canPathToPlayer = false;

                                    //  Debug
                                    if (shouldAICreateSpawnDebug)
                                    {
                                        GameObject debugCube = Instantiate(debugSpawnCube, spawnLocation, Quaternion.identity);
                                        debugCube.GetComponent<DebugCubeScript>().gizmoColour = Color.yellow;
                                    }
                                }

                            }

                            if (canPathToPlayer)
                            {
                                //Debug.Log("Area can navigate to player");
                                //  All checks passed, add found spawn location to spawn list
                                spawnLocations.Add(spawnLocation);
                                // Break out of while loop as spawn location has been found
                                maxRunCounter = maxRunAttempts;
                                areaFound = true;

                                //Debug.Log("Area found for pos : " + i);

                                //  Debug
                                if (shouldAICreateSpawnDebug)
                                {
                                    GameObject debugCube = Instantiate(debugSpawnCube, spawnLocation, Quaternion.identity);
                                    debugCube.GetComponent<DebugCubeScript>().gizmoColour = Color.blue;
                                }

                                //Debug.Log("Area Found");
                                if (shouldAIDebug)
                                {
                                    //Debug.Log("Number of runs to find spawn area = " + maxRunCounter);
                                }
                                //GameObject debugCube = Instantiate(cube, spawnLocation, Quaternion.identity);
                            }

                        }

                    }
                }
                //  Debug
                if (shouldAICreateSpawnDebug)
                {
                    GameObject debugCube = Instantiate(debugSpawnCube, spawnLocation, Quaternion.identity);
                    debugCube.GetComponent<DebugCubeScript>().gizmoColour = Color.red;
                }
                //Debug.Log("Run attempt Complete");
                maxRunCounter++;

            }
            /*
            if (maxRunCounter > maxRunAttempts)
            {
                //  Spawnable area list could not be populated 
                if (shouldAIDebug)
                {
                    Debug.Log("Area could not be found !");
                }
                return false;
            }
            */
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

        //  Ensure spawning stops when AI count is over limit
        if (activeAICount < maxAiCount)
        {
            //  Spawn more AI units near each player
            foreach (GameObject player in players)
            {
                //  Find spawn locations near the player
                scanSpawnAreas(player.transform.position, 60, 25, numberOfSpawnLocations);


                if (isDay)
                {
                    //  Day time spawning -------------------------------------------------------
                    if (player.GetComponent<PlayerStats>().intensity < targetIntensityLevelDay)
                    {

                        int randomLocation = Random.Range(0, spawnLocations.Count - 1);
                        //if (spawnLocations[randomLocation] != null)
                        //{
                            //  Spawn units with no target
                            spawnUnits(aiSpawnGroupSizeDay, spawnLocations[randomLocation], spawnBufferSize);
                        //}

                    }
                }


                else
                {
                    if (player.GetComponent<PlayerStats>().intensity < targetIntensityLevelNight)
                    {
                        //  Night time spawning -----------------------------------------------------

                        for (int i = 0; i < numerOfSpawnGroups; i++)
                        {
                            //Debug.Log("Wave Spawed !");
                            int randomLocation = Random.Range(0, spawnLocations.Count - 1);
                            //if (spawnLocations[randomLocation] != null)
                            //{
                                //  Spawn units targeting the player
                                spawnUnits(aiSpawnGroupSizeNight, spawnLocations[randomLocation], spawnBufferSize, player);
                            //}

                        }

                    }
                }



            }

        }
    }


    //  Spawn a number of AI units around a position and set them to target the provided player
    void spawnUnits(int number, Vector3 position, float spread, GameObject targetPlayer)
    {

        // Spawn the units within random locations near the defined position
        for (int i =0; i < number; i++)
        {
            float xOffset = Random.Range(-spread, spread);   //  Random x offset
            float zOffset = Random.Range(-spread, spread);   //  Random y offset

            Vector3 spawnPosition = new Vector3(position.x + xOffset, position.y + 0.5f, position.z + zOffset);    //  Generate spawn location

            var spawnedEnemy = (GameObject)Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);   //  Create new AI units
            spawnedEnemy.GetComponent<StateController>().moveCommandLocation = targetPlayer.transform.position;
            spawnedEnemy.GetComponent<StateController>().target = targetPlayer;
            NetworkServer.Spawn(spawnedEnemy);  //  Add spawned unit to server list
        }

    }

    //  Spawn a number of AI units around a position with no target
    void spawnUnits(int number, Vector3 position, float spread)
    {

        // Spawn the units within random locations near the defined position
        for (int i = 0; i < number; i++)
        {
            float xOffset = Random.Range(-spread, spread);   //  Random x offset
            float zOffset = Random.Range(-spread, spread);   //  Random y offset

            Vector3 spawnPosition = new Vector3(position.x + xOffset, position.y + 0.5f, position.z + zOffset);    //  Generate spawn location

            var spawnedEnemy = (GameObject)Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);   //  Create new AI units
            NetworkServer.Spawn(spawnedEnemy);  //  Add spawned unit to server list
        }

    }



    //  This calculates and updates the intensity level for all players currently within the game
    void updatePlayerIntensity()
    {
        
        //int foundAI = 0;        //  The number of AI that are nearby the player
        //int trackingAI = 0;     //  The number of AI that are currently targetting the player

        foreach (GameObject player in players)
        {
            int foundAI = 0;        //  The number of AI that are nearby the player
            int trackingAI = 0;     //  The number of AI that are currently targetting the player

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
            //Debug.Log("Found enemy = " + foundAI);

            //  Find all enemies that are targeting the player
            GameObject[] enemyAI = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemyAI.Length; i++)
            {
                if (enemyAI[i].GetComponent<StateController>().target == player)
                {
                    //Debug.Log("Enemy Tracking Player");
                    trackingAI++;
                    //Debug.Log("Number of tracking AI found = " + trackingAI);
                }
            }



            //float intensityLevel = 0.0f;                                    //  Overall intensity level

            //int nearbyEnemyIntensity = (foundAI * intensityPerAI);          //  0 - infinity based on number of enemies near player
            //int trackingIntensity = trackingAI * intensityPerTrackingAI;    //  Intensity level based on the number of enemy tracking the player
            //int healthLost = (100 - statsRef.currentHealth);                //  0 - 100 based on how much health has been lost from 100
            ////int ammoIntensity =                                       //  Ammo intensity not part of current build

            //float healthIntensity = ((float)healthLost / 100) + 1;      //  1.0 + value that multiplies the intensity based on how low the players health is

            float intensityLevel = 0.0f;                                    //  Overall intensity level

            nearbyEnemyIntensity = (foundAI * intensityPerAI);          //  0 - infinity based on number of enemies near player
            trackingIntensity = trackingAI * intensityPerTrackingAI;    //  Intensity level based on the number of enemy tracking the player
            healthLost = (100 - statsRef.currentHealth);                //  0 - 100 based on how much health has been lost from 100
            //int ammoIntensity =                                       //  Ammo intensity not part of current build

            healthIntensity = ((float)healthLost / 100) + 1;      //  1.0 + value that multiplies the intensity based on how low the players health is


            //Debug.Log("Nearby Enemy Intensity =  " + nearbyEnemyIntensity);
            //Debug.Log("Tracking Enemy Intensity =  " + trackingIntensity);
            //Debug.Log("Health Lost Intensity =  " + healthLost);
            //Debug.Log("");

            intensityLevel = (nearbyEnemyIntensity + trackingIntensity) * healthIntensity;    //  Finial intensity level based on above atributes



            statsRef.intensity = intensityLevel;

            // Set final player intensity

            //if (statsRef.intensity < intensityLevel)
            //{
            //    statsRef.intensity = intensityLevel;
            //}
            //else
            //{
            //    statsRef.intensity = statsRef.intensity * 0.9f;
            //}

            /*
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
            */

        }

    }


    //  This will check for any AI units that are too far away from the players and will then remove them
    //  Mostly for daytime use as the players will not be targeted by default during the day, but also
    //  for any AI that have got stuck at a location
    void cleaupAI()
    {

        foreach(GameObject enemy in enemyUnits)
        {
            bool shouldDelete = true;
            
            foreach(GameObject player in players)
            {                
                float distanceToPlayer = (player.transform.position - enemy.transform.position).magnitude;

            
                if(distanceToPlayer < maxDistanceFromPlayers)
                {
                    //  Prevent AI from being deleted if it is still close to any of the players
                    shouldDelete = false;
                }

            }
            //  If the AI is too far away from any of the players destroy it
            if (shouldDelete)
            {
                Destroy(enemy);
            }


        }

        if (shouldAIDebug)
        {
            Debug.Log("AI Cleanup complete !");
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
                playerText += "Overall Intensity = " + string.Format(statsRef.intensity.ToString() + "\n\n");

                playerText += "Nearby Enemy Intensity = " + nearbyEnemyIntensity.ToString() + "\n";
                playerText += "Tracking Enemy Intensity = " + trackingIntensity.ToString() + "\n";
                playerText += "Health Intensity = " + healthIntensity.ToString() + "\n";

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

        if (shouldAIDrawDebug)
        {

            //  Draw detection range for each enemy unit

            foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
            {
                if (foundAI.CompareTag("Enemy"))
                {
                    if (foundAI.GetComponent<StateController>().target != null)
                    {
                        //  Red debug if AI has a target
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        //  White debug if AI does not have a target
                        Gizmos.color = Color.white;
                    }
                    Gizmos.DrawWireSphere(foundAI.transform.position, foundAI.GetComponent<StateController>().detectionRange);
                }
            }

            //  Draw player intensity ranges

            foreach (PlayerStats foundPlayer in FindObjectsOfType<PlayerStats>())
            {                

                if (foundPlayer.CompareTag("NetworkedPlayer"))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(foundPlayer.transform.position, playerProximitySize);
                }


            }


            //  Draw player cleanupradius

            foreach (PlayerStats foundPlayer in FindObjectsOfType<PlayerStats>())
            {

                if (foundPlayer.CompareTag("NetworkedPlayer"))
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(foundPlayer.transform.position, maxDistanceFromPlayers);
                }


            }

            foreach (DebugCubeScript foundCube in FindObjectsOfType<DebugCubeScript>())
            {
                Gizmos.color = foundCube.gizmoColour;
                Gizmos.DrawWireSphere(foundCube.transform.position, spawnBufferSize);
            }

        }

    }

}