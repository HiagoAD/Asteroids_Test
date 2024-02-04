using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Tags
{
    public static string PLAYER = "Player";
    public static string BULLET = "Bullet";
    public static string ASTEROID = "Asteroid";
    public static string SAUCER = "Saucer";
    public static string SAUCER_BULLET = "Bullet_saucer";
}


public class GameManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameBalanceValues gameBalance;

    [Header("Player")]
    [SerializeField] Player player = null;

    [Header("Levels")]
    [SerializeField] LevelScriptableObject[] levels;

    [Header("Saucer")]
    [SerializeField] Saucer saucer;

    public static GameManager Instance { get; private set; }

    private int score = 0;
    private int maxScore = 0;
    private int life = 0;
    private bool alive = true;
    private bool gameStarted = false;
    private float saucerTimeOfDeath = -Mathf.Infinity;

    private int currentLevel = 0;

    private readonly List<Asteroid> asteroidsAlives = new();



    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameBalance.SetActive();
    }

    void Start()
    {
        maxScore = PlayerPrefs.GetInt("score", 0);
        CanvasManager.Instance.SetMaxScore(maxScore);
    }

    void Update() {
        if(gameStarted) {
            if (Time.time > saucerTimeOfDeath + gameBalance.Saucer.TimeToSpawn && !saucer.gameObject.activeInHierarchy)
            {
                saucer.Init(player);
            }
        }
    }

    void SpawnCurrentLevel()
    {
        LevelScriptableObject levelObj = levels[currentLevel];
        foreach (AsteroidObjectDefinition def in levelObj.asteroids)
        {
            Asteroid instance = Instantiate(def.prefab);
            instance.transform.position = def.position;
            instance.transform.localRotation = def.rotation;
            instance.StartSpeed = def.speed;



            AsteroidCreated(instance);
        }
    }

    void Endgame()
    {
        Debug.Log("ENDGAME");
        if (score > maxScore)
        {
            maxScore = score;
            CanvasManager.Instance.SetMaxScore(maxScore);
            PlayerPrefs.SetInt("score", maxScore);
        }
        gameStarted = false;

        StartCoroutine(ResetGame());

        CanvasManager.Instance.OnEndGame();

        Time.timeScale = 0f;
    }

    IEnumerator ResetGame() {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    void ResetPlayer()
    {
        player.Reset();
        alive = true;
    }

    public void StartGame()
    {
        life = gameBalance.Life.MaxAmount;
        gameStarted = true;
        saucerTimeOfDeath = Time.time;
        SpawnCurrentLevel();

        CanvasManager.Instance.UpdateScore(score);
    }

    public void PlayerThrust(float rawValue)
    {
        if (alive) player.Thrust(rawValue);
    }

    public void PlayerRotate(float rawValue)
    {
        if (alive) player.Rotate(-rawValue);
    }

    public void PlayerShoot()
    {
        if (alive) player.Shoot();
    }

    public void PlayerHyperspace()
    {
        if (alive) player.Hyperspace();
    }

    public void AsteroidCreated(Asteroid asteroid)
    {
        asteroidsAlives.Add(asteroid);

        StartCoroutine(asteroid.FadeIn(asteroid.Init));
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        score += asteroid.Value;
        asteroidsAlives.Remove(asteroid);

        CanvasManager.Instance.UpdateScore(score);


        if (asteroidsAlives.Count == 0)
        {
            currentLevel = currentLevel >= levels.Length - 1 ? levels.Length - 1 : currentLevel + 1;
            SpawnCurrentLevel();
        }
    }

    public void SaucerDestroyed(int value)
    {
        score += value;
        saucerTimeOfDeath = Time.time;
    }

    public void PlayerHit()
    {
        alive = false;
        life--;
        CanvasManager.Instance.UpdateLifeAmount(life);

        Action callback;
        if (life > 0)
        {
            callback = ResetPlayer;
        }
        else
        {
            callback = Endgame;
        }

        StartCoroutine(player.FadeOut(callback));
    }
}