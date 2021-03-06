﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ButtonControl : MonoBehaviour
{
	public GameObject logo;
    public GameObject[] ripplePrefabs;

    MenuControl menuManager;
    bool interactable;
    AudioSource clickSound;

	// Use this for initialization
	void Start()
	{
        menuManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MenuControl>();
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0f);
        logo.transform.DOScale(new Vector3(0f, 0f, 0f), 0f);
        interactable = true;
        clickSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update()
	{
	}

	void OnMouseEnter()
	{
        if (interactable)
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 1f).SetEase(Ease.OutBack);
            logo.transform.DOScale(new Vector3(1f, 1f, 1f), 0.7f).SetEase(Ease.OutBack);

            int idx = Random.Range(0, 2);
            GameObject ripple = Instantiate(ripplePrefabs[idx], transform);
            ripple.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
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
            clickSound.Play();
        }
	}

    IEnumerator DelayToLoad(int num, float delay)
    {
        yield return new WaitForSeconds(delay);
        menuManager.Load(1);
    }
}
