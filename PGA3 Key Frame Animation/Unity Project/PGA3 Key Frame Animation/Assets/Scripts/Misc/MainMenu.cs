using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    void Start()
    {
        Time.timeScale = 1.0f;
        int hs = PlayerPrefsManager.GetHighScore();
        highscoreText.text = $"High Score: {hs}";
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Level");
    }
}
