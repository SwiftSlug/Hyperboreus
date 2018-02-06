using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AIDirector : NetworkBehaviour
{
    //public bool blep;

    public bool isDay = true;
    //GameObject[] EnemyUnits;
    List<GameObject> enemyUnits;
    List<GameObject> players;

    GameObject[] Players;
    int targetIntensityLevelDay;
    int targetIntensityLevelNight;
    float waveCooldown;


    // Use this for initialization
    void Start () {

        Debug.Log("Director Alive !");

        enemyUnits = new List<GameObject>();    // Init AI list
        players = new List<GameObject>();       // Init player list

        //  Search for AI units and store their gameobjects in the list
        foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
        {
            enemyUnits.Add(foundAI.gameObject);
            Debug.Log("Enemy Unit Found");
        }

        foreach (PlayerStats foundAI in FindObjectsOfType<PlayerStats>())
        {
            players.Add(foundAI.gameObject);
            Debug.Log("Player Found !");
        }





    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
