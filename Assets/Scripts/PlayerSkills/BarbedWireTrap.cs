using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarbedWireTrap : NetworkBehaviour
{
    //public GameObject CollidedEnemy = null;
    float DamageTimer = 0.0f;
    public int DamageAmount;
    float DestructionTimer = 0.0f;

    /*
    [SyncVar]
    float InitialWalkSpeed;
    [SyncVar]
    float InitialRunSpeed;
    [SyncVar]
    float InitialNavMeshAgentSpeed;
    */

    public float SpeedToSet;
    public bool EnemyColliding = false;
    public int EnemiesInList = 0;

    public List<GameObject> CollidedEnemiesList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //Debug.Log("TrapSpawned");
    }

    // Update is called once per frame
    void Update()
    {
        EnemiesInList = CollidedEnemiesList.Count;
        //Debug.Log("Enemies in list: " + EnemiesInList);
        //if (CollidedEnemy != null)
        if (CollidedEnemiesList.Count >= 1)
        {
            //Debug.Log(DestructionTimer);
            if (DamageTimer >= 1.0f)
            {
                //CollidedEnemy.GetComponent<AIStats>().CmdDamage(DamageAmount);

                for (int i = 0; i < CollidedEnemiesList.Count; i++)
                {
                    if (CollidedEnemiesList[i] != null)
                    {
                        CollidedEnemiesList[i].GetComponent<AIStats>().CmdDamage(DamageAmount);
                    }
                    else
                    {
                        CollidedEnemiesList.RemoveAt(i);
                    }
                }
                DamageTimer = 0.0f;
            }
            else
            {
                DamageTimer = DamageTimer + Time.deltaTime;
            }
        }

        if (DestructionTimer >= 20.0f)
        {
            CmdDestroyTrap();
        }
        else
        {
            DestructionTimer = DestructionTimer + Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider collidedAsset)
    {
        bool AddToList = true;
        if (collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            for (int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                if (collidedAsset.gameObject != CollidedEnemiesList[i])
                {
                    AddToList = true;
                }
                else
                {
                    AddToList = false;
                }
            }

            //if (collidedAsset.gameObject.GetComponent<AIStats>().isTrapped == false)
            if(AddToList == true)
            {
                EnemyColliding = true;
                CollidedEnemiesList.Add(collidedAsset.gameObject);

                //CollidedEnemy = collidedAsset.gameObject;
                collidedAsset.gameObject.GetComponent<AIStats>().CmdSetTrapped(true);

                //InitialWalkSpeed = CollidedEnemiesList[0].gameObject.GetComponent<StateController>().walkSpeed;
                //InitialRunSpeed = CollidedEnemiesList[0].gameObject.GetComponent<StateController>().runSpeed;
                //InitialNavMeshAgentSpeed = CollidedEnemiesList[0].gameObject.GetComponent<StateController>().navMeshAgent.speed;

                collidedAsset.gameObject.GetComponent<StateController>().walkSpeed = SpeedToSet;
                collidedAsset.gameObject.GetComponent<StateController>().runSpeed = SpeedToSet;
                collidedAsset.gameObject.GetComponent<StateController>().navMeshAgent.speed = SpeedToSet;
            }
            else
            {
                Debug.Log("denied motherfucker!");
            }
        }
    }
    private void OnTriggerExit(Collider collidedAsset)
    {
        if (collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            EnemyColliding = false;
            Debug.Log("Enemy No Longer Colliding");
            collidedAsset.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);

            collidedAsset.gameObject.GetComponent<StateController>().walkSpeed = 3.5f;
            collidedAsset.gameObject.GetComponent<StateController>().runSpeed = 5.0f;
            collidedAsset.gameObject.GetComponent<StateController>().navMeshAgent.speed = 3.5f;

            for(int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                if (CollidedEnemiesList[i].gameObject == collidedAsset.gameObject)
                {
                    CollidedEnemiesList.RemoveAt(i);
                }
            }
        }
    }

    [Command]
    void CmdDestroyTrap()
    {
        if (CollidedEnemiesList.Count >= 1)
        {
            //CollidedEnemy.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);

            for (int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().walkSpeed = 3.5f;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().runSpeed = 5.0f;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().navMeshAgent.speed = 3.5f;
                CollidedEnemiesList[i].gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            }

            //CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;

            //CollidedEnemy = null;
        }
        CollidedEnemiesList.Clear();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    void RpcDestroyTrap()
    {
        if (CollidedEnemiesList.Count >= 1)
        {
            //CollidedEnemy.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);

            for (int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().walkSpeed = 3.5f;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().runSpeed = 5.0f;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().navMeshAgent.speed = 3.5f;
                CollidedEnemiesList[i].gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            }

            //CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;

            //CollidedEnemy = null;
        }
        CollidedEnemiesList.Clear();
        Destroy(gameObject);
    }
}