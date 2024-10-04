using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    GameObject hitbox;
    public enum playerStates
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    Rigidbody rb;
    Vector2 moveInput;
    [SerializeField] playerStates state;
    [SerializeField] float speed;
    public UnityEvent OnDeath;
    void Start()
    {
        Physics.IgnoreLayerCollision(9, 8,false);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hitbox = GetComponentInChildren<BoxCollider>().gameObject;
        hitbox.SetActive(false);
    }
    void FixedUpdate()
    {
        if (state != playerStates.Dead)
        {
            transform.position = new Vector3(rb.position.x, -.1f, rb.position.z);
        }
    }
    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
        SetStateOnInput();
        ActOnState();
    }

    void SetStateOnInput()
    {
        if (state != playerStates.Dead)
        {
            if (state != playerStates.Attacking)
            {
                if (moveInput.magnitude == 0)
                {
                    state = playerStates.Idle;
                }
                else
                {
                    state = playerStates.Moving;
                }
            }


            if (Input.GetAxisRaw("Fire1") == 1)
            {
                state = playerStates.Attacking;
            }
        }

        animator.SetInteger("State", (int)state);
    }

    void ActOnState()
    {
        if(state == playerStates.Moving)
        {
            Move();
            RotateOnIpnut();
        }
    }

    void Move()
    {
        float moveMag = 1f;
        if (moveInput.magnitude == Mathf.Sqrt(2))
        {
            moveMag = 0.75f;
        }
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(dir * speed * moveMag, ForceMode.Impulse);
    }

    void RotateOnIpnut()
    {
        if (moveInput.magnitude != 0 && Time.timeScale==1f)
        {
            float yRot = 90f * moveInput.x;
            if(yRot == 0 && moveInput.y == -1)
            {
                yRot = 180f;
            }
            else if (yRot > 0)
            {
                yRot -= 45f * moveInput.y;
            }
            else if(yRot < 0)
            {
                yRot += 45f * moveInput.y;
            }
            rb.transform.rotation = Quaternion.Euler(0f, yRot, 0f);
        }

    }

    public void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
    }

    public void FinishedAttack()
    {
        state = playerStates.Idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (state != playerStates.Dead && 
                collision.gameObject.GetComponent<Enemy>().canDie && 
                collision.transform.position.y <= 1f)
            {
                OnDeath.Invoke();
            }
        }
    }

    public void WhenDead()
    {
        state = playerStates.Dead;
        Physics.IgnoreLayerCollision(9, 8);
        rb.freezeRotation = false;
        rb.AddForce(transform.forward * -1000f, ForceMode.Impulse);
        rb.AddTorque(transform.right * -300f, ForceMode.Impulse);
    }
}
