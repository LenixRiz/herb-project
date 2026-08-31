using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _gameTitle;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    [Header("Settings")]
    [SerializeField] private string _shopSceneName = "Scene_Shop";

    public enum MainMenuState
    {
        MainMenu,
        Settings,
        // To add: Save Menu, About
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
    }
    
    private void OnStartClicked()
    {
        SceneManager.LoadScene(_shopSceneName);
    }

    private void OnExitClicked()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
