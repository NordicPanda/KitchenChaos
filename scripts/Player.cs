using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform objectCarryPoint;


    public event EventHandler OnPickup;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter _selectedCounter;
    }

    private Vector3 startingPosition = new(3, 0, -3);
    private Quaternion startingRotation = Quaternion.Euler(new Vector3(0, 180, 0));
    private float walkSpeed = 9;
    private float rotateSpeed = 20;
    private float playerRadius = 0.7f;
    private float playerHeight = 2;
    private float interactionRange = 2;
    private Vector3 lastInteractDirection;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private GameManager gameManager;

    private void Awake()
    {
        if (Instance != null)  // should never happen normally
        {
            Debug.LogError("There's more than one Player!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        HandleMovement();
        if (gameManager.isGamePlaying())
        {
            HandleInteractions();
        }
    }

    private Vector3 GetDirection()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new(inputVector.x, 0, inputVector.y);
        return moveDir;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void HandleInteractions()
    {
        Vector3 moveDir = GetDirection();

        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactionRange))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                    transform.forward = lastInteractDirection;
                }
            }
            else
            {
                if (selectedCounter != null)
                {
                    SetSelectedCounter(null);
                }
            }
        }
        else
        {
            if (selectedCounter != null)
            {
                SetSelectedCounter(null);
            }
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDir = GetDirection();

        float moveDistance = walkSpeed * Time.deltaTime; // make motion independent of framerate

        Vector3 origin = transform.position;  // player's current position; just to make next call a bit shorter
        bool canMove = !Physics.CapsuleCast
            (origin, origin + Vector3.up * playerHeight, // bottom and top points of collider
            playerRadius, moveDir, moveDistance);        // horizontal size of collider, move direction and distance

        if (!canMove)
        {
            // try to move along X
            Vector3 moveX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(origin, origin + Vector3.up * playerHeight, playerRadius, moveX, moveDistance);
            if (canMove)
            {
                moveDir = moveX;
            }
            else
            {
                // try to move along Z
                Vector3 moveZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(origin, origin + Vector3.up * playerHeight, playerRadius, moveZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;   // true if motion vector is not (0, 0, 0)

        Vector3 rotationVector = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        if (rotationVector != Vector3.zero) { transform.forward = rotationVector; }
    }

    public bool IsWalking
    {
        get => isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, 
                                         new OnSelectedCounterChangedEventArgs
                                         { _selectedCounter = selectedCounter });
    }

    public Transform GetCarryPoint() => objectCarryPoint;  // kitchenObjectHoldPoint in CodeMonkey's tutorial
    public bool HasKitchenObject() => kitchenObject != null;
    public KitchenObject GetKitchenObject() => kitchenObject;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnPickup?.Invoke(this, EventArgs.Empty);
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public void Initialize()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }

}
