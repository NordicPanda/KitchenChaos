using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform platePrefabVisual;

    private List<Transform> plateVisualGameObjectsList;

    private void Awake()
    {
        plateVisualGameObjectsList = new();
    }

    private void Start()
    {
        platesCounter.OnPlateVisualSpawned += PlatesCounter_OnPlateVisualSpawned;
        platesCounter.OnPlateTaken += PlatesCounter_OnPlateTaken;
    }

    private void PlatesCounter_OnPlateTaken(object sender, System.EventArgs e)
    {
        Transform plate = plateVisualGameObjectsList[plateVisualGameObjectsList.Count - 1];
        plateVisualGameObjectsList.Remove(plate);
        Destroy(plate.gameObject);
    }
        
    private void PlatesCounter_OnPlateVisualSpawned(object sender, System.EventArgs e)
    {   
        Transform plateVisualTransform = Instantiate(platePrefabVisual, counterTopPoint);
        float plateSpawnHeight = (plateVisualGameObjectsList.Count) * 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateSpawnHeight, 0);
        plateVisualGameObjectsList.Add(plateVisualTransform);
    }
}
