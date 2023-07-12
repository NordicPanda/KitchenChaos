using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSound : MonoBehaviour
{
    [SerializeField] Stove stove;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stove.OnStateChange += Stove_OnStateChange;
    }

    private void Stove_OnStateChange(object sender, Stove.StateChangeEventArgs e)
    {
        if (e.state == Stove.State.Cooking || e.state == Stove.State.Fried)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
