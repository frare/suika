using TMPro;
using UnityEngine;
using UnityEngine.UI;
using frare;
using UnityEngine.SceneManagement;

public class MainCanvas : Singleton<MainCanvas>
{
    protected override bool DontDestroyWhenLoad => false;

    [Header("HUD Elements")]
    [SerializeField] private AnimatedCounter scoreCounter;
    [SerializeField] private Image nextItemImage;

    [Header("Game Over Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AnimatedCounter gameOverScoreCounter;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    protected override void Awake()
    {
        base.Awake();

        gameOverPanel.SetActive(false);
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void UpdateScore(int score)
    {
        scoreCounter.UpdateCounterText(score);
    }

    public void UpdateNextItem(Item item)
    {
        nextItemImage.sprite = item.SpriteGraphic;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverScoreCounter.UpdateCounterText(GameManager.Instance.TotalScore);
    }
}