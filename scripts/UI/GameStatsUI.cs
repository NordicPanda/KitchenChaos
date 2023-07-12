using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    public static GameStatsUI Instance { get; private set; }

    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI orders;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        Initialize();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, RecipeSO e)
    {
        score.text = DeliveryManager.Instance.GetScore().ToString();
        orders.text = DeliveryManager.Instance.GetOrdersDone().ToString();
    }

    public void Initialize()
    {
        score.text = "0";
        orders.text = "0";
    }
}
