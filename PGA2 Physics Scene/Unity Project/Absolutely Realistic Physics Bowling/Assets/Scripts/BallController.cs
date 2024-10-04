using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallController : MonoBehaviour
{
    bool hasTriggeredBuffer = false;
    Queue<Transform> BallPositions;
    Rigidbody rb;
    Vector3 direction;
    float multPos = 2f;
    float dirNormal = 1f;
    float powerNormal = 1f;
    float power = 0f;
    float powerMax = 255f;
    float powerMult = 250f;
    [SerializeField] GameObject lineObj;
    void Start()
    {
        BallPositions = new Queue<Transform>();
        foreach (var item in GameManager.instance.BallPositions.GetComponentsInChildren<Transform>())
        {
            BallPositions.Enqueue(item);
        }
        //Dequeues parent transform
        BallPositions.Dequeue();

        rb = GetComponent<Rigidbody>();
        direction = new Vector3(0, 0, -14f);
        lineObj.GetComponent<LineRenderer>().enabled = false;
        lineObj.GetComponent<LineRenderer>().useWorldSpace = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = rb.position;
        transform.rotation = rb.rotation;
        switch (GameManager.instance.GetState())
        {
            case GameManager.gameStates.SelectPos:
                MovePosHorizontal();
                break;
            case GameManager.gameStates.SelectDir:
                SelectDirection();
                break;
            case GameManager.gameStates.SelectPower:
                SelectPower();
                break;
        }
    }

    void MovePosHorizontal()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.freezeRotation = true;

            GameManager.instance.SwitchState(GameManager.gameStates.SelectDir);
            lineObj.GetComponent<LineRenderer>().enabled = true;
        }

        float ReqZ = BallPositions.Peek().position.z;
        if ((ReqZ < 0 && transform.position.z <= ReqZ) || (ReqZ > 0 && transform.position.z >= ReqZ))
        {
            BallPositions.Enqueue(BallPositions.Dequeue());
            ReqZ = BallPositions.Peek().position.z;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(Vector3.Normalize(new Vector3(0, 0, ReqZ)) * multPos * multPos * rb.mass, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.Normalize(new Vector3(0, 0, ReqZ)) * multPos * multPos * rb.mass, ForceMode.Force);
        }
    }

    void SelectDirection()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.SwitchState(GameManager.gameStates.SelectPower);
        }
        if (direction.y >= 90f)
        {
            dirNormal = -1f;
        }
        if (direction.y <= -90f)
        {
            dirNormal = 1f;
        }
        direction = new Vector3(90f, direction.y + dirNormal * Time.deltaTime * 35f, 0);

        rb.rotation = Quaternion.Euler(direction);
    }

    void SelectPower()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.SwitchState(GameManager.gameStates.Shooting);
            Shoot();
        }
        if (power <= 0)
        {
            power = 0f;
            powerNormal = 1f;
        }
        if (power >= powerMax)
        {
            power = powerMax;
            powerNormal = -1f;
        }
        power += 1f * powerNormal;
        GameManager.instance.GetComponentInChildren<HUDManager>().SetPowerSlider(power, powerMax, powerMult);
    }

    void Shoot()
    {
        rb.freezeRotation = false;
        lineObj.GetComponent<LineRenderer>().enabled = false;
        Vector3 shootDir = new Vector3(-1f * rb.rotation.x * rb.transform.right.x, rb.rotation.y * rb.transform.forward.y, -2f * rb.rotation.z * rb.transform.up.z);
        rb.AddForce(shootDir * power * powerMult, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GameManager.instance.GetState() == GameManager.gameStates.Shooting && !hasTriggeredBuffer)
        {
            if (collision.gameObject.CompareTag("Gutter"))
            {
                GameManager.instance.SwitchState(GameManager.gameStates.Gutter);
            }
            if (collision.gameObject.CompareTag("Pin"))
            {
                RegisterHit();
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.GetState() == GameManager.gameStates.Shooting && !hasTriggeredBuffer)
        {
            switch (other.tag)
            {
                case "Miss":
                    GameManager.instance.SwitchState(GameManager.gameStates.Miss);
                    break;
                case "BallBuffer":
                    RegisterHit();
                    break;
            }
        }

    }

    void RegisterHit()
    {
        hasTriggeredBuffer = true;
        GameManager.instance.GetComponentInChildren<PinManager>().CountDown();
    }
}
