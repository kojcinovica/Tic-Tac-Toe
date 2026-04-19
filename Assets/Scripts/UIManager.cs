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
    public GameObject exitPopup;

    [Header("Stats Text")]
    public TextMeshProUGUI totalGamesText;
    public TextMeshProUGUI player1WinsText;
    public TextMeshProUGUI player2WinsText;
    public TextMeshProUGUI drawsText;
    public TextMeshProUGUI avgDurationText;

    [Header("Settings Toggles")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    private const string GAME_SCENE = "GameScene";

    void Start()
    {
        LoadSettings();
        LoadStats();
    }

    // MAIN BUTTONS

    public void OnPlayClicked()
    {
        CloseAllPopups();
        themePopup.SetActive(true);
    }

    public void OnStatsClicked()
    {
        CloseAllPopups();
        LoadStats();
        statsPopup.SetActive(true);
    }

    public void OnSettingsClicked()
    {
        CloseAllPopups();
        settingsPopup.SetActive(true);
    }

    public void OnExitClicked()
    {
        CloseAllPopups();
        exitPopup.SetActive(true);
    }

    // POPUP BUTTONS

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void OnClosePopup(GameObject popup)
    {
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
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
    }

    void LoadSettings()
    {
        musicToggle.isOn =
            PlayerPrefs.GetInt("MusicEnabled", 1) == 1;

        sfxToggle.isOn =
            PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
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