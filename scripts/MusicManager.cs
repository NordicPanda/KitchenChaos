using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }

    private float volume;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.6f);
        GetComponent<AudioSource>().volume = volume;
    }

    public void ChangeVolume(float newVolume)
    {
        GetComponent<AudioSource>().volume = newVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public float GetVolume() => volume;

}
