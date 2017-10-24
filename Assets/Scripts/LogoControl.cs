using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoControl : MonoBehaviour
{
	Animator animator;
	GameObject manager;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		manager = GameObject.Find ("MenuManager");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Start"))
		{
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				manager.SendMessage("Loading");
				Destroy(this.gameObject);
			}
		}
	}

	public void hover()
	{
		animator.SetInteger("mouseState", 1);
	}

	public void unhover()
	{
		animator.SetInteger("mouseState", 0);
	}

	public void click()
	{
		animator.SetInteger("mouseState", 2);
	}
}
