using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private UISettingsPanel settingsPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnSettingsButtonClicked()
    {
        settingsPanel.Open();
    }
}