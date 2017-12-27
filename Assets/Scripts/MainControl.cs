using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainControl : MonoBehaviour
{
    public enum GameState
    {
        Rotation = 0,
        RightClick = 1,
        LeftClick = 2,
        Game = 3,
        Over = 4,
        Undefined = 5,
    };

    public GameState gameState;

    public GameObject ballPrefabs;
    public List<GameObject> ripplePrefabs;
    public List<GameObject> popPrefabs;

    public GameObject objects;
    public GameObject border;
    public GameObject bg;
    public GameObject sign;
    public GameObject ui;
    public GameObject hints;
    public Text scoreText;
    public Text timerText;
    public Text instruction;
    public Text hint;

    public float minVelocity;
    public float maxVelocity;
    public int maxBallNumber;
    public float minScale;

    public GameObject gameOverObj;
    public GameObject shewObj;

    LoadingControl loader;
    GameObject bgm;
    AudioSource gameOverSound;
    AudioSource shewSound;
    List<GameObject> balls;
    float ballSpeedRate;
    Vector3 defaultSpawnPos;
    int score;
    int timer;
    int curMaxBallNumber;
    int stateTimer;

    // Use this for initialization
    void Start()
    {
        gameState = GameState.Undefined;
        loader = GameObject.FindGameObjectWithTag("Loader").GetComponent<LoadingControl>();
        bgm = GameObject.FindGameObjectWithTag("BGM");
        gameOverSound = gameOverObj.GetComponent<AudioSource>();
        shewSound = shewObj.GetComponent<AudioSource>();

        PrepareAssets();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Undefined)
        {
            return;
        }

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
                SlowDown();
            }

            if (Input.GetMouseButtonUp(1))
            {
                BackToNormal();

                for (int i = 0; i < balls.Count; ++i)
                {
                    balls[i].GetComponent<BallControl>().Die();
                }

                Init();
                Expand(); Expand(); Expand();
                gameState = GameState.Game;
                curMaxBallNumber = maxBallNumber;
            }

            if (ballSpeedRate < 1.0f)
            {
                timer -= 10;
                if (timer < 0)
                {
                    timer = 0;

                    BackToNormal();
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
                SlowDown();
            }

            if (Input.GetMouseButtonUp(1))
            {
                BackToNormal();
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
                    BackToNormal();
                }
            }
        }
        else
        if (gameState == GameState.Over)
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

        if (balls != null)
        {
            int length = balls.Count;
            for (int i = 0; i < length; ++i)
            {
                GameObject ball = balls[i];
                ball.GetComponent<BallControl>().SetSpeedRate(ballSpeedRate);
            }
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

    void PrepareAssets()
    {
        loader.FadeIn(0.5f);

        objects.transform.localScale = new Vector3(0, 0, 0);
        ui.transform.position = new Vector3(0, 800f, 0);
        hints.transform.localScale = new Vector3(0, 0, 0);

        objects.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.7f).SetEase(Ease.OutBack).SetDelay(1f)
            .OnStart(
                () => { shewSound.Play(); }
            )
            .OnComplete(
                () => {
                    shewSound.Play();
                    hints.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f).SetEase(Ease.OutQuad);
                }
            );
        ui.transform.DOMove(new Vector3(0, 0, 0), 2.5f).SetEase(Ease.OutQuart)
            .OnStart(() => { shewSound.Play(); })
            .OnComplete(StartGame);
    }

    void CollectAssets()
    {
        ui.transform.DOMove(new Vector3(0, 30f, 0), 1f).SetEase(Ease.OutQuart).SetDelay(1).OnStart(
            () => { gameOverSound.Play(); shewSound.Play(); }
            )
            .OnComplete(
            () => { bg.GetComponent<BgControl>().GameOver(); loader.Load(0, 1f, false); }
            );

        hints.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutQuad).SetDelay(1.4f)
            .OnStart(
                () => { shewSound.Play(); }
            )
            .OnComplete(
            () => { shewSound.Play(); objects.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.4f).SetEase(Ease.OutCubic); }
            );
    }

    void StartGame()
    {
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

    void Init()
	{
		balls = new List<GameObject>();
		SpawnNewBall(0, 1 - sign.GetComponent<SignControl>().spriteIndex, RandomPosition(), RandomVelocity());
		ballSpeedRate = 1.0f;
		defaultSpawnPos = new Vector3(0.0f, 0.0f, 0.0f);

		score = 0;
		timer = 300;
		stateTimer = 0;
		instruction.text = "";
		hint.text = "";
	}

	void GameOver()
	{
        if (gameState != GameState.Over)
        {
            print("over");
            gameState = GameState.Over;

            CollectAssets();
        }
    }

    void SlowDown()
    {
        SetBallSpeedRate(0.1f);
        border.GetComponent<BorderControl>().SetSpeedRate(0.5f);
        bg.GetComponent<BgControl>().SlowDown();
        if (bgm)
        {
            bgm.GetComponent<AudioSource>().DOFade(0.2f, 0.5f);
        }
    }

    void BackToNormal()
    {
        SetBallSpeedRate(1.0f);
        border.GetComponent<BorderControl>().SetSpeedRate(1.0f);
        bg.GetComponent<BgControl>().BackToNormal();
        if (bgm)
        {
            bgm.GetComponent<AudioSource>().DOFade(1.0f, 0.5f);
        }
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
		return new Vector3(x, y, 0);
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

	public bool BallHitBorder(GameObject ball)
	{
		int colorIdx = ball.GetComponent<BallControl>().color;

		Vector3 pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-8f, 8f), 0);
		GameObject ripple = Instantiate(ripplePrefabs[colorIdx], pos, Quaternion.identity) as GameObject;
		ripple.GetComponent<RippleControl>().Init(ball.GetComponent<BallControl>().type);

        int type = ball.GetComponent<BallControl>().type;
        if (type <= 1 && balls.Count < curMaxBallNumber)
        {
            SplitBall(ball);
            return true;
            //
        }
        else
        {
            //RemoveBall(ball);
        }

        return false;
	}

	public void SplitBall(GameObject ball)
	{
		int type = ball.GetComponent<BallControl>().type;
        Vector3 pos = ball.transform.position;
        Vector3 v = ball.GetComponent<Rigidbody2D>().velocity;
        Vector2 v0 = new Vector2();
        Vector2 v1 = new Vector2();
        Vector3 pos0 = pos;
        Vector3 pos1 = pos;
        v0.x = v.y;
        v0.y = -v.x;
        v1.x = -v.y;
        v1.y = v.x;

        SpawnNewBall(type + 1, 1 - ball.GetComponent<BallControl>().color, pos0, v0);
        SpawnNewBall(type + 1, 1 - ball.GetComponent<BallControl>().color, pos1, v1);

        balls.Remove(ball);
	}

    public void BallHitSign(GameObject ball)
	{
		if (ball)
		{
			if (balls.Count <= curMaxBallNumber - 1)
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

            balls.Remove(ball);
            ball.GetComponent<BallControl>().Die();
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

    public void RemoveBall(GameObject ball)
    {
        balls.Remove(ball);
    }
}
