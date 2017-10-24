using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		this.transform.eulerAngles = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// rotate arrow
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 pos = this.transform.position;
		float x = mousePos.x - pos.x;
		float y = mousePos.y - pos.y;
		float z = Mathf.Sqrt (x * x + y * y);
		float angle = Mathf.Round (Mathf.Asin (Mathf.Abs (y / z)) / Mathf.PI * 180);

		if (x < 0)
		{
			if (y > 0)
			{
				angle = 90 - angle;
			}
			else
			{
				angle = angle + 90;
			}
		}
		else
		{
			if (y < 0)
			{
				angle = 270 - angle;
			}
			else
			{
				angle = 270 + angle;
			}
		}

		this.transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
