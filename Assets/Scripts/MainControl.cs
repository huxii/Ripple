using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainControl : MonoBehaviour
{
	public GameObject ballPrefabs;
	public List<GameObject> ripplePrefabs;
	public List<GameObject> popPrefabs;

	public GameObject border;
	public GameObject bg;
	public GameObject sign;
	public Text scoreText;
	public Text timerText;
	public Text instruction;
	public Text hint;

	public float minVelocity;
	public float maxVelocity;
	public int maxBallNumber;
	public float minScale;

	enum GameState
	{
		Rotation = 0,
		RightClick = 1,
		LeftClick = 2,
		Game = 3,
		Over = 4,
	};

	GameState gameState;
    LoadingControl loader;
	GameObject bgm;
	List<GameObject> balls;
	int lastBouncerIndex;
	int lastBallColor;
	float ballSpeedRate;
	Vector3 defaultSpawnPos;
	int score;
	int timer;
	int curMaxBallNumber;
    int stateTimer;

	// Use this for initialization
	void Start()
	{
        loader = GameObject.FindGameObjectWithTag("Loader").GetComponent<LoadingControl>();
        loader.FadeIn(3f);

		bgm = GameObject.FindGameObjectWithTag("BGM");
        if (bgm && bgm.transform.position.x > 0)
        { 
			curMaxBallNumber = maxBallNumber;
			gameState = GameState.Game;
		}
		else
		{
			curMaxBallNumber = 2;
			gameState = GameState.Rotation;
		}

        Init();
	}
	
	// Update is called once per frame
	void Update()
	{
		/*
		if (bgm.GetComponent<BGMControl>().count > 0)
		{
			if (gameState != GameState.Over || gameState != GameState.Game)
			{
				hint.text = "Balls (have a maximum number) split and flip color after hitting the wall.";
			}
		}
		*/

		if (gameState == GameState.Rotation)
		{
			++stateTimer;
			instruction.text = "Move mouse to rotate the central circle";
			hint.text = "Balls (have a maximum number) split and flip color after hitting the wall.";
			//print(Input.GetAxis("Mouse X"));
			if (Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f)
			{
				if (stateTimer >= 10)
				{
					gameState = GameState.LeftClick;
					stateTimer = 0;
				}
			}
		}
		else
		if (gameState == GameState.LeftClick)
		{
			instruction.text = "'LEFT CLICK' to flip the color of central circle";
			hint.text = "Control central circle to abosorb balls with the same color and gain scores; if not the same color, lose scores and wall shrinks.";
			
			if (Input.GetMouseButtonDown(0))
			{
				FlipColor();
				gameState = GameState.RightClick;
			}
		}
		else
		if (gameState == GameState.RightClick)
		{
			++stateTimer;
			instruction.text = "Hold 'RIGHT CLICK' to slow down";	
			hint.text = "Slowing-down continues when timer has not gone to 0, which is recharged naturally.";
			
			if (Input.GetMouseButtonDown(1))
			{
				SetBallSpeedRate(0.1f);
				border.GetComponent<BorderControl>().SetSpeedRate(0.5f);
				bg.GetComponent<BgControl>().Play();
			}

			if (Input.GetMouseButtonUp(1))
			{
				SetBallSpeedRate(1.0f);
				border.GetComponent<BorderControl>().SetSpeedRate(1.0f);
				bg.GetComponent<BgControl>().Reverse();
				
				for (int i = 0; i < balls.Count; ++i)
				{
					Destroy(balls[i].gameObject);
				}

				Init();
				Expand();Expand();Expand();
				gameState = GameState.Game;
				curMaxBallNumber = maxBallNumber;
			}

			if (ballSpeedRate < 1.0f)
			{
				timer -= 10;
				if (timer < 0)
				{
					timer = 0;
					SetBallSpeedRate(1.0f);
					border.GetComponent<BorderControl>().SetSpeedRate(1.0f);
					bg.GetComponent<BgControl>().Reverse();
				}
			}
		}
		else
		if (gameState == GameState.Game)
		{
			if (stateTimer < 200)
			{
				++stateTimer;
				if (stateTimer == 200)
				{
					instruction.text = "";
					hint.text = "";		
				}
				else
				{
					instruction.text = "START!";	
					hint.text = "";			
				}
			}

			if (Input.GetMouseButtonDown(0))
			{
				FlipColor();
			}

			if (Input.GetMouseButtonDown(1))
			{
				SetBallSpeedRate(0.1f);
				border.GetComponent<BorderControl>().SetSpeedRate(0.5f);
				bg.GetComponent<BgControl>().Play();
			}

			if (Input.GetMouseButtonUp(1))
			{
				SetBallSpeedRate(1.0f);
				border.GetComponent<BorderControl>().SetSpeedRate(1.0f);
				bg.GetComponent<BgControl>().Reverse();
			}
			/*
			if (Input.GetKeyDown("space"))
			{
				SpawnNewBall(0, 0, RandomPosition(), RandomVelocity());
			}
			*/

			if (ballSpeedRate < 1.0f)
			{
				timer -= 10;
				if (timer < 0)
				{
					timer = 0;
					SetBallSpeedRate(1.0f);
					border.GetComponent<BorderControl>().SetSpeedRate(1.0f);
					bg.GetComponent<BgControl>().Reverse();
				}
			}
		}
		else
		{
			SetBallSpeedRate(0.05f);
			border.GetComponent<BorderControl>().SetSpeedRate(0.5f);
		}

		if (timer < 990)
		{
			++timer;
		}

		scoreText.text = score.ToString();
		timerText.text = (timer / 10).ToString();

		int length = balls.Count;
		for (int i = 0; i < length; ++i)
		{
			GameObject ball = balls[i];
			ball.GetComponent<BallControl>().SetSpeedRate(ballSpeedRate);
		}

		/*
		int length = balls.Count;
		for (int i = 0; i < length; ++i)
		{
			GameObject ball = balls[i];
			if (ball.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 1e-3)
			{
				ball.GetComponent<BallControl>().SetSpeed(RandomVelocity());
			}
		}
		*/
	}

	void Init()
	{
		balls = new List<GameObject>();
		SpawnNewBall(0, 1 - sign.GetComponent<SignControl>().spriteIndex, RandomPosition(), RandomVelocity());
		ballSpeedRate = 1.0f;
		defaultSpawnPos = new Vector3(0.0f, 0.0f, 10.0f);

		score = 0;
		timer = 300;
		stateTimer = 0;
		instruction.text = "";
		hint.text = "";
	}

	void GameOver()
	{
		print("over");
		gameState = GameState.Over;

        loader.Load(0, 3f);
	}

	void SpawnNewBall(int type, int colorIndex, Vector3 pos, Vector2 v)
	{
		GameObject ball = Instantiate(ballPrefabs, pos, Quaternion.identity) as GameObject;
		ball.GetComponent<BallControl>().Init(type, colorIndex, v.x, v.y);
		balls.Add(ball);		
	}

	void Scoring(int thisScore)
	{
		score += thisScore;
		scoreText.text = score.ToString();
	}

	void Shrink()
	{
		// sign animation
		sign.GetComponent<SignControl>().Shrink();
		border.GetComponent<BorderControl>().Shrink();

        if (border.transform.localScale.x - minScale < 1e-5)
        {
            GameOver();
        }
			
		Vector2 signPos = new Vector2(sign.transform.position.x, sign.transform.position.y);

		//print(signPos.x);
		//print(signPos.y);

		/*
		int length = balls.Count;
		for (int i = 0; i < length; ++i)
		{
			GameObject ball = balls[i];
			Vector2 pos = new Vector2(ball.transform.position.x, ball.transform.position.y);
			Vector2 dir = pos - signPos;
			if (dir.sqrMagnitude > radius)
			{
				dir = dir.normalized * radius;
				pos = signPos + dir;
				ball.GetComponent<BallControl>().SetPos(pos);
				//print("hhhhhhhhhhhhhhhh");
			}
		}
		*/
	}

	void Expand()
	{
		sign.GetComponent<SignControl>().Expand();
		border.GetComponent<BorderControl>().Expand();
	}

	Vector3 RandomPosition(float radius = 2.5f)
	{
		int angle = Random.Range(0, 360);
		float x = radius * Mathf.Cos(angle / Mathf.PI);
		float y = radius * Mathf.Sin(angle / Mathf.PI);
		return new Vector3(x, y, 10);
	}

	Vector2 RandomVelocity()
	{
		float x = Random.Range(minVelocity, maxVelocity);
		float y = Random.Range(minVelocity, maxVelocity);

		if (Random.Range(0, 1) == 0)
		{
			x *= -1;
		}
		if (Random.Range(0, 1) == 0)
		{
			y *= -1;
		}
			
		return new Vector2(x, y);
	}

	public void BallHitBorder(GameObject ball)
	{
		ball.GetComponent<BallControl>().collideDelay();
		lastBallColor = ball.GetComponent<BallControl>().color;

		Vector3 pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-8f, 8f), 10.0f);
		GameObject ripple = Instantiate(ripplePrefabs[lastBallColor], pos, Quaternion.identity) as GameObject;
		ripple.GetComponent<RippleControl>().Init(ball.GetComponent<BallControl>().type);
	}

	public void SplitBall(GameObject ball)
	{
		int type = ball.GetComponent<BallControl>().type;

		if (lastBouncerIndex <= 3)
		{
			if (type <= 1 && balls.Count < curMaxBallNumber)
			{
				Vector3 pos = ball.transform.position;
				Vector3 v = ball.GetComponent<Rigidbody2D>().velocity;

				Vector2 v0 = new Vector2();
				Vector2 v1 = new Vector2();
				Vector3 pos0 = pos;
				Vector3 pos1 = pos;
				if (lastBouncerIndex == 0)
				{
					// top
					v0.x = v.y;
					v1.x = -v.y;
					v0.y = Mathf.Abs(v.x);
					v1.y = v0.y;
					pos0.x += 0.3f;
					pos1.x -= 0.3f;
				}
				else
				if (lastBouncerIndex == 1)
				{
					// bottom
					v0.x = v.y;
					v1.x = -v.y;
					v0.y = Mathf.Abs(v.x) * -1;
					v1.y = v0.y;	
					pos0.x += 0.3f;
					pos1.x -= 0.3f;
				}
				else
				if (lastBouncerIndex == 2)
				{
					// right
					v0.y = v.x; 
					v1.y = -v.x;
					v0.x = Mathf.Abs(v.y) * -1;
					v1.x = v0.x;
					pos0.y += 0.3f;
					pos1.y -= 0.3f;
				}
				else
				if (lastBouncerIndex == 3)
				{
					//left
					v0.y = v.x;
					v1.y = -v.x;
					v0.x = Mathf.Abs(v.y);
					v1.x = v0.x;
					pos0.y += 0.3f;
					pos1.y -= 0.3f;
				}
                
				SpawnNewBall(type + 1, 1 - ball.GetComponent<BallControl>().color, pos0, v0);
				SpawnNewBall(type + 1, 1 - ball.GetComponent<BallControl>().color, pos1, v1);

				balls.Remove(ball);
				Destroy(ball);
			}

			//print(lastBouncerIndex);
		}
			
		//print(balls.Count);
	}

	public void BallHitSign(GameObject ball)
	{
		if (ball)
		{
			Destroy(ball);
			balls.Remove(ball);

			if (balls.Count < curMaxBallNumber - 1)
			{
				Vector3 pos = RandomPosition();
				Vector3 v = RandomVelocity();
				Vector3 dir = defaultSpawnPos - pos;
				Vector3 V = dir.normalized * v.magnitude;
				SpawnNewBall(0, 1 - sign.GetComponent<SignControl>().spriteIndex, pos, V);
			}

			int signColor = sign.GetComponent<SignControl>().spriteIndex;
			int ballColor = ball.GetComponent<BallControl>().color;

			if (signColor != ballColor)
			{
                GameObject pop = Instantiate(popPrefabs[1], border.transform) as GameObject;
                Scoring(-(ball.GetComponent<BallControl>().type + 1));
				Shrink();
				//print("....");
			}
			else
			{
                GameObject pop = Instantiate(popPrefabs[0], border.transform) as GameObject;
                int tmp = ball.GetComponent<BallControl>().type + 1;
				Scoring(tmp * tmp);
				//Expand();
			}
		}

		// ink
	}

	public void FlipColor()
	{
		/*
		int length = bouncers.Count;
		for (int i = 0; i < length - 1; ++i)
		{
			//bouncers[i].GetComponent<BorderControl>().FlipColor();
		}

		bouncers[length - 1].GetComponent<SignControl>().FlipColor();
		*/
		sign.GetComponent<SignControl>().FlipColor();
	}

	public void SetBallSpeedRate(float speedRate)
	{
		ballSpeedRate = speedRate;
	}
}
