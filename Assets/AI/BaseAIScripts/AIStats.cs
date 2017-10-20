using UnityEngine;
using UnityEngine.Networking;

public class AIStats : NetworkBehaviour
{
    [SyncVar]
    public int enemyHealth = 100;

    [SyncVar]
    public int enemyAttackDamage = 100;

    [Command]
    public void CmdDamage(int damageAmount)
    {
        if (isServer)
        {
            enemyHealth -= damageAmount;
        }
    }



}