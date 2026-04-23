using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Board")]
    public Button[] cells;

    [Header("HUD")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI player1MovesText;
    public TextMeshProUGUI player2MovesText;

    [Header("Game Over")]
    public GameObject gameOverPopup;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI durationText;

    [Header("Settings")]
    public GameObject settingsPopup;

    private string currentPlayer = "X";

    private string[] board =
        new string[9];

    private int player1Moves = 0;
    private int player2Moves = 0;

    private float timer = 0f;

    private bool gameEnded = false;

    void Start()
    {
        InitializeBoard();
    }

    void Update()
    {
        if (gameEnded)
            return;

        timer += Time.deltaTime;

        timerText.text =
            "Time: "
            + timer.ToString("F1")
            + "s";
    }

    void InitializeBoard()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            int index = i;

            cells[i].onClick.AddListener(
                () => OnCellClicked(index)
            );

            board[i] = "";
            cells[i]
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = "";
        }

        gameOverPopup.SetActive(false);
        settingsPopup.SetActive(false);
    }

    // =========================
    // CELL CLICK
    // =========================

    void OnCellClicked(int index)
    {
        if (gameEnded)
            return;

        if (board[index] != "")
            return;

        board[index] = currentPlayer;

        cells[index]
            .GetComponentInChildren<TextMeshProUGUI>()
            .text = currentPlayer;

        if (currentPlayer == "X")
        {
            player1Moves++;
            player1MovesText.text =
                "P1 Moves: " + player1Moves;

            currentPlayer = "O";
        }
        else
        {
            player2Moves++;
            player2MovesText.text =
                "P2 Moves: " + player2Moves;

            currentPlayer = "X";
        }

        CheckWinner();
    }

    // =========================
    // WIN CHECK
    // =========================

    void CheckWinner()
    {
        int[,] winPatterns =
        {
            {0,1,2},
            {3,4,5},
            {6,7,8},

            {0,3,6},
            {1,4,7},
            {2,5,8},

            {0,4,8},
            {2,4,6}
        };

        for (int i = 0; i < 8; i++)
        {
            string a =
                board[winPatterns[i, 0]];

            string b =
                board[winPatterns[i, 1]];

            string c =
                board[winPatterns[i, 2]];

            if (a != "" && a == b && b == c)
            {
                EndGame(a + " Wins");
                return;
            }
        }

        bool draw = true;

        for (int i = 0; i < 9; i++)
        {
            if (board[i] == "")
                draw = false;
        }

        if (draw)
        {
            EndGame("Draw");
        }
    }

    // =========================
    // GAME END
    // =========================

    void EndGame(string result)
    {
        gameEnded = true;

        resultText.text = result;

        durationText.text =
            "Duration: "
            + timer.ToString("F1")
            + "s";

        gameOverPopup.SetActive(true);

        SaveStats(result);
    }

    // =========================
    // BUTTONS
    // =========================

    public void RetryGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        settingsPopup.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPopup.SetActive(false);
    }

    // =========================
    // SAVE STATS
    // =========================

    void SaveStats(string result)
    {
        int totalGames =
            PlayerPrefs.GetInt("TotalGames", 0);

        PlayerPrefs.SetInt(
            "TotalGames",
            totalGames + 1
        );

        if (result == "X Wins")
        {
            int wins =
                PlayerPrefs.GetInt(
                    "Player1Wins", 0);

            PlayerPrefs.SetInt(
                "Player1Wins",
                wins + 1
            );
        }
        else if (result == "O Wins")
        {
            int wins =
                PlayerPrefs.GetInt(
                    "Player2Wins", 0);

            PlayerPrefs.SetInt(
                "Player2Wins",
                wins + 1
            );
        }
        else
        {
            int draws =
                PlayerPrefs.GetInt(
                    "Draws", 0);

            PlayerPrefs.SetInt(
                "Draws",
                draws + 1
            );
        }

        float totalDuration =
            PlayerPrefs.GetFloat(
                "TotalDuration", 0f);

        PlayerPrefs.SetFloat(
            "TotalDuration",
            totalDuration + timer
        );

        PlayerPrefs.Save();
    }
}