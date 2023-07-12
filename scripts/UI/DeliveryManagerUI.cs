using System.Collections.Generic;
using System.Net.Cache;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    public static DeliveryManagerUI Instance { get; private set; }

    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        Instance = this;
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        deliveryManager.OnOrderAdded += DeliveryManager_OnOrderAdded;
        deliveryManager.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderAdded(object sender, RecipeSO recipeSO)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, RecipeSO recipeSOe)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child != recipeTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        List<RecipeSO> waitingOrdersSOList = deliveryManager.GetWaitingOrdersSOList();
        foreach (RecipeSO recipe in waitingOrdersSOList)
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipe);
        }
    }

    public void Initialize()
    {
        UpdateVisual();
    }
}
