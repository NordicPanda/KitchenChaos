using System;

public class TrashBin : BaseCounter
{
    public static event EventHandler OnRecycle;

    new public static void ResetStaticData()
    {
        OnRecycle = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnRecycle?.Invoke(this, EventArgs.Empty);
        }
    }
}
