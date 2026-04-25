using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Music")]
    public Image musicButtonImage;
    public Color musicOnColor = Color.green;
    public Color musicOffColor = Color.red;

    [Header("SFX")]
    public Image sfxButtonImage;
    public Color sfxOnColor = Color.green;
    public Color sfxOffColor = Color.red;

    void Start()
    {
    }

    void OnEnable()
    {
    }

    // =========================
    // MUSIC
    // =========================
    public void ToggleMusic()
    {
        bool current = AudioManager.Instance.IsMusicEnabled();
        AudioManager.Instance.SetMusic(!current);
    }

    // =========================
    // SFX
    // =========================
    public void ToggleSFX()
    {
        bool current = AudioManager.Instance.IsSFXEnabled();
        AudioManager.Instance.SetSFX(!current);

        // optional: play click to confirm (only if enabling)
        if (!current)
            AudioManager.Instance.PlayButton();

    }

    // =========================
    // UI UPDATE
    // =========================
   
}