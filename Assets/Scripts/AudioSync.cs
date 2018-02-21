using UnityEngine;
using UnityEngine.Networking;

public class AudioSync : NetworkBehaviour
{
    public GameObject[] players;
    public AudioSource[] audioSources;
    public AudioClip[] clipArray;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("NetworkedPlayer");

        audioSources = new AudioSource[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            audioSources[i] = players[i].GetComponent<AudioSource>();
        }
        
    }
    
    public void PlaySound(int audioID)
    {
        if (audioID >= 0 || audioID < clipArray.Length)
        {
            CmdSyncAudioServer(audioID);
        }
    }
	
    [Command]
    public void CmdSyncAudioServer(int audioID)
    {
        RpcSyncAudioClient(audioID);
    }

    [ClientRpc]
    public void RpcSyncAudioClient(int audioID)
    {
        for (int i = 0; i < players.Length; i++)
        {
            audioSources[i].PlayOneShot(clipArray[audioID], 1.0f);
        }
    }
}