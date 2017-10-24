using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderControl : MonoBehaviour 
{
	public Sprite[] sprites;
	public float shrinkScale;
	public float radius;

	GameObject manager;
	Animator animator;

	int spriteIndex;
	SpriteRenderer spriteRenderer;

	float originalScale;
	float originalRadius;

	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		animator = GetComponent<Animator>();

		spriteIndex = 0;
		spriteRenderer = GetComponent<SpriteRenderer>();

		originalScale = transform.localScale.x;
		originalRadius = 2.0f;
		radius = originalRadius * originalScale;
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

	public void FlipColor()
	{
		spriteIndex = 1 - spriteIndex;
		spriteRenderer.sprite = sprites[spriteIndex];
	}

	public void Shrink()
	{
		transform.localScale *= shrinkScale;
		radius = originalRadius * transform.localScale.x;

		animator.SetFloat("scale", transform.localScale.x);
		animator.SetFloat("direction", 1.0f);
		animator.SetBool("shrink", true);
	}

	public void Expand()
	{
		if (Mathf.Abs(transform.localScale.x - originalScale) > 1e-5)
		{
			transform.localScale /= shrinkScale;
			radius = originalRadius * transform.localScale.x;

			animator.SetFloat("scale", transform.localScale.x);
			animator.SetFloat("direction", -1.0f);
			animator.SetBool("shrink", false);
			//animator.Play("CircleShrinkAnimation", 0, 1.0f);
		}
	}

	public void SetSpeedRate(float speedRate)
	{
		animator.SetFloat("speed", speedRate);
	}
}
