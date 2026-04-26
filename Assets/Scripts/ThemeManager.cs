using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public XOTheme[] themes;

    public int selectedThemeIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            LoadTheme();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public XOTheme GetCurrentTheme()
    {
        return themes[selectedThemeIndex];
    }

    public void SetTheme(int index)
    {
        selectedThemeIndex = index;

        PlayerPrefs.SetInt("ThemeIndex",index);
    }

    void LoadTheme()
    {
        selectedThemeIndex =PlayerPrefs.GetInt("ThemeIndex",0);
    }
}