using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent nav;
    public GameObject moveLocationObject;
    Vector3 moveLocation;

    // Use this for initialization
    void Start () {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //moveLocation = new Vector3(Random.Range(-100.0f, 100.0f), 0, Random.Range(-100.0f, 100.0f));    // Sets random move location

        moveLocation = moveLocationObject.transform.position;
        nav.SetDestination(moveLocation);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
