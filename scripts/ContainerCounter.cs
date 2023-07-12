using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerPickup;

    public override void Interact(Player player)
    {
        // player can spawn new object at this counter and take it, but can't put it back
        if (!player.HasKitchenObject())     // if player has nothing,
        {                                   // spawn new object and give it to player
            OnPlayerPickup?.Invoke(this, EventArgs.Empty);  // fire an event to launch animation playback
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);  // create new kitchen object
        }
    }
}
