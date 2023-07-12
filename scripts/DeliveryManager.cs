using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    private GameManager gameManager;

    [SerializeField] RecipeListSO recipeListSO;  // field to link special SO that contains all possible recipes
    private List<RecipeSO> waitingOrdersSOList;  // list of pending orders, items are added randomly during game
    private List<RecipeSO> recipesList;          // list of all possible orders

    public event EventHandler<RecipeSO> OnOrderAdded;       // for visuals and for sound
    public event EventHandler<RecipeSO> OnOrderCompleted;
    public event EventHandler OnOrderFailed;                // only for sound

    private float orderSpawnTimer;
    private float orderSpawnMaxTime = 5f;
    private int ordersDone = 0;     // total amount of orders
    private int score = 0;          // different orders have different score value
    private int maxOrders = 4;

    private void Awake()
    {
        Instance = this;
        waitingOrdersSOList = new();
        recipesList = recipeListSO.recipeSOList;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (gameManager.isGamePlaying())
        {
            // Generate orders
            orderSpawnTimer += Time.deltaTime;
            if (orderSpawnTimer >= orderSpawnMaxTime)
            {
                orderSpawnTimer = 0f;
                if (waitingOrdersSOList.Count < maxOrders)
                {
                    // get random recipe from the list
                    RecipeSO recipeSO = recipesList[UnityEngine.Random.Range(0, recipesList.Count)];
                    waitingOrdersSOList.Add(recipeSO);
                    OnOrderAdded?.Invoke(this, recipeSO);
                }
            }
        }
    }

    public void DeliverOrder(PlateKitchenObject plate)
        // compare things on plate from player with things in order
        // and if they are the same, order delivered successfully
    {
        if (OrderDone(out RecipeSO doneOrder, plate.GetIngredientsSOList()))
        {
            ordersDone++;
            score += doneOrder.scorePoints;
            waitingOrdersSOList.Remove(doneOrder);
            OnOrderCompleted?.Invoke(this, doneOrder);
        }
        else
        {
            OnOrderFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool OrderDone(out RecipeSO doneOrder, List<KitchenObjectSO> thingsOnPlate)
    {
        foreach (RecipeSO order in waitingOrdersSOList)
        {
            if (RecipeMatchesOrder(thingsOnPlate, order.kitchenObjectSOList))
            {
                doneOrder = order;
                return true;
            }
        }
        doneOrder = null;
        return false;
    }

    private bool RecipeMatchesOrder(List<KitchenObjectSO> thingsOnPlate, List<KitchenObjectSO> thingsInOrder)
        // compare plate with ONE order from list of all waiting orders
    {
        if (thingsInOrder.Count != thingsOnPlate.Count)
        {
            return false;
        }

        int matches = 0;
        foreach (KitchenObjectSO thing in thingsOnPlate)
        {
            if (thingsInOrder.Contains(thing))
            {
                matches++;
            }
        }
        if (matches == thingsInOrder.Count)
        { 
            return true;
        }
        return false;
    }

    public void Initialize()
    {
        ordersDone = 0;
        score = 0;
        waitingOrdersSOList.Clear();
    }

    public List<RecipeSO> GetWaitingOrdersSOList() => waitingOrdersSOList;
    public int GetOrdersDone() => ordersDone;
    public int GetScore() => score;
}
