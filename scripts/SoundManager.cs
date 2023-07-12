using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource soundCheckAudioSource;
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private AudioClipsRefsSO audioClips;
    private Vector3 deliveryCounterPosition;
    private Vector3 playerPosition;
    private float volume;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 0.6f);
    }

    private void Start()
    {
        deliveryCounterPosition = DeliveryCounter.Instance.transform.position;
        playerPosition = Player.Instance.transform.position;
        deliveryManager.OnOrderAdded += DeliveryManager_OnOrderAdded;
        deliveryManager.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        deliveryManager.OnOrderFailed += DeliveryManager_OnOrderFailed;
        CutterCounter.OnAnyCut += CutterCounter_OnAnyCut;
        Player.Instance.OnPickup += Player_OnPickup;
        PlayerSounds.OnMadeStep += PlayerSounds_OnMadeStep;
        BaseCounter.OnItemDrop += BaseCounter_OnItemDrop;
        TrashBin.OnRecycle += TrashBin_OnRecycle;
    }

    private void PlayerSounds_OnMadeStep(object sender, EventArgs e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.footsteps.Count());
        PlaySound(audioClips.footsteps[clipNumber], playerPosition);
    }

    private void TrashBin_OnRecycle(object sender, EventArgs e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.trash.Count());
        PlaySound(audioClips.trash[clipNumber], playerPosition);
    }

    private void BaseCounter_OnItemDrop(object sender, EventArgs e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.objectDrop.Count());
        PlaySound(audioClips.objectDrop[clipNumber], playerPosition);
    }

    private void Player_OnPickup(object sender, EventArgs e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.objectPickup.Count());
        PlaySound(audioClips.objectPickup[clipNumber], playerPosition);
    }

    private void CutterCounter_OnAnyCut(object sender, EventArgs e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.chop.Count());
        PlaySound(audioClips.chop[clipNumber], playerPosition);
    }

    private void DeliveryManager_OnOrderAdded(object sender, RecipeSO e)
    {
        PlaySound(audioClips.newOrder, deliveryCounterPosition);
    }

    private void DeliveryManager_OnOrderCompleted(object sender, RecipeSO e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.deliverySuccess.Count());
        PlaySound(audioClips.deliverySuccess[clipNumber], deliveryCounterPosition);
    }

    private void DeliveryManager_OnOrderFailed(object sender, EventArgs e)
    {
        int clipNumber = UnityEngine.Random.Range(0, audioClips.deliveryFail.Count());
        PlaySound(audioClips.deliveryFail[clipNumber], deliveryCounterPosition);
    }

    public void ChangeVolume(float newVolume)
    {
        volume = newVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
        soundCheckAudioSource.volume = volume;
        soundCheckAudioSource.Play();
    }

    private void PlaySound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public float GetVolume() => volume;
}
