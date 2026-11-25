using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private const string TutorialSeenKey = "TutorialSeen";

    [SerializeField, TextArea(3, 10)] private string mouseTutorialText;
    [SerializeField, TextArea(3, 10)] private string touchTutorialText;

    [SerializeField] private GameObject rootPanel;
    [SerializeField] private TMP_Text inputTutorialTmp;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(TutorialSeenKey, 0) == 0) 
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        UpdateInputText();
        rootPanel.SetActive(true);
        GameManager.Instance.SetPaused(true);
    }

    public void ClosePanel()
    {
        PlayerPrefs.SetInt(TutorialSeenKey, 1);
        PlayerPrefs.Save();

        rootPanel.SetActive(false);
        GameManager.Instance.SetPaused(false);
    }

    private void UpdateInputText()
    {
        var isInputTouch = Touchscreen.current != null;
        inputTutorialTmp.text = isInputTouch ? touchTutorialText : mouseTutorialText;
    }
}
