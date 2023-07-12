using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ordersDeliveredText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] List<GameObject> UIElementsList;
    [SerializeField] Button newGameButton;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        newGameButton.onClick.AddListener(() =>
        {
            foreach (GameObject uiElement in UIElementsList)
            {
                uiElement.SetActive(false);
            }
            gameManager.RestartGame();
        });
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (gameManager.isGameOver())
        {
            foreach (GameObject uiElement in UIElementsList)
            {
                uiElement.SetActive(true);
            }
            ordersDeliveredText.text = DeliveryManager.Instance.GetOrdersDone().ToString();
            scoreText.text = DeliveryManager.Instance.GetScore().ToString();
        }
    }

}
