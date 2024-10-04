using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static int GetHighScore()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        return PlayerPrefs.GetInt("HighScore");

    }

    public static void SetHighScore(int value)
    {
        int hs = GetHighScore();
        if (value > hs)
        {
            PlayerPrefs.SetInt("HighScore", value);
        }
    }
}
