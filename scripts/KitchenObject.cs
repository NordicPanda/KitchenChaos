using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent carrier; // this variable is named kitchenObjectParent in CodeMonkey's tutorial

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }    

    public void SetCarrier(IKitchenObjectParent carrier)    // Moving ingredient to another carrier
    {
        if (!carrier.HasKitchenObject())                    // if a carrier (counter or player) has an object
        {
            if (this.carrier != null)                       // if this ingredient has a carrier
            {
                this.carrier.ClearKitchenObject();          // remove this carrier's ingredient (THIS ingredient)
            }                                               // ( carrier.kitchenObject = null; )

            this.carrier = carrier;                         // set new carrier for this ingredient
            carrier.SetKitchenObject(this);                 // make reference to this ingredient for new carrier

            transform.parent = carrier.GetCarryPoint();     // change ingredient's parent object
            transform.localPosition = Vector3.zero;         // change its position relative to new parent
        }
    }

    public void DestroySelf()
    {
        carrier.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plate)
    {
        if (this is PlateKitchenObject)
        {
            plate = (PlateKitchenObject)this;
            return true;
        }
        plate = null;
        return false;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent carrier)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetCarrier(carrier);
        KitchenObjectSO thisObjectSO = kitchenObject.GetKitchenObjectSO();
        thisObjectSO.hitPointsLeft = thisObjectSO.hitPoints;
        return kitchenObject;
    }

    public BaseCounter GetClearCounter()
    {
        return (BaseCounter)carrier;
    }
}
