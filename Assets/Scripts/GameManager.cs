using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Timer Settings")]
    [SerializeField] private float startingTime = 10f;
    [SerializeField] private float maxTime = 10f;
    public float timeBonusPerTarget = 2f;

    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Color selectedEasyColor;
    [SerializeField] private Color selectedMediumColor;
    [SerializeField] private Color selectedHardColor;

    [SerializeField] private Color normalColor;

    public float TimeRemaining { get; private set; }
    public int TargetsDestroyed { get; private set; }
    public int HighScore { get; private set; }
    private bool isPlayingFinalSecondsWarning = false;
    private static bool shouldAutoStart = false;

    public event Action<GameState> OnStateChanged;
    public event Action<float> OnTimeChanged;
    public event Action<int> OnTargetDestroyed;
    public event Action OnGameOver;
    public event Action OnGameStarted;

    public enum GameState { Title, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Title;
    private bool isReloading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        HighScore = PlayerPrefs.GetInt("HighScore", 0);

        Instance = this;
    }

    private void Start()
    {
        HighlightSelected(easyButton); // Easy queda marcado desde el inicio
    }
    public void SetDifficultyEasy()
    {
        SetDifficulty(10f, 10f, 2f);
        HighlightSelected(easyButton);
    }

    public void SetDifficultyMedium()
    {
        SetDifficulty(7f, 7f, 1.5f);
        HighlightSelected(mediumButton);
    }

    public void SetDifficultyHard()
    {
        SetDifficulty(5f, 5f, 1f);
        HighlightSelected(hardButton);
    }

    private void SetDifficulty(float newStartingTime, float newMaxTime, float newTimeBonus)
    {
        startingTime = newStartingTime;
        maxTime = newMaxTime;
        timeBonusPerTarget = newTimeBonus;
    }

    private void HighlightSelected(Button selectedButton)
    {
        easyButton.image.color = normalColor;
        mediumButton.image.color = normalColor;
        hardButton.image.color = normalColor;

        if(selectedButton == easyButton)
        {
            selectedButton.image.color = selectedEasyColor;
        }
        if(selectedButton == mediumButton)
        {
            selectedButton.image.color = selectedMediumColor;
        }
        if(selectedButton == hardButton)
        {
            selectedButton.image.color = selectedHardColor;
        }
    }

    public void StartGame()
    {
        if (CurrentState == GameState.Playing) return;

        AudioManager.Instance.PlaySFXClickButton();
        isPlayingFinalSecondsWarning = false;
        TimeRemaining = startingTime;
        TargetsDestroyed = 0;
        CurrentState = GameState.Playing;
        OnStateChanged?.Invoke(CurrentState);
        Time.timeScale = 1f;
        OnTimeChanged?.Invoke(TimeRemaining);
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (isReloading) return;

        if (shouldAutoStart)
        {
            shouldAutoStart = false;
            StartGame();
        }
        
        if (CurrentState == GameState.Title && Keyboard.current.anyKey.isPressed)
        {
            StartGame();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && (CurrentState == GameState.Playing || CurrentState == GameState.Paused))
        {
            TogglePause();
        }

        if (CurrentState != GameState.Playing) return;

        TimeRemaining -= Time.deltaTime;
        OnTimeChanged?.Invoke(TimeRemaining);

        if (TimeRemaining <= 3f && !isPlayingFinalSecondsWarning)
        {
            isPlayingFinalSecondsWarning = true;
            AudioManager.Instance.PlaySFXFinalSeconds();
        }
        else if (TimeRemaining > 3f && isPlayingFinalSecondsWarning)
        {
            isPlayingFinalSecondsWarning = false;
            AudioManager.Instance.StopSFXFinalSeconds();
        }

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame();
        }
    }

    // Los Targets llaman esto cuando son destruidos
    public void RegisterTargetDestroyed()
    {
        if (CurrentState != GameState.Playing) return;

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
            OnStateChanged?.Invoke(CurrentState);
            Time.timeScale = 0f;
        }
        else if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            OnStateChanged?.Invoke(CurrentState);
            Time.timeScale = 1f;
        }

        AudioManager.Instance.PlaySFXClickButton();
    }

    private void EndGame()
    {
        if(TargetsDestroyed > HighScore)
        {
            HighScore = TargetsDestroyed;
            PlayerPrefs.SetInt("HighScore", TargetsDestroyed);
        }
        
        PlayerPrefs.Save();

        CurrentState = GameState.GameOver;
        OnStateChanged?.Invoke(CurrentState);
        OnGameOver?.Invoke();
    }

    public void ReturnToTitle()
    {
        CurrentState = GameState.Title;
        OnStateChanged?.Invoke(CurrentState);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        isReloading = true;
        shouldAutoStart = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Scene");
    }
}