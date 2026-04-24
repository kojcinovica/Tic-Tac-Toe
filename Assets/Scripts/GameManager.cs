using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Board")]
    public Button[] cells;

    [Header("Strike Line")]
    public RectTransform strikeLine;

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

    private string[] board = new string[9];

    private int[] winningCells;

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

        AudioManager.Instance.PlayPlacement();


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
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (board[a] != "" &&
                board[a] == board[b] &&
                board[b] == board[c])
            {
                winningCells =
                    new int[] { a, b, c };

                AudioManager.Instance.PlayStrike();

                StartCoroutine(AnimateStrikeLine(a, b, c));

                StartCoroutine(
                    EndGameWithDelay(
                        board[a] + " Wins"
                    )
                );
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
            EndGame("Draw");
    }

    IEnumerator EndGameWithDelay(string result)
    {
        yield return new WaitForSeconds(0.4f);

        EndGame(result);
    }

    IEnumerator AnimateStrikeLine(int a, int b, int c)
    {
        strikeLine.gameObject.SetActive(true);

        Vector3 posA =
            cells[a].transform.position;

        Vector3 posC =
            cells[c].transform.position;

        // Start from FIRST cell
        strikeLine.position = posA;

        Vector3 direction =
            posC - posA;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x)
            * Mathf.Rad2Deg;

        strikeLine.rotation =
            Quaternion.Euler(0, 0, angle);

        float fullDistance =
            Vector3.Distance(posA, posC);

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float width =
                Mathf.Lerp(
                    0,
                    fullDistance,
                    time / duration
                );

            strikeLine.sizeDelta =
                new Vector2(width, 20f);

            yield return null;
        }

        strikeLine.sizeDelta =
            new Vector2(fullDistance, 20f);
    }

    void ShowStrikeLine(int a, int b, int c)
    {
        strikeLine.gameObject.SetActive(true);

        Vector3 posA =
            cells[a].transform.position;

        Vector3 posC =
            cells[c].transform.position;

        Vector3 center =
            (posA + posC) / 2f;

        strikeLine.position = center;

        float distance =
            Vector3.Distance(posA, posC);

        strikeLine.sizeDelta =
            new Vector2(distance, 20f);

        Vector3 direction =
            posC - posA;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x)
            * Mathf.Rad2Deg;

        strikeLine.rotation =
            Quaternion.Euler(
                0,
                0,
                angle
            );
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

        AudioManager.Instance.PlayPopup();


        gameOverPopup.SetActive(true);

        SaveStats(result);
    }

    // =========================
    // BUTTONS
    // =========================

    public void RetryGame()
    {
        AudioManager.Instance.PlayButton();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlayButton();

        SceneManager.LoadScene("PlayScene");
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlayButton();

        settingsPopup.SetActive(true);

        AudioManager.Instance.PlayPopup();
    }

    public void CloseSettings()
    {
        AudioManager.Instance.PlayButton();

        settingsPopup.SetActive(false);

        AudioManager.Instance.PlayPopup();

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