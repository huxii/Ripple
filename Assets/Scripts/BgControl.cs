﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgControl : MonoBehaviour
{
	GameObject manager;

	Animator animator;
	float curSpeedRate;
	float playSpeedRate;
	float reverseSpeedRate;
    bool slowDown;

	// Use this for initialization
	void Start()
	{
		manager = GameObject.FindGameObjectWithTag("Manager");
			
		animator = GetComponent<Animator>();
		curSpeedRate = 0.0f;
		playSpeedRate = 3.0f;
		reverseSpeedRate = -5.0f;
        slowDown = false;
	}

	// Update is called once per frame
	void Update()
	{
        // manually reset
		float curTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("StarDust") && curTime <= 0.05f && !slowDown)
		{
            animator.Play("BgAppear", 0, 0.0f);
		}
		//GetComponent<Animator>().Play(StarLike",0,.5f);

		//print(curTime);

		animator.SetFloat("direction", curSpeedRate);
	}

    public void SlowDown()
    {
        curSpeedRate = playSpeedRate;
        animator.SetFloat("direction", curSpeedRate);
        animator.SetBool("slowDown", true);
    }

    public void BackToNormal()
    {
        curSpeedRate = reverseSpeedRate;
        animator.SetFloat("direction", curSpeedRate);
        animator.SetBool("slowDown", false);
    }
}
