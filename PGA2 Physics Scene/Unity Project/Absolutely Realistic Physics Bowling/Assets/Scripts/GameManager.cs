using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public GameObject BallPositions;
    [SerializeField] PinManager pinManager;
    [NonSerialized] public bool firstTurn = false;
    bool hasResed = false;
    public enum gameStates
    {
        Menu,
        SelectWeight,
        SelectPos,
        SelectDir,
        SelectPower,
        Shooting,
        Gutter,
        Miss,
        Hit,
        Spare,
        Strike,
        AITurn,
        GameOver
    }

    [SerializeField] List<GameObject> balls;
    Vector3 ballSpawnPos;

    GameObject selectedBall;
    gameStates state = gameStates.Menu;
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!hasResed)
        {
            switch (state)
            {
                case gameStates.Gutter:
                case gameStates.Miss:
                case gameStates.Hit:
                    hasResed = true;
                    if (firstTurn)
                    {
                        SoftResScene();
                    }
                    else
                    {
                        HardResScene();
                    }
                    break;
                case gameStates.Strike:
                case gameStates.Spare:
                    hasResed = true;
                    HardResScene();
                    break;
            }
        }
    }

    public void SwitchState(gameStates value)
    {
        state = value;
    }

    public void GetSelectedBall(int value)
    {
        switch (value)
        {
            case 8:
                selectedBall = balls[0];
                break;
            case 9:
                selectedBall = balls[1];
                break;
            case 10:
                selectedBall = balls[2];
                break;
            case 11:
                selectedBall = balls[3];
                break;
            case 12:
                selectedBall = balls[4];
                break;
            default:
                Debug.LogError($"Ball of value {value} not found!");
                break;
        }
        InstantiateBall();
    }

    void InstantiateBall()
    {
        selectedBall = Instantiate(selectedBall, ballSpawnPos, Quaternion.identity);
        SwitchState(gameStates.SelectPos);
    }

    public gameStates GetState()
    {
        return state;
    }

    public void InstantiatePins()
    {
        pinManager.InstantiatePins();
    }
    public void SoftResScene()
    {
        StartCoroutine("SoftResIENUM");

    }

    public void HardResScene()
    {
        StartCoroutine("HardResIENUM");
    }

    IEnumerator SoftResIENUM()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponentInChildren<HUDManager>().Init();

        SwitchState(gameStates.SelectWeight);
        GetComponentInChildren<UIManager>().Init();
        Init();

        pinManager.Init();

        SceneManager.LoadScene("SampleScene");
    }
    IEnumerator HardResIENUM()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponentInChildren<HUDManager>().Init();

        SwitchState(gameStates.Menu);
        GetComponentInChildren<UIManager>().Init();
        Init();

        pinManager.HardResetAndInit();

        SceneManager.LoadScene("SampleScene");
    }

    void Init()
    {
        firstTurn = !firstTurn;
        hasResed = false;
        ballSpawnPos = new Vector3(-40, 4, 0);
        selectedBall = null;
    }
}
