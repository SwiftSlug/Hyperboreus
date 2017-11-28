using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour
{
    public int playersDown = 0;

    public GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerStats>().manager = gameObject;
            Debug.Log("Called");
        }
    }

    [Command]
    public void CmdIncreaseDowned()
    {
        if (!isServer)
        {
            return;
        }
        if (playersDown < players.Length)
        {
            playersDown++;
        }
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

    public int GetDowned()
    {
        return playersDown;
    }

    [ClientRpc]
    public void RpcGetDowned()
    {
        if (!isClient)
        {
            return;
        }

        Debug.Log(playersDown);

        GetDowned();

        return;
    }

    public int GetPlayerAmount()
    {
        return players.Length - 1;
    }

    [ClientRpc]
    public void RpcGetPlayerAmount()
    {
        Debug.Log(players.Length - 1);

        GetPlayerAmount();

        return;
    }  

    [ClientRpc]
    public void RpcFinishGame()
    {
        if (!isClient)
        {
            return;
        }

        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

    [ClientRpc]
    public void RpcGameOverlay()
    {
        if (!isClient)
        {
            return;
        }

        players[1].GetComponent<PlayerStats>().gameOverOverlay.gameObject.SetActive(true);
    }
}