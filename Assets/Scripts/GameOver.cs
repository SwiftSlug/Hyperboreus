using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour
{
    [SyncVar]
    public int playersDown = 0;

    public GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerStats>().manager = gameObject;
            Debug.Log("Assigned GameObject " + i);
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

    public void IncreaseDowned()
    {
        if (playersDown < players.Length)
        {
            playersDown++;
        }

        if (playersDown == players.Length)
        {
            GameOverlay();
        }
    }

    public void DecreaseDowned()
    {
        if (playersDown > 0)
        {
            playersDown--;
        }
    }

    public int GetDowned()
    {
        return playersDown;
    }

    public int GetPlayerAmount()
    {
        return players.Length;
    }


    [ClientRpc]
    public void RpcFinishGame()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

    [ClientRpc]
    public void RpcGameOverlay()
    {
        Debug.Log("RPC Called");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerStats>().gameOverOverlay.gameObject.SetActive(true);
        }
    }

    public void GameOverlay()
    {
        Debug.Log("RPC Called");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerStats>().gameOverOverlay.gameObject.SetActive(true);
        }
    }
}