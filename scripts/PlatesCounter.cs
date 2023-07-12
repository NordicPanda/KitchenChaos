using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateSO;
    public event EventHandler OnPlateVisualSpawned;
    public event EventHandler OnPlateTaken;

    private float plateSpawnTimer;
    private float plateSpawnTime = 5f;
    private int numberOfPlates = 0;
    private int maxNumberOfPlates = 4;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;

        if (plateSpawnTimer > plateSpawnTime)
        {
            plateSpawnTimer = 0f; // reset timer even if maximum number of plates reached to avoid it ticking infinitely
            if (numberOfPlates < maxNumberOfPlates)
            {
                numberOfPlates++; // this is only to change counter visual, actual plate is spawned on Interact
                OnPlateVisualSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (numberOfPlates >= 1 && !player.HasKitchenObject()) // if there are plates on counter and player has nothing
        {
            // if numberOfPlates >= 1, actually spawn plate object and give it to player
            KitchenObject.SpawnKitchenObject(plateSO, player);
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
            numberOfPlates--;
            // modify visual
        }
    }
}
