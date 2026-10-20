using TMPro;
using UnityEngine;
using UnityEngine.UI;
using frare;

public class MainCanvas : Singleton<MainCanvas>
{
    protected override bool DontDestroyWhenLoad => false;

    [SerializeField] private TMP_Text scoreTmp;
    [SerializeField] private Image nextItemImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverScoreTmp;

    protected override void Awake()
    {
        base.Awake();

        gameOverPanel.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void UpdateScore(int score)
    {
        scoreTmp.text = $"Score: {score}";
    }

    public void UpdateNextItem(Item item)
    {
        nextItemImage.sprite = item.SpriteGraphic;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverScoreTmp.text = $"{GameManager.Instance.TotalScore}";
    }
}