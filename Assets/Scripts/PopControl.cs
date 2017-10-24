using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopControl : MonoBehaviour
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
		if (curTime >= 0.99f)
		{
			DestroyObject(gameObject);
		}
	}

	public void Init(float scale)
	{
		transform.localScale *= scale;
	}
}
