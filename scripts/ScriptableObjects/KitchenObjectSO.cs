using UnityEngine;

[CreateAssetMenu()]

public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
    public KitchenObjectSO cutKitchenObjectSO;
    public int hitPoints;
    public int hitPointsLeft;
    public bool canBeFried;
    public KitchenObjectSO friedKitchenObjectSO; 
}
