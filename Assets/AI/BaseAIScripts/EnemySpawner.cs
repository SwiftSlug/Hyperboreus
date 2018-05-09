using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemytoSpawn;
    public int numberToSpawn;
    public float spawnArea;

    [SyncVar]
    Vector3 randomPosition;

	// Use this for initialization
	void Start () {

        if (isServer)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                randomPosition = new Vector3(Random.Range(-spawnArea, spawnArea), 0, Random.Range(-spawnArea, spawnArea));
                Vector3 currentPosition = transform.position;

                Vector3 spawnPos = randomPosition + currentPosition;

                var spawnedEnemy = (GameObject)Instantiate(enemytoSpawn, spawnPos, Quaternion.identity);
                NetworkServer.Spawn(spawnedEnemy);
                //NetworkServer.SpawnWithClientAuthority(spawnedEnemy, connectionToServer);
            }
        }
        
    }	

}