using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgControl : MonoBehaviour
{
	GameObject manager;

	Animator animator;
	float curSpeedRate;
	float playSpeedRate;
	float reverseSpeedRate;
	/*
	int desiredTimer;
	float animLength;
	int timer;
	*/

	// Use this for initialization
	void Start()
	{
		manager = GameObject.Find("GameManager");
			
		animator = GetComponent<Animator>();
		curSpeedRate = 0.0f;
		playSpeedRate = 1.0f;
		reverseSpeedRate = -5.0f;

		/*
		desiredTimer = -5;
		animLength = 4.25f;
		timer = 0;
		*/
	}

	// Update is called once per frame
	void Update()
	{
		float curTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		if (curSpeedRate > 0)
		{
			if (curTime >= 0.95f)
			{
				Reverse();
			}
		}
		else
		{
			if (curTime <= 0.05f)
			{
				Stop();
				animator.Play("StarLike", 0, 0.0f);
				manager.SendMessage("SetBallSpeedRate", 1.0f);
			}
		}

		//GetComponent<Animator>().Play(StarLike",0,.5f);

		print(curTime);

		animator.SetFloat("Direction", curSpeedRate);
	}

	public void Play()
	{
		curSpeedRate = playSpeedRate;
	}

	public void Stop()
	{
		curSpeedRate = 0.0f;
	}

	public void Reverse()
	{
		curSpeedRate = reverseSpeedRate;
	}
}
