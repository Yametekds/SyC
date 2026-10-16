using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	public float waitSeconds = 0;
	public string sceneName;
	public UnityEvent<float, float> OnSceneLoadUpdate;

	private float progress = 0;
	private bool running = false;
	public void ChangeSceneMethod()
	{
		StartCoroutine(ChangeSceneCoroutine());
	}

    private void Update()
    {
		if(!running)
        {
			return;
        }
		OnSceneLoadUpdate?.Invoke(progress, 1.0f);
	}

    private IEnumerator ChangeSceneCoroutine() 
	{
		yield return new WaitForSeconds(waitSeconds);
		AsyncOperation scene = null;
		yield return scene = SceneManager.LoadSceneAsync(sceneName);
		if(scene != null) 
		{
			this.running = true;
			while (scene.isDone)
			{
				this.progress = scene.progress;
				yield return progress;
			}
			this.running = false;
		}
	}
}
