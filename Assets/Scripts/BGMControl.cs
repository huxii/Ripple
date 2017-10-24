using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMControl : MonoBehaviour
{
	public int count;

	AudioSource bgm;

	// Use this for initialization
	void Start()
	{
		count = 0;

		GameObject[] objs = GameObject.FindGameObjectsWithTag("BGM");
		if (objs.Length > 1)
		{
			objs[0].GetComponent<BGMControl>().count++;
			Destroy(this.gameObject);
		}
		else
		{
			DontDestroyOnLoad(this.gameObject);
		}

		bgm = GetComponent<AudioSource>();
		bgm.loop = true;
		bgm.Play();
	}
	
	// Update is called once per frame
	void Update()
	{
	}
}
