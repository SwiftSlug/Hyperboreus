using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarbedWireTrap : NetworkBehaviour
{

    public GameObject CollidedEnemy = null;
    float DamageTimer = 0.0f;
    float DestructionTimer = 0.0f;
    float InitialRunSpeed;
    public bool EnemyColliding = false;
    public GameObject CollidedEnemyTest = null;

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
            CollidedEnemy = collidedAsset.gameObject;
            collidedAsset.gameObject.GetComponent<AIStats>().CmdSetTrapped(true);
            InitialRunSpeed = CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed;
            collidedAsset.gameObject.GetComponent<StateController>().runSpeed = 0.5f;
        }
    }
    private void OnTriggerExit(Collider collidedAsset)
    {
        if (collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            EnemyColliding = false;
            Debug.Log("Enemy No Longer Colliding");
            collidedAsset.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            collidedAsset.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            CollidedEnemy = null;
        }
    }

    [Command]
    void CmdDestroyTrap()
    {
        if (CollidedEnemy != null)
        {
            CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            CollidedEnemy.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            CollidedEnemy = null;
        }
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    void RpcDestroyTrap()
    {
        if (CollidedEnemy != null)
        {
            CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = InitialRunSpeed;
            CollidedEnemy.gameObject.GetComponent<AIStats>().CmdSetTrapped(false);
            CollidedEnemy = null;
        }
        Destroy(gameObject);
    }
}