using UnityEngine;
using UnityEngine.UI;

public class ExitGamePopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject confirmationPanel; // Assign your popup panel here
    public Button yesButton;             // Assign the "Yes" button
    public Button noButton;              // Assign the "No" button

    void Start()
    {
        // Ensure popup is hidden at start
        confirmationPanel.SetActive(false);

        // Hook up button events
        yesButton.onClick.AddListener(OnConfirmExit);
        noButton.onClick.AddListener(OnCancelExit);
    }

    // Called by your Exit button in the Main Menu
    public void OnExitButtonClicked()
    {
        confirmationPanel.SetActive(true);
    }

    private void OnConfirmExit()
    {
        // Works in build; in editor it just stops play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnCancelExit()
    {
        confirmationPanel.SetActive(false);
    }
}
