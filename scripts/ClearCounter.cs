using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        // player can place object on this counter and take it back
        // if the counter is empty and player also doesn't have anything, nothing should happen
        if (!HasKitchenObject())                    // if nothing on counter
        {
            if (player.HasKitchenObject())          // and if player has something
            {                                       // => change carrier from player to counter
                KitchenObject kitchenObjectFromPlayer = player.GetKitchenObject();
                kitchenObjectFromPlayer.SetCarrier(this);
                SetKitchenObject(kitchenObjectFromPlayer);
                player.ClearKitchenObject();
            }
        }
        else                                        // if there's object on counter
        {
            if (!player.HasKitchenObject())         // and if player has nothing
            {
                GetKitchenObject().SetCarrier(player);   // => give object to player
                ClearKitchenObject();
            }
            else  // there's object on the counter and player also has something
            {
                // check if one of objects is a plate, and if the other object is valid for the final recipe
                // and if both conditions are true, put object on the plate
                TryPutIngredientOnPlate(); 
                // if the player's object isn't a plate (and there's something on counter), do nothing
            }
        }
        // 1) GetComponent returns GameObject from Transform
        // 2) GameObject has KitchenObject.cs script attached
        // 3) This script declares Serialize Field to attach SO to KitchenObject and method that returns that SO
        // 4) The SO has string field objectName
    }
}