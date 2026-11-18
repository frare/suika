using frare;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    protected override bool DontDestroyWhenLoad => false;

    private const string BestScoreKey = "BestScore";

    public int CurrentScore => currentScore;
    public int BestScore => bestScore;

    [SerializeField] private AnimatedCounter currentScoreText;
    [SerializeField] private TMP_Text bestScoreText;

    private int currentScore = 0;
    private int bestScore = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);

        UpdateScoreUI();
        UpdateBestScoreUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        currentScoreText.UpdateCounterText(currentScore);

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
            UpdateBestScoreUI();
        }
    }

    private void UpdateBestScoreUI()
    {
        bestScoreText.text = $"Best: {bestScore}";
    }
}