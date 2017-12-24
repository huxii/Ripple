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
    AudioSource changeColorAudio;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		animator = GetComponent<Animator>();
        changeColorAudio = GetComponent<AudioSource>();
        
		spriteIndex = 0;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () 
	{
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
        changeColorAudio.Play();
		//spriteRenderer.sprite = sprites[spriteIndex];
	}

	public void Shrink()
	{
		//animator.SetInteger("hit", 1);
	}

	public void Expand()
	{
		//animator.SetInteger("hit", 2);
	}
}

