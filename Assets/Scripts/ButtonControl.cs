using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
	public GameObject logo;

	Animator animator;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hover"))
		{
			print("**********");
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				SceneManager.LoadScene("GameScene");
			}
		}
	}

	void OnMouseEnter()
	{
		animator.SetBool("mouseOver", true);
		logo.GetComponent<LogoControl>().hover();
	}

	void OnMouseExit()
	{
		animator.SetBool("mouseOver", false);
		logo.GetComponent<LogoControl>().unhover();
	}

	void OnMouseDown()
	{
		animator.SetBool("mouseOver", false);
		logo.GetComponent<LogoControl>().click();
		Destroy(gameObject);
	}
}
