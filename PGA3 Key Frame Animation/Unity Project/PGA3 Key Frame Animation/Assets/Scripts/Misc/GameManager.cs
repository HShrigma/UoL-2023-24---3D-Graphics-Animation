using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int Score { get; private set; }
    public int Wave { get; private set; }

    public UnityEvent ScoreChanged;
    public UnityEvent WaveChanged;
    public UnityEvent OnPause;
    public UnityEvent OnGameOver;

    bool paused = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 1f;
        Score = 0;
        Wave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Cancel") == 1)
        {
            PauseGame();
        } 
    }

    public void AddScore(int score)
    {
        Score += score;
        ScoreChanged.Invoke();
    }

    public void AddWave()
    {
        Wave++;
        WaveChanged.Invoke();
    }
    public void PauseGame()
    {
        if (!paused)
        {
            Time.timeScale = 0f;
            paused = true;
            OnPause.Invoke();
        }
    }

    public void UnpauseGame()
    {
        paused = false;
        Time.timeScale = 1f;
    }

    public void EndGame()
    {
        StartCoroutine(GameOverInSeconds(2f));
    }
    IEnumerator GameOverInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayerPrefsManager.SetHighScore(Score);
        Time.timeScale = 0f;
        OnGameOver.Invoke();
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Level");
    }
}