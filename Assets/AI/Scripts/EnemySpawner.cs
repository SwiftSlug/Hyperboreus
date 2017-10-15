using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemytoSpawn;
    public int numberToSpawn;

    [SyncVar]
    Vector3 randomPosition;

	// Use this for initialization
	void Start () {

        if (isServer)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                var spawnedEnemy = (GameObject)Instantiate(enemytoSpawn, randomPosition, Quaternion.identity);
                NetworkServer.Spawn(spawnedEnemy);
                
            }
        }
        
    }


	
}
