using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignControl : MonoBehaviour 
{
	public Sprite[] sprites;
	public int spriteIndex;

	GameObject manager;
	Animator animator;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		animator = GetComponent<Animator>();

		spriteIndex = 0;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () 
	{
		animator.SetInteger("hit", 0);
	}

	/*
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Ball") 
		{
			manager.SendMessage ("BouncerHit", this.gameObject);
		}
	}
	*/

	public void FlipColor()
	{
		spriteIndex = 1 - spriteIndex;
		animator.SetInteger("color", spriteIndex);
		//spriteRenderer.sprite = sprites[spriteIndex];
	}

	public void Shrink()
	{
		animator.SetInteger("hit", 1);
	}

	public void Expand()
	{
		animator.SetInteger("hit", 2);
	}
}

