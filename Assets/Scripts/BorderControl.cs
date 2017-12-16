using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BorderControl : MonoBehaviour 
{
	public float shrinkScale;

	GameObject manager;
	Animator animator;

	float originalScale;

	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		animator = GetComponent<Animator>();

		originalScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	/*
	void OnCollisionEnter2D(Collision2D thisCollision)
	{
		if (thisCollision.collider.tag == "Ball") 
		{
			manager.SendMessage ("BorderHit");
		}
	}
	*/

	public void Shrink()
	{
        float scale = transform.localScale.x * shrinkScale;
        transform.DOScale(new Vector3(scale, scale, scale), 0.5f).SetEase(Ease.InOutCubic);
	}

	public void Expand()
	{
		if (Mathf.Abs(transform.localScale.x - originalScale) > 1e-5)
		{
            float scale = transform.localScale.x / shrinkScale;
            transform.DOScale(new Vector3(scale, scale, scale), 0.5f).SetEase(Ease.InOutCubic);
        }
	}

	public void SetSpeedRate(float speedRate)
	{
		animator.SetFloat("speed", speedRate);
	}
}
