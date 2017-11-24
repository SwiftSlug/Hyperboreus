using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour
{
    public int playersDown = 0;

    private NetworkLobbyManager lobbyManager;

    GameObject[] players;

    void Start()
    {
        lobbyManager = FindObjectOfType<NetworkLobbyManager>();

        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerStats>().manager = gameObject;
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
        return playersDown;
    }

    public int GetPlayerAmount()
    {
        return players.Length - 1;
    }

    public void FinishGame()
    {
        if (lobbyManager != null)
        {
            Network.Disconnect(200);
            Destroy(lobbyManager.gameObject);
            NetworkLobbyManager.Shutdown();
        }
    }
}