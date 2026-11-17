using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button creditsCloseButton;
    [SerializeField] private UISettingsPanel settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        creditsCloseButton.onClick.AddListener(OnCreditsCloseButtonClicked);

        creditsPanel.SetActive(false);
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnSettingsButtonClicked()
    {
        settingsPanel.Open();
    }

    private void OnCreditsButtonClicked()
    {
        creditsPanel.SetActive(true);
    }

    private void OnCreditsCloseButtonClicked()
    {
        creditsPanel.SetActive(false);
    }
}