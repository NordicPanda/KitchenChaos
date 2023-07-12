using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameClockUI : MonoBehaviour
{
    private Color regularColor = new(22 / 255f, 106 / 255f, 185 / 255f, 200 / 255f); // light blue
    private Color warningColor = new(1f, 0f, 0f, 200 / 255f); // red

    public static GameClockUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI gamePlayingClockText;
    [SerializeField] private Image timerVisual;
    private float timeLeft;
    private float timeMax;

    GameManager gameManager;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Initialize();    
    }

    void Update()
    {
        timeLeft = gameManager.GetGameOverTimer();
        gamePlayingClockText.text = Mathf.Ceil(timeLeft).ToString();
        timerVisual.fillAmount = timeLeft / timeMax;
        if (timeLeft <= 10f)
        {
            timerVisual.color = warningColor;
        }
    }

    public void Initialize()
    {
        gameManager = GameManager.Instance;
        timeMax = gameManager.GetGameOverTimerMax();
        timeLeft = timeMax;
        timerVisual.color = regularColor;
    }
}
