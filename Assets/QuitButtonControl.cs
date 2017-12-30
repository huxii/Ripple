using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuitButtonControl : MonoBehaviour
{
    LoadingControl loader;

    // Use this for initialization
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Destroy(gameObject);
        }

        loader = GameObject.FindGameObjectWithTag("Loader").GetComponent<LoadingControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f);
    }

    void OnMouseExit()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
    }

    void OnMouseDown()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f).OnComplete(
            () => { transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f); }
            );
        loader.Quit();
    }
}
