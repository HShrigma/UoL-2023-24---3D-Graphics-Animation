using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    List<Vector3> PinSpawnPositions;
    List<GameObject> pins;
    List<bool> pushedPins;
    bool canSpare;
    [SerializeField] GameObject pin;
    void Start()
    {
        PinSpawnPositions = new List<Vector3>();
        float posX = -425f;
        //first row
        PinSpawnPositions.Add(new Vector3(posX, 5, 0));
        //second row
        PinSpawnPositions.Add(new Vector3(posX-3, 5, -2));
        PinSpawnPositions.Add(new Vector3(posX-3, 5, 2));
        //third row
        PinSpawnPositions.Add(new Vector3(posX-6, 5, 0));
        PinSpawnPositions.Add(new Vector3(posX-6, 5, -4));
        PinSpawnPositions.Add(new Vector3(posX-6, 5, 4));
        //fourth row
        PinSpawnPositions.Add(new Vector3(posX-9, 5, -2));
        PinSpawnPositions.Add(new Vector3(posX-9, 5, 2));
        PinSpawnPositions.Add(new Vector3(posX - 9, 5, 6));
        PinSpawnPositions.Add(new Vector3(posX - 9, 5, -6));


        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstantiatePins()
    {
        if (pushedPins.Any())
        {
            SpawnStandingPins();
        }
        else
        {
            foreach (Vector3 pos in PinSpawnPositions)
            {
                pins.Add(Instantiate(pin, pos, pin.transform.rotation));
            }
        }
    }

    public void CountDown()
    {
        //-0.7071068 is uprigt X rotation
        StartCoroutine("WaitForPins");
    }

    IEnumerator WaitForPins()
    {
        int prevPushed = pushedPins.Where(n => n==true).Count(); 
        yield return new WaitForSeconds(3f);

        CheckPushedPins();

        canSpare = prevPushed != 0;
        if (pushedPins.Where(n => n==true).Count() == 10)
        {
            if (GameManager.instance.firstTurn)
            {
                GameManager.instance.SwitchState(GameManager.gameStates.Strike);
            }
            else
            {
                if (canSpare)
                {
                    GameManager.instance.SwitchState(GameManager.gameStates.Spare);
                }
                else
                {
                    GameManager.instance.SwitchState(GameManager.gameStates.Strike);
                }
            }
        }
        else if((GameManager.instance.firstTurn && pushedPins.Where(n => n == true).Any()) || 
            (!GameManager.instance.firstTurn && pushedPins.Where(n => n == true).Count()>prevPushed)) 
        {
            GameManager.instance.SwitchState(GameManager.gameStates.Hit);
        }
        else
        {
            GameManager.instance.SwitchState(GameManager.gameStates.Miss);
        }
    }

    void CheckPushedPins()
    {
        if (pushedPins.Any())
        {
            Debug.Log($"pushed before:{pushedPins.Where(n => n == true).Count()}");
            for (int i = 0; i < pins.Count; i++)
            {
                if (pins[i] != null)
                {
                    if (!pins[i].GetComponent<Pin>().Standing)
                    {
                        pushedPins[i] = true;
                    }
                }
            }
            Debug.Log($"pushed:{pushedPins.Where(n => n==true).Count()}");
        }
        else
        {
            for (int i = 0; i < pins.Count; i++)
            {
                if (!pins[i].GetComponent<Pin>().Standing)
                {
                    pushedPins.Add(true);
                }
                else
                {
                    pushedPins.Add(false);
                }
            }
        }
    }
    public void Init()
    {
        if (pushedPins == null)
        {
            pushedPins = new List<bool>();
        }
        pins = new List<GameObject>();
    }

    public void HardResetAndInit()
    {
        pins = null;
        pushedPins = null;
        Init();
    }

    void SpawnStandingPins()
    {
        for (int i = 0; i < PinSpawnPositions.Count; i++)
        {
            if (!pushedPins[i])
            {
                pins.Add(Instantiate(pin, PinSpawnPositions[i], pin.transform.rotation));
            }
            else
            {
                pins.Add(null);
            }
        }
    }
}
