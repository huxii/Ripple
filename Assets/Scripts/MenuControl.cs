using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameObject BGMPrefab;

    LoadingControl loader;
    
	// Use this for initialization
	void Start ()
	{
        loader = GameObject.FindGameObjectWithTag("Loader").GetComponent<LoadingControl>();
        loader.FadeIn(1.5f);

        GameObject bgm = GameObject.FindGameObjectWithTag("BGM");
        if (bgm == null)
        {
            bgm = Instantiate(BGMPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            DontDestroyOnLoad(bgm);
        }
        else
        {
            //bgm.transform.position = new Vector3(1.0f, 1.0f, 1.0f);
        }
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
