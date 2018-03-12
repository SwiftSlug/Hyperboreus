using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour
{
    public GameObject[] players;

    [SyncVar]
    public int playersDown = 0;

    [SyncVar]
    public int playerTotal = 0;

    void Start()
    {
        Invoke("CmdPlayerJoin", 2);
    }

    [Command]
    public void CmdPlayerJoin()
    {
        if (!isServer)
        {
            return;
        }

        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        int tmp = 0;

        foreach (GameObject x in players)
        {
            tmp++;
        }

        playerTotal = tmp;

        CmdSyncTotal(playerTotal);

        RpcSyncPlayerTotal(playerTotal);

    }

    [Command]
    public void CmdSyncTotal(int total)
    {
        if (!isServer)
        {
            return;
        }

        playerTotal = total;
    }

    [ClientRpc]
    public void RpcSyncPlayerTotal(int total)
    {
        playerTotal = total;
    }

    [Command]
    public void CmdCheckDead()
    {
        if(!isServer)
        {
            return;
        }

        int downTotal = 0;

        foreach (GameObject x in players)
        {
            if (x.GetComponent<PlayerStats>().isDead)
            {
                downTotal++;
            }
        }

        if(downTotal == playerTotal)
        {
            RpcGameOverlay();
        }
    }

    [ClientRpc]
    public void RpcGameOverlay()
    {
        GetComponent<PlayerStats>().gameOverOverlay.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcFinishGame()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }
}