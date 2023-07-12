using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float stepsTimer;
    private float stepsTimerMax = 0.2f;
    public static event EventHandler OnMadeStep;

    public static void ResetStaticData()
    {
        OnMadeStep = null;
    }



    private void Start()
    {
        player = Player.Instance;
    }

    private void Update()
    {
        stepsTimer += Time.deltaTime;
        if (stepsTimer > stepsTimerMax && player.IsWalking) 
        { 
            stepsTimer = 0;
            OnMadeStep?.Invoke(this, EventArgs.Empty);
        }
    }

}
