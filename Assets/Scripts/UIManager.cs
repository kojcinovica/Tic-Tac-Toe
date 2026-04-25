using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Popups")]
    public GameObject themePopup;
    public GameObject statsPopup;
    public GameObject settingsPopup;
    public GameObject creditsPopup;
    public GameObject exitPopup;

    [Header("Stats Text")]
    public TextMeshProUGUI totalGamesText;
    public TextMeshProUGUI player1WinsText;
    public TextMeshProUGUI player2WinsText;
    public TextMeshProUGUI drawsText;
    public TextMeshProUGUI avgDurationText;

    //[Header("Settings Toggles")]
    //public Toggle musicToggle;
    //public Toggle sfxToggle;

    private const string GAME_SCENE = "GameScene";

    void Start()
    {
        LoadSettings();
        LoadStats();
        //musicToggle.isOn =
        //AudioManager.Instance.IsMusicEnabled();

        //sfxToggle.isOn =
           // AudioManager.Instance.IsSFXEnabled();
    }

    // MAIN BUTTONS

    public void SelectTheme(int index)
    {
        ThemeManager.Instance.SetTheme(index);
    }

    public void OnPlayClicked()
    {
        AudioManager.Instance.PlayButton();

        CloseAllPopups();

        themePopup.SetActive(true);

        AudioManager.Instance.PlayPopup();
    }

    public void OnStatsClicked()
    {
        AudioManager.Instance.PlayButton();
        CloseAllPopups();
        LoadStats();
        statsPopup.SetActive(true);
        AudioManager.Instance.PlayPopup();
    }

    public void OnCreditsClicked()
    {
        AudioManager.Instance.PlayButton();
        CloseAllPopups();
        creditsPopup.SetActive(true);
        AudioManager.Instance.PlayPopup();
    }

    public void OnSettingsClicked()
    {
        AudioManager.Instance.PlayButton();

        CloseAllPopups();

        settingsPopup.SetActive(true);

        AudioManager.Instance.PlayPopup();
    }

    public void OnExitClicked()
    {
        AudioManager.Instance.PlayButton();

        CloseAllPopups();

        exitPopup.SetActive(true);

        AudioManager.Instance.PlayPopup();
    }

    // POPUP BUTTONS

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void OnClosePopup(GameObject popup)
    {
        AudioManager.Instance.PlayButton();

        popup.SetActive(false);
    }

    public void OnExitConfirmYes()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnExitConfirmNo()
    {
        exitPopup.SetActive(false);
    }

    // SETTINGS

    public void OnMusicToggleChanged(bool isOn)
    {
        AudioManager.Instance.SetMusic(isOn);
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        AudioManager.Instance.SetSFX(isOn);
    }

    void LoadSettings()
    {
        //musicToggle.isOn =
        //    PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        //sfxToggle.isOn =
        //    PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
    }

    // STATS

    void LoadStats()
    {
        int totalGames =
            PlayerPrefs.GetInt("TotalGames", 0);

        int p1Wins =
            PlayerPrefs.GetInt("Player1Wins", 0);

        int p2Wins =
            PlayerPrefs.GetInt("Player2Wins", 0);

        int draws =
            PlayerPrefs.GetInt("Draws", 0);

        float totalDuration =
            PlayerPrefs.GetFloat("TotalDuration", 0f);

        float avgDuration =
            totalGames > 0
            ? totalDuration / totalGames
            : 0f;

        totalGamesText.text =
            "Total Games: " + totalGames;

        player1WinsText.text =
            "Player 1 Wins: " + p1Wins;

        player2WinsText.text =
            "Player 2 Wins: " + p2Wins;

        drawsText.text =
            "Draws: " + draws;

        avgDurationText.text =
            "Avg Duration: "
            + avgDuration.ToString("F1")
            + " sec";
    }

    void CloseAllPopups()
    {
        themePopup.SetActive(false);
        statsPopup.SetActive(false);
        settingsPopup.SetActive(false);
        exitPopup.SetActive(false);
    }
}