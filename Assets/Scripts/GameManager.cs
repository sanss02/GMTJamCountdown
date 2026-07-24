using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float startingTime = 10f;
    [SerializeField] private float maxTime = 10f;
    [SerializeField] private float timeBonusPerTarget = 2f;

    public float TimeRemaining { get; private set; }
    public int TargetsDestroyed { get; private set; }

    // Otros scripts se suscriben a estos eventos en vez de que el GameManager
    // los busque y les hable directamente. Esto reduce el acoplamiento.
    public event Action<float> OnTimeChanged;
    public event Action<int> OnTargetDestroyed;
    public event Action OnGameOver;
    public event Action OnGameStarted;

    public enum GameState { Title, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Title;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartGame()
    {
        TimeRemaining = startingTime;
        TargetsDestroyed = 0;
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        OnTimeChanged?.Invoke(TimeRemaining);
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (CurrentState == GameState.Title && Keyboard.current.anyKey.isPressed)
        {
            StartGame();
        }

        if (CurrentState != GameState.Playing) return;

        TimeRemaining -= Time.deltaTime;
        OnTimeChanged?.Invoke(TimeRemaining);

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame();
        }

        Debug.Log(TimeRemaining); //BORRAR CUANDO APAREZCA LA UI
    }

    // Los Targets llaman esto cuando son destruidos
    public void RegisterTargetDestroyed()
    {
        TargetsDestroyed++;
        AddTime(timeBonusPerTarget);
        OnTargetDestroyed?.Invoke(TargetsDestroyed);
    }

    public void AddTime(float amount)
    {
        if (CurrentState != GameState.Playing) return;

        // Mathf.Min asegura que nunca pase el tope de 10 segundos
        TimeRemaining = Mathf.Min(TimeRemaining + amount, maxTime);
        OnTimeChanged?.Invoke(TimeRemaining);
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
        }
        else if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }

    private void EndGame()
    {
        CurrentState = GameState.GameOver;
        OnGameOver?.Invoke();
    }

    public void ReturnToTitle()
    {
        CurrentState = GameState.Title;
        Time.timeScale = 1f;
    }
}