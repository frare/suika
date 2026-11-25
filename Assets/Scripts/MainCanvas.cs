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

    [SerializeField] private Button tutorialButton;
    
    [Header("Game Over Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AnimatedCounter gameOverScoreCounter;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    [Header("References")]
    [SerializeField] private UISettingsPanel settingsPanel;
    [SerializeField] private TutorialManager tutorialManager;

    protected override void Awake()
    {
        base.Awake();

        gameOverPanel.SetActive(false);

        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        tutorialButton.onClick.AddListener(OnTutorialButtonClicked);
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

    private void OnTutorialButtonClicked()
    {
        tutorialManager.ShowPanel();
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