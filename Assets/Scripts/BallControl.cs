using UnityEngine;
using System.Collections;

public class BallControl : MonoBehaviour 
{

	//public variables are accessible by other scripts, and are often set in the inspector
	//they're great for tunable variables, like these, since we can edit them in play mode.
	//public float horizontalSpeed; //the ball's constant horizontal speed
	//public float maxVerticalSpeed; //the maximum vertical speed

	public int type;
	public int color;
	public int timer;

	//private variables are more like the global variables in Phaser, and 
	//they can't be accessed by other scripts
	Rigidbody2D rb; //a reference to the Rigidbody2D component on this object
	GameObject manager;
	Animator animator;

	int spriteIndex;
	float initSpeed;
	float ballSpeedRate;
	int delayTime;

	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find ("GameManager");
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		spriteIndex = 0;
		delayTime = 5;
		initSpeed = 3.5f;
		ballSpeedRate = 1.0f;
		//timer = -10;
	}

	void Update()
	{	
		++timer;
		if (timer == 0) 
		{
			manager.SendMessage ("SplitBall", this.gameObject);	
		}

		//if (Mathf.abs(rb.velocity.x) < 0.2)
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
			manager.SendMessage("BallHitBorder", this.gameObject);

			if (color == 0)
			{
				animator.Play("Bounce1", 0);
			}
			else
			{
				animator.Play("Bounce0", 0);
			}
		}
		else
		{		
			if (color == 0)
			{
				animator.Play("Bounce0", 0);
			}
			else
			{
				animator.Play("Bounce1", 0);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Sign") 
		{
			manager.SendMessage ("BallHitSign", this.gameObject);
		}
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

		if (color == 0)
		{
			GetComponent<Animator>().Play("Spawn0", 0);
		}
		else
		{
			GetComponent<Animator>().Play("Spawn0", 0);
		}
	}

	public void collideDelay()
	{
		timer = -delayTime;
	}

	public void FlipColor()
	{
		color = 1 - color;
		animator.SetInteger("color", color);
		//spriteRenderer.sprite = sprites[color];
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
}
