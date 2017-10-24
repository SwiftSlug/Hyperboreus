using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ButtonLoadScene : MonoBehaviour
{
    public NetworkLobbyManager lobbyManager;

    void Start()
    {
        lobbyManager = FindObjectOfType<NetworkLobbyManager>();
    }

	public void LoadScene(int index)
	{
        if (lobbyManager != null)
        {
            Destroy(lobbyManager.gameObject);
            NetworkLobbyManager.Shutdown();
        }

		Scene(index);
	}

	public void LoadSceneAsync(int index)
	{
        if (lobbyManager != null)
        {
            Destroy(lobbyManager.gameObject);
            NetworkLobbyManager.Shutdown();
        }

        StartCoroutine(SceneAsync(index));
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	private void Scene(int index)
	{
		SceneManager.LoadScene(index);
	}

	IEnumerator SceneAsync(int sceneIndex)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}