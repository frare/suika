using UnityEngine;
using frare;

public class GameManager : Singleton<GameManager>
{
    protected override bool DontDestroyWhenLoad => false;

    public bool IsGameOver => isGameOver;
    public bool IsPaused => isPaused;

    private bool isGameOver;
    private bool isPaused;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void SetPaused(bool paused)
    {
        if (isGameOver) return; // do not let the game be 'resumed' if game is over

        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    public void GameOver()
    {
        isGameOver = true;
        SetPaused(true);

        MainCanvas.Instance.ShowGameOverPanel();
    }
}
