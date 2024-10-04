using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> Menus;
    TextMeshProUGUI sliderValue;
    [NonSerialized] public bool HUDEnabled;
    void Start()
    {
        sliderValue = Menus[1].GetComponentInChildren<Slider>().gameObject.GetComponentInChildren<TextMeshProUGUI>();
        Init();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ActivateMenuDeactivateAllOthers(int menuIndex)
    {
        Menus.ForEach(menu => menu.SetActive(false));
        Menus[menuIndex].SetActive(true);
    }

    public void SwitchToMain()
    {
        ActivateMenuDeactivateAllOthers(0);
    }

    public void SwitchToWeightSelect()
    {
        ActivateMenuDeactivateAllOthers(1);
        GameManager.instance.SwitchState(GameManager.gameStates.SelectWeight);
    }

    public void SwitchToHUD()
    {
        HUDEnabled = true;
        ActivateMenuDeactivateAllOthers(2);
    }

    public void ChangeWeight(float value)
    {
        sliderValue.text = $"{value} Kg";
    }

    public void PassSelectedBalltoGM()
    {
        int selected = int.Parse(sliderValue.text.Split(' ')[0]);
        GameManager.instance.GetSelectedBall(selected);
    }

    public void Init()
    {
        HUDEnabled = false;
        switch (GameManager.instance.GetState())
        {
            case GameManager.gameStates.Menu:
                SwitchToMain();
                break;
            case GameManager.gameStates.SelectWeight:
                SwitchToWeightSelect();
                break;
            case GameManager.gameStates.SelectPos:
                break;
            default:
                break;
        }
    }
}
