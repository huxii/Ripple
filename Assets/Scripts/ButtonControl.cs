using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ButtonControl : MonoBehaviour
{
	public GameObject logo;

    MenuControl menuManager;
    bool interactable;

	// Use this for initialization
	void Start()
	{
        menuManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MenuControl>();
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0f);
        logo.transform.DOScale(new Vector3(0f, 0f, 0f), 0f);
        interactable = true;
    }
	
	// Update is called once per frame
	void Update()
	{
	}

	void OnMouseEnter()
	{
        if (interactable)
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 1f).SetEase(Ease.InOutCubic);
            logo.transform.DOScale(new Vector3(1f, 1f, 1f), 0.7f).SetEase(Ease.InOutCubic);
        }
    }

	void OnMouseExit()
	{
        if (interactable)
        {
            transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f).SetEase(Ease.InOutCubic);
            logo.transform.DOScale(new Vector3(0f, 0f, 0f), 0.7f).SetEase(Ease.InOutCubic);
        }
    }

	void OnMouseDown()
	{
        if (interactable)
        {
            interactable = false;
            logo.GetComponent<Animator>().SetTrigger("Clicked");
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(DelayToLoad(1, 1.1f));
        }
	}

    IEnumerator DelayToLoad(int num, float delay)
    {
        yield return new WaitForSeconds(delay);
        menuManager.Load(1);
    }
}
