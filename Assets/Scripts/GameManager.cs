using UnityEngine;
using frare;

public class GameManager : Singleton<GameManager>
{
    protected override bool DontDestroyWhenLoad => false;

    public int TotalScore => totalScore;
    public bool IsGameOver => isGameOver;

    private int totalScore;
    private bool isGameOver;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        MainCanvas.Instance.UpdateScore(totalScore);
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        MainCanvas.Instance.ShowGameOverPanel();
    }
}
