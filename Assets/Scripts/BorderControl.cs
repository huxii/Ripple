using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BorderControl : MonoBehaviour 
{
	public float shrinkScale;
    public GameObject inner;
    public GameObject outer;

	GameObject manager;
	Animator animator;

    float scale;
	float originalScale;

	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		animator = outer.GetComponent<Animator>();

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
            inner.transform.DOShakeScale(0.25f, new Vector3(0.02f, 0.02f, 0), 1, 90, true).OnComplete(
                () => { inner.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f); }
                );
            outer.transform.DOScale(new Vector3(1.1f, 1.1f, 0), 0.25f).OnComplete(
               () => { outer.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f); }
               );
        }
	}

	public void Shrink()
	{
        Vector3 localPos = transform.localPosition;
        scale = transform.localScale.x * shrinkScale;
        transform.DOShakePosition(1f, 0.4f).SetEase(Ease.InOutCubic).OnComplete(
            () => { transform.DOLocalMove(localPos, 0.2f); }
        );
        transform.DOScale(new Vector3(scale, scale, scale), 0.6f).SetEase(Ease.InOutCubic);
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
