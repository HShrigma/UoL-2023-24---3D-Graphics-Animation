using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI GOScoreText;
    [SerializeField] TextMeshProUGUI GOHighScoreText;
    [SerializeField] TextMeshProUGUI WaveText;
    [SerializeField] List<GameObject> Menus;
    void Start()
    {
        ChangeScoreText();
        ChangeWaveText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGOScoreText()
    {
        GOScoreText.text = $"Current Score: {GameManager.instance.Score}";
    }
    public void ChangeGOHighScoreText()
    {
        int hs = PlayerPrefsManager.GetHighScore();
        GOHighScoreText.text = $"High Score: {hs}";
    }
    public void ChangeScoreText()
    {
        scoreText.text = $"Score: {GameManager.instance.Score}";
    }
    public void ChangeWaveText()
    {
        StartCoroutine(FadeWaveTXTAlphaForTime());
    }
    IEnumerator FadeWaveTXTAlphaForTime()
    {
        float seconds = 1.2f;        
        WaveText.CrossFadeAlpha(1f, seconds, true);
        WaveText.text = $"Wave {GameManager.instance.Wave}";
        yield return new WaitForSeconds(seconds);
        WaveText.CrossFadeAlpha(0f, seconds, true);
    }

    public void SelectMenu(string menuName)
    {
        foreach (var menu in Menus)
        {
            menu.SetActive(false);
        }
        switch (menuName)
        {
            case "HUD":
                Menus[0].SetActive(true);
                break;
            case "GameOver":
                Menus[1].SetActive(true);
                break;
            case "Pause":
                Menus[2].SetActive(true);
                break;
        }
    }
}
