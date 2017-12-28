using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoDestroyControl : MonoBehaviour {

    // Use this for initialization
    public ObjectType type = ObjectType.lifespan;
    public enum ObjectType
    {
        particle = 1,
        lifespan = 2,
    };

    public float lifeSpan = 3f;

    ParticleSystem ps;

	void Start ()
    {
        if (type == ObjectType.particle)
        {
            ps = GetComponent<ParticleSystem>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (type == ObjectType.particle)
        {
            if (ps && !ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
        else
        if (type == ObjectType.lifespan)
        {
            if (lifeSpan > 0)
            {
                lifeSpan -= Time.deltaTime;
                if (lifeSpan <= 0)
                {
                    gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(
                        () => { Destroy(gameObject); }
                        );
                }
            }
        }
	}
}
