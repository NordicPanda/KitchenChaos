using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    } 
    
    [SerializeField] private PlateKitchenObject plate;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        plate.OnIngredientAdded += Plate_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject item in kitchenObjectSOGameObjectList)
        { // make sure everything is disabled on start
            item.gameObject.SetActive(false);
        }
    }

    private void Plate_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject item in kitchenObjectSOGameObjectList)
        {
            if (item.kitchenObjectSO == e.ingredientSO)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
