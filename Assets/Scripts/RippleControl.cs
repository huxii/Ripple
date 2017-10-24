using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleControl : MonoBehaviour
{
	public string animationName;
	public float startTime;

	Animator animator;
	AudioSource audio;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		animator.Play(animationName, 0, startTime);

		audio = GetComponent<AudioSource>();
		audio.Play();
	}
	
	// Update is called once per frame
	void Update()
	{
		float curTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		if (curTime >= 0.95f)
		{
			DestroyObject(gameObject);
		}
	}

	public void Init(int type)
	{
		float scale = 1.0f;
		if (type == 0)
		{
			scale = 1.4f;
		}
		else
		if (type == 1)
		{
			scale = 1.0f;
		}
		else
		if (type == 2)
		{
			scale = 0.5f;
		}
		transform.localScale *= scale;
	}
}
