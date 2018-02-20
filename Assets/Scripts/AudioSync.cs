using UnityEngine;
using UnityEngine.Networking;

public class AudioSync : NetworkBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clipArray;
    
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
        audioSource.PlayOneShot(clipArray[audioID], 1.0f);
    }
}