using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("SFX Clips")]
    public AudioClip buttonClick;
    public AudioClip placement;
    public AudioClip strike;
    public AudioClip popup;

    private bool musicEnabled;
    private bool sfxEnabled;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded+= OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
        LoadSettings();
        PlayMusicForScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        PlayMusicForScene();
    }

    // =========================
    // MUSIC
    // =========================

    void PlayMusicForScene()
    {

        Debug.Log("Playing Menu Music");

        if (!musicEnabled)
            return;

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            PlayMusic(gameMusic);
        }
        else
        {
            PlayMusic(menuMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // =========================
    // SFX
    // =========================

    public void PlayButton()
    {
        PlaySFX(buttonClick);
    }

    public void PlayPlacement()
    {
        PlaySFX(placement);
    }

    public void PlayStrike()
    {
        PlaySFX(strike);
    }

    public void PlayPopup()
    {
        PlaySFX(popup);
    }

    void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled)
            return;

        sfxSource.PlayOneShot(clip);
    }

    // =========================
    // SETTINGS
    // =========================

    public void SetMusic(bool enabled)
    {
        musicEnabled = enabled;

        PlayerPrefs.SetInt(
            "MusicEnabled",
            enabled ? 1 : 0);

        if (enabled)
            PlayMusicForScene();
        else
            musicSource.Stop();
    }

    public void SetSFX(bool enabled)
    {
        sfxEnabled = enabled;

        PlayerPrefs.SetInt(
            "SFXEnabled",
            enabled ? 1 : 0);
    }

    void LoadSettings()
    {
        // If settings don't exist yet → create defaults
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        if (!PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetInt("SFX", 1);
        }

        // Load saved settings
        musicEnabled =
            PlayerPrefs.GetInt("Music") == 1;

        sfxEnabled =
            PlayerPrefs.GetInt("SFX") == 1;
    }

    public bool IsMusicEnabled()
    {
        return musicEnabled;
    }

    public bool IsSFXEnabled()
    {
        return sfxEnabled;
    }
}