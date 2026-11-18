using TMPro;
using UnityEngine;
using UnityEngine.UI;
using frare;
using UnityEngine.SceneManagement;

public class MainCanvas : Singleton<MainCanvas>
{
    protected override bool DontDestroyWhenLoad => false;

    [Header("HUD Elements")]
    [SerializeField] private Image nextItemImage;
    [SerializeField] private Button pauseButton;

    [Header("Game Over Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AnimatedCounter gameOverScoreCounter;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    [Header("References")]
    [SerializeField] private UISettingsPanel settingsPanel;

    protected override void Awake()
    {
        base.Awake();

        gameOverPanel.SetActive(false);

        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
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

    private void OnPauseButtonClicked()
    {
        settingsPanel.Open();
    }

    public void UpdateNextItem(Item item)
    {
        nextItemImage.sprite = item.SpriteGraphic;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverScoreCounter.UpdateCounterText(ScoreManager.Instance.CurrentScore);
    }
}