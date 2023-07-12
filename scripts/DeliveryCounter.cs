using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [SerializeField] DeliveryManager deliveryManager;

    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
            {
                deliveryManager.DeliverOrder(plate);
                // remove plate even if cooked meal isn't in any order
                // in this case Delivery Manager will increase wrong orders counter
                plate.DestroySelf();
            }
        }
    }
}
