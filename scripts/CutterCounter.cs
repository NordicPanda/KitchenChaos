using System;
using UnityEngine;

public class CutterCounter : BaseCounter
{
    public event EventHandler OnCut;            // for visuals (change progress bar on currently active counter)
    public static event EventHandler OnAnyCut;  // for sound (play sound when any counter is used)

    new public static void ResetStaticData()                                         
    {                                           // remove all listeners to avoid null reference exception
        OnAnyCut = null;                        // after exiting to main menu and starting new game   
    }                                           // (static fields aren't being destroyed on scene unloading)

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float hitPointsLeftNormalized;
    }

    public override void Interact(Player player)
    {
        // player can place object on this counter, cut it and take slices (another object) back
        if (!HasKitchenObject())                    // if nothing on counter
        {
            if (player.HasKitchenObject())          // and if player has something
            {                                       // => change carrier from player to counter
                KitchenObject kitchenObjectFromPlayer = player.GetKitchenObject();
                KitchenObjectSO wholeObjectSO = kitchenObjectFromPlayer.GetKitchenObjectSO();
                if (wholeObjectSO.hitPoints > 0)    // and also if this object can be cut
                {                                   // if it is 0, then player can't place this object here                    
                    kitchenObjectFromPlayer.SetCarrier(this);
                    SetKitchenObject(kitchenObjectFromPlayer);
                    player.ClearKitchenObject();

                    // invoke event to pass object's remaining hit points when it is placed onto the counter
                    // this is useful in case if player took away an object but have not finished cutting it,
                    // so when player places it back, its HP will be shown correctly by progress bar

                    OnProgressChanged?.Invoke
                        (
                            this,
                            new OnProgressChangedEventArgs
                            { hitPointsLeftNormalized = (float)wholeObjectSO.hitPointsLeft / wholeObjectSO.hitPoints }
                        );
                }
            }
        }
        else                                        // if there's object on counter
        {
            if (!player.HasKitchenObject())         // and if player has nothing
            {
                KitchenObjectSO wholeObjectSO = GetKitchenObject().GetKitchenObjectSO();

                // fire an event with 0 as parameter to hide progress bar
                OnProgressChanged?.Invoke
                    ( this, new OnProgressChangedEventArgs { hitPointsLeftNormalized = 0f } );

                GetKitchenObject().SetCarrier(player);   // => give object to player
                ClearKitchenObject();
            }
            else // if player has a plate
            {
                TryPutIngredientOnPlate();
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            KitchenObject wholeObject = GetKitchenObject();
            KitchenObjectSO wholeObjectSO = wholeObject.GetKitchenObjectSO();
            KitchenObjectSO cutObjectSO = wholeObjectSO.cutKitchenObjectSO;
            if (cutObjectSO != null)  // if this object can be cut
            {
                if (wholeObjectSO.hitPointsLeft > 0)
                { 
                    wholeObjectSO.hitPointsLeft--;
                    OnCut?.Invoke(this, EventArgs.Empty);
                    OnAnyCut?.Invoke(this, EventArgs.Empty);
                    
                    // fire an event to pass remaining HP after each cut and adjust progress bar
                    OnProgressChanged?.Invoke
                        (
                            this, 
                            new OnProgressChangedEventArgs 
                            { hitPointsLeftNormalized = (float)wholeObjectSO.hitPointsLeft / wholeObjectSO.hitPoints } 
                        );
                }
                if (wholeObjectSO.hitPointsLeft == 0)
                {
                    wholeObjectSO.hitPointsLeft = wholeObjectSO.hitPoints;
                    wholeObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(cutObjectSO, this);
                }
            }
        }
    }

    public void Initialize()
    {
        // hide progress bar
        OnProgressChanged?.Invoke (this, new OnProgressChangedEventArgs { hitPointsLeftNormalized = 0f });
    }
}
