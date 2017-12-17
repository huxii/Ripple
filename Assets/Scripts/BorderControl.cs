using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BorderControl : MonoBehaviour 
{
	public float shrinkScale;

	GameObject manager;
	Animator animator;

    float scale;
	float originalScale;

	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		animator = GetComponentInChildren<Animator>();

		originalScale = transform.localScale.x;
        scale = originalScale;

    }
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnCollisionEnter2D(Collision2D thisCollision)
	{
		if (thisCollision.collider.tag == "Ball") 
		{
            transform.DOShakeScale(0.25f, new Vector3(0.02f, 0.02f, 0), 1, 90, true).OnComplete(
                () => { transform.DOScale(new Vector3(scale, scale, scale), 0.25f); }
                );
		}
	}

	public void Shrink()
	{
        scale = transform.localScale.x * shrinkScale;
        transform.DOShakePosition(1f, 0.5f);
        transform.DOScale(new Vector3(scale, scale, scale), 0.5f).SetEase(Ease.InOutCubic);
	}

	public void Expand()
	{
		if (Mathf.Abs(transform.localScale.x - originalScale) > 1e-5)
		{
            scale = transform.localScale.x / shrinkScale;
            transform.DOScale(new Vector3(scale, scale, scale), 0.5f).SetEase(Ease.InOutCubic);
        }
	}

	public void SetSpeedRate(float speedRate)
	{
		animator.SetFloat("speed", speedRate);
	}
}
