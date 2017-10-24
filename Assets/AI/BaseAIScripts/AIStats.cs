﻿using UnityEngine;
using UnityEngine.Networking;

public class AIStats : NetworkBehaviour
{
    [SyncVar]
    public int enemyHealth = 100;

    [SyncVar]
    public bool isDead = false;
    //[SyncVar]
    //public int enemyAttackDamage = 100;

    [Command]
    //[ClientRpc]
    public void CmdDamage(int damageAmount)
    {
        if (isServer)
        {
            enemyHealth -= damageAmount;
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
            Destroy(this.transform.gameObject);
        }
    }


}