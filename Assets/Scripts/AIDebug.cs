using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Blep");

            GameObject[] ai = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject foundAI in ai)
            {
                foundAI.GetComponent<StateController>().moveCommandLocation = new Vector3(20, 0, 0);
            }

        }

        /*
        if (Input.GetKeyDown(KeyCode.))
        {
            Debug.Log("Blep");
        }
        */
    }
}
