using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemytoSpawn;
    public int numberToSpawn;

	// Use this for initialization
	void Start () {


        for(int i = 0; i < numberToSpawn; i++)
        {
            var randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(enemytoSpawn, randomPosition, Quaternion.identity);
        }
        


    }
	
}
