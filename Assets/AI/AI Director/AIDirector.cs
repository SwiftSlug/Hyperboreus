using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AIDirector : NetworkBehaviour
{
    public bool blep;

    public bool isDay = true;
    //GameObject[] EnemyUnits;
    public GameObject enemyToSpawn;

    List<GameObject> enemyUnits;
    List<GameObject> players;

    GameObject[] Players;
    int targetIntensityLevelDay;
    int targetIntensityLevelNight;
    float waveCooldown;

    public float playerProximitySize = 200.0f;
    public int intensityPerAI = 10;


    // Use this for initialization
    void Start () {

        Debug.Log("Director Alive !");

        enemyUnits = new List<GameObject>();    // Init AI list
        players = new List<GameObject>();       // Init player list

        //  Search for AI units and store their gameobjects in the list
        foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
        {
            enemyUnits.Add(foundAI.gameObject);
            Debug.Log("Enemy Unit Found by Director !");
        }


        foreach (PlayerStats foundAI in FindObjectsOfType<PlayerStats>())
        {
            players.Add(foundAI.gameObject);
            Debug.Log("Player Found by director !");
        }

        //Invoke("updatePlayerIntensity", 1);
        //Invoke("updatePlayerIntensity", 5);
        //Invoke("updatePlayerIntensity", 15);
        //Invoke("updatePlayerIntensity", 20);
        //Invoke("updatePlayerIntensity", 25);



    }
	
	// Update is called once per frame
	void Update () {

        //updatePlayerIntensity();
        updatePlayerIntensity();

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


    void updatePlayerIntensity()
    {

        Debug.Log("Player Intensity Update -------------");

        int foundAI = 0;
        int trackingAI = 0;

        foreach (GameObject player in players)
        {
            PlayerStats statsRef = player.GetComponent<PlayerStats>();

            Collider[] hitCollider = Physics.OverlapSphere(player.transform.position, playerProximitySize);
            for (int i = 0; i < hitCollider.Length; i++)
            {
                if (hitCollider[i].CompareTag("Enemy"))
                {
                    Debug.Log("Enemy Found Near Player");
                    foundAI++;                    
                }
            }

            GameObject[] enemyAI = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemyAI.Length; i++)
            {
                if (enemyAI[i].GetComponent<StateController>().target == player)
                {
                    Debug.Log("Enemy Tracking Player");
                }
            }
        
            

            //hitCollider[i].GetComponent<StateController>().target ==


            int intensityLevel = 0; //  Overall intensity level

            int nearbyEnemyIntensity = (foundAI * intensityPerAI);        //  0 - infinity based on number of enemies near player
            int healthIntensity = (100 - statsRef.currentHealth);   //  0 - 100 based on how much health has been lost from 100
            //int ammoIntensity = 

            intensityLevel = nearbyEnemyIntensity * (1 + (healthIntensity / 100));    //  Finial intensity level based on above atributes7

            Debug.Log(intensityLevel);
        }

    }

}