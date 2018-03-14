using UnityEngine;
using UnityEngine.Networking;

public class AIStats : NetworkBehaviour
{
    [SyncVar]
    public int enemyHealth = 100;

    [SyncVar]
    public bool isDead = false;

    [SyncVar]
    public bool isTrapped = false;

    StateController controller;

    //[SyncVar]
    //public int enemyAttackDamage = 100;

    public AudioSync audioSync;

    void Start()
    {
        audioSync = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioSync>();
        controller = GetComponent<StateController>();
    }

    private void Update()
    {
        if (isServer)
        {
            if (enemyHealth <= 0)
            {
                CmdDie();
            }
        }
    }

    [Command]
    public void CmdDamage(int damageAmount)
    {
        if (isServer)
        {
            enemyHealth -= damageAmount;
            //  Set under attack flag to true for state switch
            controller.underAttack = true;
            //  If AI has no target set nearest player as target
            if (controller.target == null)
            {
                CmdSetTargetNearestPlayer();
            }

            if (enemyHealth <= 0)
            {
                audioSync.PlaySound(this.gameObject, 2, false);

                isDead = true;
            }
            if (!isDead)
            {
                RpcSyncHealth(enemyHealth);
            }
        }
    }

    [ClientRpc]
    public void RpcSyncHealth(int newHealth)
    {
        if (!isServer)
        {
            enemyHealth = newHealth;
        }
    }

    [Command]
    public void CmdDie()
    {
        if (isServer)
        {
            GetComponent<StateController>().aiActive = false;
            enemyHealth = 0;
            isDead = true;

            Destroy(this.transform.gameObject, audioSync.clipArray[2].length);

            audioSync.ResetSound();
        }
    }

    [Command]
    public void CmdSetTrapped(bool trappedValue)
    {
        if (isServer)
        {
            isTrapped = trappedValue;

            if (!isDead)
            {
                RpcSetTrapped(trappedValue);
            }
        }
    }

    [ClientRpc]
    public void RpcSetTrapped(bool trappedValue)
    {
        if (!isServer)
        {
            isTrapped = trappedValue;
        }
    }

    [Command]
    public void CmdSetTargetNearestPlayer()
    {
        
        float minDistance = 0.0f;
        GameObject closestPlayer = null;

        GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");
        foreach (GameObject player in players)
        {
            //  Run through each player and find the clostest to the AI controller
            float distance = (transform.position - player.transform.position).magnitude;

            if ( distance < minDistance || minDistance == 0.0f)
            {
                minDistance = distance;
                closestPlayer = player;
            }
        }

        //  Set the AI to target the closest player
        controller.target = closestPlayer;

    }

}