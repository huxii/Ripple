using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingControl : MonoBehaviour
{
    public bool loadWithProgress = false;
	public Slider loadingBar;
	public Text loadingText;
	public SpriteRenderer loadingBlank;

	AsyncOperation ao;

	// Use this for initialization
	void Start()
	{
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
	}

    public void FadeIn(float duration = 1f)
	{
        Color tmpColor = loadingBlank.color;
        tmpColor.a = 1.0f;
        loadingBlank.color = tmpColor;

        loadingBlank.gameObject.SetActive(true);
        loadingBlank.DOFade(0, duration);
	}

    public void FadeOut(float duration = 1f)
	{
        Color tmpColor = loadingBlank.color;
        tmpColor.a = 0.0f;
        loadingBlank.color = tmpColor;

        loadingBlank.gameObject.SetActive(true);
        loadingBlank.DOFade(1f, duration);
	}

    public void FadeText(Text text, string target, float duration = 1f)
    {
        text.DOFade(0, duration/2);
        StartCoroutine(DelayToChangeText(text, target, duration / 2));
    }

    public void Load(int num, float delay = 0.25f, bool fade = true)
    {
        if (fade)
        {
            FadeOut(delay);
        }

        if (loadWithProgress)
        {
            LoadWithProgress(num, delay);
        }
        else
        {
            LoadWithoutProgress(num, delay);         
        }
	}

    public void LoadWithoutProgress(int num, float delay = 1f)
    {
        StartCoroutine(LoadLevelWithDelay(num, delay));
    }

    public void LoadWithProgress(int num, float delay = 1f)
	{
        //loadingBar.gameObject.SetActive(true);

        //loadingText.DOFade(0f, 0f);
        //loadingText.DOFade(1f, 1f);

        StartCoroutine(LoadLevelWithRealProgress(num, delay));
	}

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator DelayToChangeText(Text text, string target, float delay = 1f)
    {
        yield return new WaitForSeconds(delay);

        text.text = target;
        text.DOFade(1f, delay);
    }

    IEnumerator LoadLevelWithDelay(int num, float delay = 1f)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(num);
	}

	IEnumerator LoadLevelWithRealProgress(int num, float delay = 1f)
	{
		yield return new WaitForSeconds(delay);

        loadingText.gameObject.SetActive(true);
        loadingText.text = "Loading...";

        ao = SceneManager.LoadSceneAsync(num);
		ao.allowSceneActivation = false;

		while (!ao.isDone)
		{
			loadingBar.value = ao.progress;

			if (ao.progress == 0.9f)
			{
				loadingBar.value = 1.0f;
                //FadeText(loadingText, "Press 'SPACE' to continue", 0.25f);
				//if (Input.GetKeyDown("space"))
				//{
					ao.allowSceneActivation = true;
				//}
			}

			//Debug.Log(ao.progress);
			yield return null;
		}
	}
}
