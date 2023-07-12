using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownTimer;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (gameManager.isCountdown())
        {
            countdownTimer.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (gameManager.isCountdown())
        {
            countdownTimer.text = Mathf.Ceil(gameManager.GetCountdownTimer()).ToString();
        }
        else
        {
            countdownTimer.gameObject.SetActive(false);
        }
    }
}
