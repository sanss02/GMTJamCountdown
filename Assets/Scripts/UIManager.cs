using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject gameOver;

    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI highScoreGOText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        GameManager.Instance.OnTimeChanged += HandleTimeChanged;
        GameManager.Instance.OnTargetDestroyed += HandleTargetDestroyed;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
        GameManager.Instance.OnTimeChanged -= HandleTimeChanged;
        GameManager.Instance.OnTargetDestroyed -= HandleTargetDestroyed;
    }


    private void HandleStateChanged(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Title:
                title.SetActive(true);
                hud.SetActive(false);
                pause.SetActive(false);
                gameOver.SetActive(false);

                highScoreText.text = $"High score: {GameManager.Instance.HighScore}";
                break;
            case GameManager.GameState.Playing:
                title.SetActive(false);
                hud.SetActive(true);
                pause.SetActive(false);
                gameOver.SetActive(false);

                HandleTimeChanged(GameManager.Instance.TimeRemaining);
                HandleTargetDestroyed(GameManager.Instance.TargetsDestroyed);
                break;
            case GameManager.GameState.Paused:
                title.SetActive(false);
                hud.SetActive(false);
                pause.SetActive(true);
                gameOver.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                title.SetActive(false);
                hud.SetActive(false);
                pause.SetActive(false);
                gameOver.SetActive(true);

                finalScoreText.text = $"Final score: {GameManager.Instance.TargetsDestroyed}";
                if (GameManager.Instance.TargetsDestroyed < GameManager.Instance.HighScore)
                {
                    highScoreGOText.text = $"High score: {GameManager.Instance.HighScore}";
                } else
                {
                    highScoreGOText.text = $"New record!\nHigh score: {GameManager.Instance.HighScore}";
                }
                break;
            default:
                Debug.Log("Game state not valid");
                break;
        }
    }

    private void HandleTimeChanged(float timeRemaining){
        int time = (int)timeRemaining;
        timerText.text = time.ToString();
    }

    private void HandleTargetDestroyed(int targetsDestroyed)
    {
        scoreText.text = $"Score: {targetsDestroyed}";
    }

    public void ShowHowToPlay()
    {
        title.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        title.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToTitle()
    {
        howToPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);
        title.SetActive(true);
    }


}
