using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStateChanged;

    [SerializeField] List<BaseCounter> counters; // used to remove items on game restart

    private enum GameState
    { 
        WaitingToStart,
        CountdownToStart,
        Playing,
        GameOver
    }

    private GameState state;
    private float startTimer;
    private float countdownTimer;
    private float gameOverTimer;
    private float gameOverTimerMax;
    private bool isGamePaused;

    private void Awake()
    {
        Instance = this;
        state = GameState.WaitingToStart;
    }

    private void Start()
    {
        Initialize();
        GameInput.Instance.OnPause += GameInput_OnPause;
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        if (!OptionsUI.Instance.isActive)
        {
            TogglePause();
        }        
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.WaitingToStart:
                startTimer -= Time.deltaTime;
                if (startTimer <= 0)
                {
                    state = GameState.CountdownToStart;
                }
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.CountdownToStart:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0)
                {
                    state = GameState.Playing;
                }
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.Playing:
                gameOverTimer -= Time.deltaTime;
                if (gameOverTimer <= 0)
                {
                    state = GameState.GameOver;
                }
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                break;

            case GameState.GameOver:
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public void TogglePause()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
        isGamePaused = !isGamePaused;
    }

    public bool isGamePlaying() => state == GameState.Playing;

    public bool isCountdown() => state == GameState.CountdownToStart;

    public bool isGameOver() => state == GameState.GameOver;

    public float GetCountdownTimer() => countdownTimer;
    public float GetGameOverTimer() => gameOverTimer;
    public float GetGameOverTimerMax() => gameOverTimerMax;
    public bool GetIsGamePaused() => isGamePaused;

    public void Initialize()
    {
        state = GameState.WaitingToStart;
        startTimer = 1f;
        countdownTimer = 3f;
        gameOverTimerMax = 90f;
        gameOverTimer = gameOverTimerMax;
        isGamePaused = false;
        GameClockUI.Instance.Initialize();
        GameStatsUI.Instance.Initialize();
        DeliveryManager.Instance.Initialize();
        DeliveryManagerUI.Instance.Initialize();
        Player.Instance.Initialize();
    }

    public void RestartGame()
    {
        Initialize();

        // remove all ingredients that are still present on the scene
        foreach (BaseCounter counter in counters)
        {
            if (counter.HasKitchenObject())
            {
                counter.GetKitchenObject().DestroySelf();
            }
            if (counter is Stove)  // reset state to Idle
            {
                (counter as Stove).Initialize();
            }
            if (counter is CutterCounter)  // hide progress bar
            {
                (counter as CutterCounter).Initialize();
            }
        }
        
        if (Player.Instance.HasKitchenObject()) {
            Player.Instance.GetKitchenObject().DestroySelf();
        }
    }
}