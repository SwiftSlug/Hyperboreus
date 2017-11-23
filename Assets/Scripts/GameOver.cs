using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour
{
    private int playersDown = 0;

    GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("Settings Manager");
            players[i].GetComponent<PlayerStats>().manager = gameObject;
        }
    }

    public void IncreaseDowned()
    {
        playersDown++;
    }

    public void DecreaseDowned()
    {
        playersDown--;
    }

    public int GetDowned()
    {
        return playersDown;
    }

    public int GetPlayerAmount()
    {
        return players.Length - 1;
    }
}
