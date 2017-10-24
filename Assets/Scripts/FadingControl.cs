using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingControl : MonoBehaviour
{
	public float fadingSpeed = 1.0f;
	public int fadingDir;

	float alpha;
	GameObject manager;

	// Use this for initialization
	void Start()
	{
		alpha = 0.5f - fadingDir * 0.5f;
		manager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update()
	{
		alpha += fadingDir * fadingSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);

		if (fadingDir == -1)
		{
			if (alpha <= 0.1f)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			if (alpha >= 0.9f)
			{
				manager.SendMessage("FadedOut");
			}	

			if (alpha >= 0.99f)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
