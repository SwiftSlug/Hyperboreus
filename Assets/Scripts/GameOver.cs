using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour
{
    [SyncVar]
    public int playersDown = 0;

    public GameObject[] players;

    [SyncVar]
    public int playerTotal = 0;

    void Start()
    {
        Debug.Log("Start Called");

        Invoke("CmdPlayerJoin", 2);
    }

    [Command]
    public void CmdPlayerJoin()
    {
        if (!isServer)
        {
            return;
        }

        Debug.Log("Server Called Player Join");

        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        int tmp = 0;

        foreach (GameObject x in players)
        {
            tmp++;
        }

        playerTotal = tmp;

        CmdSyncTotal(playerTotal);

        RpcSyncPlayerTotal(playerTotal);

        Debug.Log("Player Total: " + playerTotal);
    }

    [Command]
    public void CmdSyncTotal(int total)
    {
        if (!isServer)
        {
            return;
        }

        Debug.Log("Server Sync Total");

        Debug.Log("Total: " + total);

        playerTotal = total;

        Debug.Log("Server's Player Total: " + playerTotal);
    }

    [ClientRpc]
    public void RpcSyncPlayerTotal(int total)
    {
        Debug.Log("Client Sync Total");

        playerTotal = total;
    }

    [Command]
    public void CmdIncreaseDowned()
    {
        if (!isServer)
        {
            return;
        }

        CmdSyncPlayersDown(playersDown);

        if (playersDown < playerTotal)
        {
            Debug.Log("Player: " + playerTotal);
            Debug.Log("Players Down: " + playersDown);

            playersDown++;

            Debug.Log("Player Down Increase");

            Debug.Log("Players Down: " + playersDown);

            CmdSyncPlayersDown(playersDown);
        }

        if (playersDown == playerTotal)
        {
            CmdSyncPlayersDown(playersDown);

            RpcGameOverlay();
        }
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

        if(downTotal == playerTotal - 1)
        {
            RpcGameOverlay();
        }
    }

    [Command]
    public void CmdSyncPlayersDown(int downed)
    {
        if (!isServer)
        {
            return;
        }

        Debug.Log("Server Sync Downed");

        Debug.Log("Downed: " + downed);

        //playersDown++;

        playersDown = downed;

        Debug.Log("Server Downed Count: " + playersDown);
    }

    [ClientRpc]
    public void RpcSyncPlayersDown(int downed)
    {
        Debug.Log("Client Sync Downed");

        Debug.Log("Downed: " + downed);

        //playersDown++;

        playersDown = downed;

        Debug.Log("Synced Downed: " + playersDown);
    }

    [ClientRpc]
    public void RpcSyncPlayers(int downed)
    {
        Debug.Log("Client Sync Downed");

        Debug.Log("Downed: " + downed);

        playersDown = downed;

        Debug.Log("Synced Downed: " + playersDown);
    }

    [Command]
    public void CmdDecreaseDowned()
    {
        if (!isServer)
        {
            return;
        }

        if (playersDown > 0)
        {
            playersDown--;
        }
    }

    [ClientRpc]
    public void RpcGameOverlay()
    {
        Debug.Log("RPC Called");

        GetComponent<PlayerStats>().gameOverOverlay.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcFinishGame()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }
}