using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownControl : MonoBehaviour
{
	Animator animator;
	AudioSource audio;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
		float curTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		if (curTime > 0.0f)
		{
			audio.Play();
		}
		else
		{
			audio.Stop();
		}
	}
}
