using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
	public void LoadScene(int index)
	{
		Scene(index);
	}

	public void LoadSceneAsync(int index)
	{
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