using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] List<GameObject> uiComponents;  // groups of all visual elements of UI to deactivate them    
    [SerializeField] Slider sliderSoundVolume;
    [SerializeField] TextMeshProUGUI textSoundVolume;
    [SerializeField] Slider sliderMusicVolume;
    [SerializeField] TextMeshProUGUI textMusicVolume;
    [SerializeField] Button buttonBack;
    [SerializeField] GameObject textPressNewKey;

    // key binding buttons
    [SerializeField] Button buttonUp;
    [SerializeField] Button buttonDown;
    [SerializeField] Button buttonLeft;
    [SerializeField] Button buttonRight;
    [SerializeField] Button buttonInteract;
    [SerializeField] Button buttonAltInteract;
    [SerializeField] Button buttonPause;

    // text on buttons
    [SerializeField] TextMeshProUGUI textButtonUp;
    [SerializeField] TextMeshProUGUI textButtonDown;
    [SerializeField] TextMeshProUGUI textButtonLeft;
    [SerializeField] TextMeshProUGUI textButtonRight;
    [SerializeField] TextMeshProUGUI textButtonInteract;
    [SerializeField] TextMeshProUGUI textButtonAltInteract;
    [SerializeField] TextMeshProUGUI textButtonPause;

    public bool isActive { get; set; }

    public static OptionsUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        isActive = false;
    }

    private void Start()
    {
        // restore saved values
        float soundVolume = SoundManager.Instance.GetVolume() * 5;
        float musicVolume = MusicManager.Instance.GetVolume() * 5;

        sliderSoundVolume.value = soundVolume;
        textSoundVolume.text = soundVolume.ToString();

        sliderMusicVolume.value = musicVolume;
        textMusicVolume.text = musicVolume.ToString();

        buttonBack.onClick.AddListener(CloseOptions);
        sliderSoundVolume.onValueChanged.AddListener( (volume) => ChangeSoundVolume(volume) );
        sliderMusicVolume.onValueChanged.AddListener( (volume) => ChangeMusicVolume(volume) );

        buttonUp.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.Up) );
        buttonDown.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.Down) );
        buttonLeft.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.Left) );
        buttonRight.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.Right) );
        buttonInteract.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.Interact) );
        buttonAltInteract.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.AltInteract) );
        buttonPause.onClick.AddListener( () => GameInput.Instance.Rebind(GameInput.KeyBinding.Pause) );

        UpdateVisuals(); // get all key bindings and set text on binding buttons
    }

    private void ChangeSoundVolume(float volume)
    {
        textSoundVolume.text = volume.ToString();
        SoundManager.Instance.ChangeVolume(volume / sliderSoundVolume.maxValue);
        // slider has values 0 to 5 and volume is a float 0 to 1, so need to divide slider value by maxValue
    }

    private void ChangeMusicVolume(float volume)
    {
        textMusicVolume.text = volume.ToString();
        MusicManager.Instance.ChangeVolume(volume / sliderMusicVolume.maxValue);
    }

    private void CloseOptions()
    {
        foreach (GameObject uiComponent in uiComponents)
        {
            uiComponent.SetActive(false);
        }
        isActive = false;
    }

    private void UpdateVisuals()
    {
        textButtonInteract.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.Interact);
        textButtonAltInteract.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.AltInteract);
        textButtonPause.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.Pause);
        textButtonUp.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.Up);
        textButtonDown.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.Down);
        textButtonLeft.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.Left);
        textButtonRight.text = GameInput.Instance.GetBindingText(GameInput.KeyBinding.Right);

        // if binding is arrow key, show arrow symbol instead of text
        List<TextMeshProUGUI> buttonCaptions = new() { textButtonUp, textButtonDown, textButtonLeft, textButtonRight,
                                                       textButtonInteract, textButtonAltInteract, textButtonPause };
        for (int i = 0; i < buttonCaptions.Count; i++)
        {
            switch (buttonCaptions[i].text)
            {
                case "Up Arrow":
                    buttonCaptions[i].text = "↑";
                    break;
                case "Down Arrow":
                    buttonCaptions[i].text = "↓";
                    break;
                case "Left Arrow":
                    buttonCaptions[i].text = "←";
                    break;
                case "Right Arrow":
                    buttonCaptions[i].text = "→";
                    break;
            }
        }                               
    }

    public void EnableBindingVisual()
    {
        textPressNewKey.SetActive(true);
    }

    public void DisableBindingVisual()
    {
        textPressNewKey.SetActive(false);
        UpdateVisuals();
    }

    public List<GameObject> GetUIComponents() => uiComponents;
 }
