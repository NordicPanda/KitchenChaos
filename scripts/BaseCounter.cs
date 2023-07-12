using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnItemDrop;

    public static void ResetStaticData()
    {
        OnItemDrop = null;
    }

    private KitchenObject kitchenObject;
    [SerializeField] private Transform counterTopPoint;

    public virtual void Interact(Player player) { }
    public virtual void InteractAlternate(Player player) { }

    public Transform GetCarryPoint() => counterTopPoint;
    public bool HasKitchenObject() => kitchenObject != null;
    public KitchenObject GetKitchenObject() => kitchenObject;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnItemDrop?.Invoke(this, EventArgs.Empty);
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool TryPutIngredientOnPlate()
    {
        // if player has something, check if it's a plate,
        // and if the object on counter is valid for the final recipe
        Player player = Player.Instance;
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateFromPlayer)) // if player has a plate
        {
            // check what's on counter and if that's a valid ingredient for the final recipe, give it to player
            KitchenObject objectOnCounter = GetKitchenObject();
            if (plateFromPlayer.TryAddIngredient(objectOnCounter.GetKitchenObjectSO()))
            {
                objectOnCounter.DestroySelf();
                return true;
            }
            return false;
        }
        else // player doesn't have a plate, check if counter has
        {
            if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plateOnCounter))
            {
                KitchenObject objectFromPlayer = player.GetKitchenObject();
                if (plateOnCounter.TryAddIngredient(objectFromPlayer.GetKitchenObjectSO()))
                {
                    objectFromPlayer.DestroySelf();
                    return true;
                }
            }
        }
        return false;
    }

}

