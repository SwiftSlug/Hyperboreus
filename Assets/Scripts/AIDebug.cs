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
            

            GameObject[] ai = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject foundAI in ai)
            {
                foundAI.GetComponent<StateController>().moveCommandLocation = new Vector3(20, 0, 0);
                Debug.Log("Move Command Sent");
            }

        }

        if (Input.GetButtonDown("DebugAIDamage"))
        {
            

            GameObject[] ai = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject foundAI in ai)
            {
                foundAI.GetComponent<AIStats>().CmdDamage(10);
                Debug.Log("Enemy Damaged");
            }

        }

        //GameObject[] aibelp = GameObject.FindGameObjectsWithTag("Enemy");

        /*
        if (false)
        {
            foreach (GameObject foundAI in aibelp)
            {
                Debug.Log("Enemy health = :" + foundAI.GetComponent<AIStats>().enemyHealth);
            }
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.))
        {
            Debug.Log("Blep");
        }
        */
    }
}
