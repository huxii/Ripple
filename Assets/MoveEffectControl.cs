﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffectControl : MonoBehaviour
{
    public Vector2 friction = new Vector2(1.0f, 1.0f);

    float x;
    float y;

	// Use this for initialization
	void Start ()
    {
        x = 0;
        y = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float lMouseX = Mathf.Max(-100f, Mathf.Min(100, Screen.width / 2 - Input.mousePosition.x));
        float lMouseY = Mathf.Max(-100f, Mathf.Min(100, Screen.height / 2 - Input.mousePosition.y));
        float lFollowX = (0.1f * lMouseX) / 100 * friction.x; // 100 : 12 = lMouxeX : lFollow
        float lFollowY = (0.05f * lMouseY) / 100 * friction.x;
        x += (lFollowX - x) * 0.01f;
        y += (lFollowY - y) * 0.01f;
        transform.position = new Vector3(x, y, 0);
    }
}
