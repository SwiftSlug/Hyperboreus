using UnityEngine;
using UnityEngine.Networking;

public class AIStats : NetworkBehaviour
{
    [SyncVar]
    public int enemyHealth = 100;

    [SyncVar]
    public bool isDead = false;
<<<<<<< HEAD

    [SyncVar]
    public bool isTrapped = false;
    //[SyncVar]
    //public int enemyAttackDamage = 100;
=======
>>>>>>> Audio

    public AudioSync audioSync;

    void Start()
    {
        audioSync = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioSync>();
    }

    [Command]
    public void CmdDamage(int damageAmount)
    {
        if (isServer)
        {
            enemyHealth -= damageAmount;
<<<<<<< HEAD
            if (enemyHealth <= 0)
=======

            if(enemyHealth <= 0)
>>>>>>> Audio
            {
                audioSync.PlaySound(this.gameObject, 2);

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
    public void CmdDie()
    {
        if (isServer)
        {
            GetComponent<StateController>().aiActive = false;
            enemyHealth = 0;
            isDead = true;

<<<<<<< HEAD
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
}
=======
            Destroy(this.transform.gameObject, audioSync.clipArray[2].length);

            audioSync.ResetSound();
        }
    }
}
>>>>>>> Audio
