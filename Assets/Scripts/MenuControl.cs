using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
	LoadingControl loader;

	// Use this for initialization
	void Start ()
	{
        loader = GameObject.FindGameObjectWithTag("Loader").GetComponent<LoadingControl>();
        loader.FadeIn(1.5f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Load(int num)
	{
        loader.Load(num);
	}
}
