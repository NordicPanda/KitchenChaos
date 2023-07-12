using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] GameObject gamePausedUI;
    [SerializeField] Button resumeButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button optionsButton;
    OptionsUI optionsUI;

    private void Start()
    {
        optionsUI = OptionsUI.Instance;
        resumeButton.onClick.AddListener( () => { GameManager.Instance.TogglePause(); } );
        menuButton.onClick.AddListener(ExitToMenu);
        optionsButton.onClick.AddListener(ShowOptions);
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsGamePaused())
        {
            gamePausedUI.SetActive(true);
        }
        else
        {
            gamePausedUI.SetActive(false);
        }
    }

    private void ExitToMenu()
    {
        // unpause the game so it won't stay paused when starting new game from menu
        GameManager.Instance.TogglePause();
        Loader.LoadScene(Loader.Scene.MainMenu); 
    } 

    private void ShowOptions()
    {
        foreach (GameObject uiComponent in optionsUI.GetUIComponents())
        {
            uiComponent.SetActive(true);
        }
        optionsUI.isActive = true;
    }
}
