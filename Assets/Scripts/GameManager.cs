using UnityEngine;
using frare;

public class GameManager : Singleton<GameManager>
{
    protected override bool DontDestroyWhenLoad => false;

    private int totalScore;

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
        HUDManager.Instance.UpdateScore(totalScore);
    }
}
