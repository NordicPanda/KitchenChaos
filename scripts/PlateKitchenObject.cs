using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectsList;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO ingredientSO;
    }

    private List<KitchenObjectSO> kitchenObjectSOList; // list of ingredients already added to plate

    private void Awake()
    {
        kitchenObjectSOList = new();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // if there's no such ingredient already on the plate and this ingredient can be added into final recipe
        if (!kitchenObjectSOList.Contains(kitchenObjectSO) && validKitchenObjectsList.Contains(kitchenObjectSO))
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { ingredientSO = kitchenObjectSO });
            return true;
        }
        return false;
    }

    public List<KitchenObjectSO> GetIngredientsSOList() => kitchenObjectSOList;
    
}
