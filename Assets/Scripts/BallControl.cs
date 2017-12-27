using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BallControl : MonoBehaviour 
{

    //public variables are accessible by other scripts, and are often set in the inspector
    //they're great for tunable variables, like these, since we can edit them in play mode.
    //public float horizontalSpeed; //the ball's constant horizontal speed
    //public float maxVerticalSpeed; //the maximum vertical speed
    public Sprite[] sprites;
    public GameObject hitParticlePrefab;
	public int type;
	public int color;
	public int timer;

	//private variables are more like the global variables in Phaser, and 
	//they can't be accessed by other scripts
	Rigidbody2D rb; //a reference to the Rigidbody2D component on this object
	MainControl manager;
    Animator animator;
    SpriteRenderer spriteRenderer;
    AudioSource hitConAudio;
    int spriteIndex;
	float initSpeed;
	float ballSpeedRate;
    float scale;
    bool dying;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find ("GameManager").GetComponent<MainControl>();
		rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hitConAudio = GetComponentInChildren<AudioSource>();

        spriteIndex = 0;
		initSpeed = 3.5f;
		ballSpeedRate = 1.0f;
        timer = 0;
        dying = false;
    }

	void Update()
	{	
		++timer;

        if (!dying && OutOfBorder())
        {
            Die(1f, true);
        }
    }

	// FixedUpdate is called along with the physics engine, at regular time intervals 
	// It's often used whenever you want to interact with physics components, as we do here
	void FixedUpdate()
	{
		float desiredSpeed = initSpeed * ballSpeedRate;
		float currentSpeed = rb.velocity.magnitude ;
		if (currentSpeed > 0)
		{
			rb.velocity *= desiredSpeed / currentSpeed;
		}
		//so for example, if currentSpeed is 2.5, we will multiply the velocity by 2.0
	}

	void OnCollisionEnter2D(Collision2D thisCollision)
	{
        if (thisCollision.collider.tag == "Wall")
        {
            if (timer >= 5)
            {
                if (manager.BallHitBorder(gameObject))
                {
                    Split();
                }
                else
                {
                    Bounce();
                    FlipColor();
                }
            }
		}
        else
        if (thisCollision.collider.tag == "Player")
        {
            Bounce();
            hitConAudio.Play();
        }

    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Sign") 
		{
			manager.BallHitSign(gameObject);
		}
	}

    bool OutOfBorder()
    {
        // outside of border
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            new Vector2(0, 0), new Vector2(transform.position.x, transform.position.y), transform.position.magnitude
        );
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                manager.RemoveBall(gameObject);
                return true;
            }
        }

        return false;
    }

	public void Init(int _type, int _colorIndex, float _vx, float _vy)
	{
		//first we fill our Rigidbody2D reference with the component on this obejct
		rb = GetComponent<Rigidbody2D> ();
		//then give it a starting velocity, up and to the right (remember positive Y is up in Unity!)
		rb.velocity = new Vector2 (_vx, _vy); 
		initSpeed = rb.velocity.magnitude;

		type = _type;
		color = _colorIndex;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprites[color];

        switch (type)
        {
            case 0:
                scale = 1f;
                break;
            case 1:
                scale = 0.8f;
                break;
            case 2:
                scale = 0.5f;
                break;
            default:
                break;
        }

        if (!dying)
        {
            transform.localScale = new Vector3(0, 0, 0);
            Born();
        }
    }

	public void FlipColor()
	{
		color = 1 - color;
		//animator.SetInteger("color", color);
		spriteRenderer.sprite = sprites[color];
	}

	public void SetPos(Vector2 pos)
	{
		transform.position = new Vector3(pos.x, pos.y, 10);
	}

	public void SetSpeed(Vector2 speed)
	{
		rb.velocity = speed;
	}

	public void SetSpeedRate(float rate)
	{
		ballSpeedRate = rate;
	}

    public void Born()
    {
        if (dying)
        {
            return;
        }

        transform.DOScale(new Vector3(scale, scale, scale), 0.3f).SetEase(Ease.InOutBack);
    }

    public void Split()
    {
        if (dying)
        {
            return;
        }

        rb.velocity = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.InOutCubic).OnComplete(
            () => {  Destroy(gameObject); }
        );

        GameObject particle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
    }

    public void Bounce()
    {
        if (dying)
        {
            return;
        }

        transform.DOShakeScale(0.25f, new Vector3(scale * 0.3f, scale * 0.3f, 0), 1, 90, true).OnComplete(
            () => { transform.DOScale(new Vector3(scale, scale, scale), 0.25f); }
        );
    }

    public void Die(float duration = 0.3f, bool stop = false)
    {
        if (dying)
        {
            return;
        }
        dying = true;
        if (stop)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
        transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(Ease.InOutCubic).OnComplete(
            () => { Destroy(gameObject); }
        );
    }
}
