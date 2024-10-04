using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] GameObject botPanel;
    [SerializeField] Slider powerSlider;
    [SerializeField] Image sliderBG;
    [SerializeField] TextMeshProUGUI anouncements;
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (GameManager.instance.GetComponentInChildren<UIManager>().HUDEnabled)
        {
            switch (GameManager.instance.GetState())
            {
                case GameManager.gameStates.SelectPos:
                    botPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Set Position: Spacebar";
                    break;

                case GameManager.gameStates.SelectDir:
                    botPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Set Direction: Spacebar";
                    break;

                case GameManager.gameStates.SelectPower:
                    botPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Set Power: Spacebar";

                    if (!powerSlider.gameObject.activeSelf)
                    {
                        powerSlider.gameObject.SetActive(true);
                    }
                    break;
                case GameManager.gameStates.Shooting:
                    if (powerSlider.gameObject.activeSelf)
                    {
                        powerSlider.gameObject.SetActive(false);
                    }
                    botPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Forfeit Shot: R";
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        GameManager.instance.SwitchState(GameManager.gameStates.Miss);
                    }
                    break;
                case GameManager.gameStates.Gutter:
                case GameManager.gameStates.Miss:
                case GameManager.gameStates.Hit:
                case GameManager.gameStates.Strike:
                case GameManager.gameStates.Spare:
                    botPanel.GetComponent<Image>().CrossFadeAlpha(0f, .5f, true);
                    botPanel.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0f, .5f, true);
                    anouncements.text = GameManager.instance.GetState().ToString() + "!";
                    anouncements.CrossFadeAlpha(0f, .5f, true);
                    break;
            }
        }
    }

    public void SetPowerSlider(float value, float max, float mult)
    {
        Color color = new Color(value / 255, (255 - value) / 255, 0);
        powerSlider.maxValue = max;
        sliderBG.color = color;
        powerSlider.value = value;

        int textNum = Mathf.RoundToInt(value * mult);
        powerSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{textNum}";
    }

    public void Init()
    {
        anouncements.text = "";
        powerSlider.gameObject.SetActive(false);
    }
}
