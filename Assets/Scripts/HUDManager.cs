using TMPro;
using UnityEngine;
using frare;

public class HUDManager : Singleton<HUDManager>
{
    protected override bool DontDestroyWhenLoad => false;

    [SerializeField] private TMP_Text scoreTmp;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void UpdateScore(int score)
    {
        scoreTmp.text = $"Score: {score}";
    }
}