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

    public void IncreaseDowned()
    {
        if (playersDown < players.Length)
        {
            playersDown++;
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
        Debug.Log(playersDown);

        return playersDown;
    }

    public int GetPlayerAmount()
    {
        return players.Length - 1;
    }   

    public void FinishGame()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }
}