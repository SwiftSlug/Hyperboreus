using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarbedWireTrap : NetworkBehaviour
{
    public GameObject CollidedEnemy = null;
    float DamageTimer = 0.0f;
    float DestructionTimer = 0.0f;
    float InitialWalkSpeed;
    float InitialRunSpeed;
    float InitialNavMeshAgentSpeed;
    public float SpeedToSet;
    public bool EnemyColliding = false;

    public List<GameObject> CollidedEnemiesList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //Debug.Log("TrapSpawned");
    }

    // Update is called once per frame
    void Update()
    {
        if (CollidedEnemy != null)
        {
            //Debug.Log(DestructionTimer);
            if (DamageTimer >= 1.0f)
            {
                CollidedEnemy.GetComponent<AIStats>().CmdDamage(5);

                for (int i = 0; i < CollidedEnemiesList.Count; i++)
                {
                    if (CollidedEnemiesList[i] != null)
                    {
                        CollidedEnemiesList[i].GetComponent<AIStats>().CmdDamage(5);
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
        if (collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            EnemyColliding = true;
            CollidedEnemiesList.Add(collidedAsset.gameObject);

            CollidedEnemy = collidedAsset.gameObject;
            collidedAsset.gameObject.GetComponent<AIStats>().CmdSetTrapped(true);

            InitialWalkSpeed = CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed;
            InitialRunSpeed = CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed;
            InitialNavMeshAgentSpeed = CollidedEnemy.gameObject.GetComponent<StateController>().navMeshAgent.speed;


            collidedAsset.gameObject.GetComponent<StateController>().walkSpeed = SpeedToSet;
            collidedAsset.gameObject.GetComponent<StateController>().runSpeed = SpeedToSet;
            collidedAsset.gameObject.GetComponent<StateController>().navMeshAgent.speed = SpeedToSet;
        }
    }
    private void OnTriggerExit(Collider collidedAsset)
    {
        if (collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            EnemyColliding = false;
            Debug.Log("Enemy No Longer Colliding");
            collidedAsset.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);

            collidedAsset.gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
            collidedAsset.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            collidedAsset.gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;

            for(int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                if (CollidedEnemiesList[i] == collidedAsset)
                {
                    CollidedEnemiesList.RemoveAt(i);
                }
            }


            CollidedEnemy = null;
        }
    }

    [Command]
    void CmdDestroyTrap()
    {
        if (CollidedEnemy != null)
        {
            //CollidedEnemy.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);

            for (int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;
                CollidedEnemiesList[i].gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            }
            CollidedEnemiesList.Clear();

            //CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;

            //CollidedEnemy = null;
        }
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    void RpcDestroyTrap()
    {
        if (CollidedEnemy != null)
        {
            //CollidedEnemy.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);

            for (int i = 0; i < CollidedEnemiesList.Count; i++)
            {
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
                CollidedEnemiesList[i].gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;
                CollidedEnemiesList[i].gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            }
            CollidedEnemiesList.Clear();

            //CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = InitialWalkSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            //CollidedEnemy.gameObject.GetComponent<StateController>().navMeshAgent.speed = InitialNavMeshAgentSpeed;

            //CollidedEnemy = null;
        }
        Destroy(gameObject);
    }
}