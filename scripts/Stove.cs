using System;
using UnityEngine;

public class Stove : BaseCounter
{
    public event EventHandler<StateChangeEventArgs> OnStateChange;
    public class StateChangeEventArgs: EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Cooking,
        Fried,
        Burned
    }

    float timer;
    float timeToCook = 4;
    float timeToBurn = 6;
    State state;

    private void Start()
    {
        timer = 0;
        state = State.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                //disable particles and glow
                OnStateChange?.Invoke(this, new StateChangeEventArgs { state = state });
                break;

            case State.Cooking:
                // enable particles and glow
                OnStateChange?.Invoke(this, new StateChangeEventArgs { state = state });

                timer += Time.deltaTime;
                if (timer >= timeToCook)
                {
                    timer = 0;
                    KitchenObject rawObject = GetKitchenObject();
                    KitchenObjectSO cookedObjectSO = rawObject.GetKitchenObjectSO().friedKitchenObjectSO;
                    rawObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(cookedObjectSO, this);
                    state = State.Fried;
                }
                break;

            case State.Fried:
                // enable red light
                OnStateChange?.Invoke(this, new StateChangeEventArgs { state = state });

                timer += Time.deltaTime;
                if (timer >= timeToBurn)
                {
                    timer = 0;
                    KitchenObject cookedObject = GetKitchenObject();
                    KitchenObjectSO burnedObjectSO = cookedObject.GetKitchenObjectSO().friedKitchenObjectSO;
                    cookedObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(burnedObjectSO, this);
                    state = State.Burned;
                }
                break;

            case State.Burned:
                // disable particles
                OnStateChange?.Invoke(this, new StateChangeEventArgs { state = state });
                break;
        }
    }

    public override void Interact(Player player)
    {
        // player can place object (meat patty) on this counter,
        // then the object is fried (raw patty replaced with cooked) and player can take it back
        // if the player waits for too long, the patty will be burned

        if (!HasKitchenObject())                    // if nothing on counter
        {
            if (player.HasKitchenObject())          // and if player has something
            {                                       // => change carrier from player to stove
                KitchenObject kitchenObjectFromPlayer = player.GetKitchenObject();
                KitchenObjectSO rawObjectSO = kitchenObjectFromPlayer.GetKitchenObjectSO();
                if (rawObjectSO.canBeFried)         // and also if this object can be fried (this is meat patty)               
                {
                    kitchenObjectFromPlayer.SetCarrier(this);
                    SetKitchenObject(kitchenObjectFromPlayer);
                    player.ClearKitchenObject();
                    if (rawObjectSO.objectName == "Raw Meat Patty")
                    {
                        state = State.Cooking;
                        timer = 0; // reset timer when new patty is being fried
                                   // This is bad because if player takes raw patty and then
                                   // immediately puts it back, timer also will be reset.
                                   // Better to store frying time for each Patty object inside it,
                                   // but that requires reworking patty SO.
                    }
                    else if (rawObjectSO.objectName == "Fried Meat Patty")
                    {   // if the player accidentally tries to put already fried patty, it also should be possible 
                        state = State.Fried;
                    }   // this is bad because if we want to add more types of meat, we have to manually enter all names here
                }
            }
        }
        else                                        // if there's object on counter
        {
            if (!player.HasKitchenObject())         // and if player has nothing
            {
                KitchenObjectSO wholeObjectSO = GetKitchenObject().GetKitchenObjectSO();
                GetKitchenObject().SetCarrier(player);   // => give object to player
                ClearKitchenObject();
                state = State.Idle;
            }
            else
            {
                if (TryPutIngredientOnPlate()) // check for plate and if true, move object there and turn off stove
                {
                    state = State.Idle;
                }                
            }
        }
    }

    public void Initialize()
    {
        state = State.Idle;
    }
}
