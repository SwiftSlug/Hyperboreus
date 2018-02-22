using UnityEngine;
using UnityEngine.Networking;

public class AIStats : NetworkBehaviour
{
    [SyncVar]
    public int enemyHealth = 100;

    [SyncVar]
    public bool isDead = false;

    public AudioSync audioSync;

    void Start()
    {
        audioSync = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioSync>();
    }

    //[ClientRpc]
    [Command]
    public void CmdDamage(int damageAmount)
    {
        if (isServer)
        {
            enemyHealth -= damageAmount;
            if(enemyHealth <= 0)
            {
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
        if(!isServer)
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
        audioSync.PlaySound(GetComponent<AudioSource>(), 2);

        if (isServer)
        {
            GetComponent<StateController>().aiActive = false;
            enemyHealth = 0;
            isDead = true;
            Destroy(this.transform.gameObject);
        }
    }


}