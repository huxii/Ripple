using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingControl : MonoBehaviour
{
	public Slider loadingBar;
	public Text loadingText;
	public GameObject loadingBgPrefab;
	public Canvas title;

	AsyncOperation ao;

	// Use this for initialization
	void Start()
	{
		FadeIn();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FadeIn()
	{
		GameObject bg = Instantiate(loadingBgPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
		bg.GetComponent<FadingControl>().fadingDir = -1;		
	}

	void FadeOut()
	{
		GameObject bg = Instantiate(loadingBgPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
		bg.GetComponent<FadingControl>().fadingDir = 1;		
	}

	public void Loading()
	{
		FadeIn();

		//loadingBar.gameObject.SetActive(true);
		loadingText.gameObject.SetActive(true);
		loadingText.text = "Loading...";
		title.gameObject.SetActive(false);

		StartCoroutine(LoadLevelWithRealProgress());
	}

	IEnumerator LoadLevelWithRealProgress()
	{
		yield return new WaitForSeconds(1);

		ao = SceneManager.LoadSceneAsync(1);
		ao.allowSceneActivation = false;

		while (!ao.isDone)
		{
			loadingBar.value = ao.progress;

			if (ao.progress == 0.9f)
			{
				loadingBar.value = 1.0f;
				loadingText.text = "Press 'SPACE' to continue";
				if (Input.GetKeyDown("space"))
				{
					//print(",,,,,,,,,,");
					FadeOut();
					ao.allowSceneActivation = true;
				}
			}

			//Debug.Log(ao.progress);
			yield return null;
		}
	}
}
