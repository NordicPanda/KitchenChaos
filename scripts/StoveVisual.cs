using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// idea: redo stove model so the lights on it turn on when cooking and off when done cooking
// i.e. green is on while the patty is still raw and red is on when it's cooked

public class StoveVisual : MonoBehaviour
{
    [SerializeField] GameObject stoveOnGlowVisual;
    [SerializeField] GameObject particlesVisual;
    [SerializeField] Light redLight;
    [SerializeField] Stove stove;

    private void Start()
    {
        stove.OnStateChange += Stove_OnStateChange;
        redLight.intensity = 0;
    }

    private void Stove_OnStateChange(object sender, Stove.StateChangeEventArgs e)
    {
        switch (e.state)
        {
            case Stove.State.Idle:
                stoveOnGlowVisual.SetActive(false);
                particlesVisual.SetActive(false);
                redLight.intensity = 0;
                break;

            case Stove.State.Cooking:
                stoveOnGlowVisual.SetActive(true);
                particlesVisual.SetActive(true);
                break;

            case Stove.State.Fried:
                stoveOnGlowVisual.SetActive(true);  // turning effects on also here because the player 
                particlesVisual.SetActive(true);    // can accidentally put already fried patty onto stove
                redLight.intensity = 2;   // stove is smart and flashes warning red light when the patty is cooked
                break;

            case Stove.State.Burned:
                particlesVisual.SetActive(false);
                break;
        }
    }
}
