using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float speed;
    [SerializeField] float moveDelay;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject particles;
    public Rigidbody rb;
    public int tier;
    public UnityEvent OnDeath;
    public bool canDie { get; private set; }
    private void Awake()
    {
        canDie = false;
        StartCoroutine(DisableSpawnInvincibility());
    }
    void Start()
    {
        OnDeath.AddListener(delegate { GameObject.FindGameObjectWithTag("Respawn").GetComponent<SlimeSpawner>().OnSlimeDeathSpawn(parent); });
        OnDeath.AddListener(delegate { GameManager.instance.AddScore(tier); });
        StartCoroutine(FollowPlayerRecursive());
        float scale = tier * 0.4f;
        parent.GetComponent<Transform>().localScale = parent.GetComponent<Transform>().localScale * scale;
    }

    private void FixedUpdate()
    {
        rb.rotation = transform.rotation;
    }
    void Update()
    {
        speed = 2.5f;
        speed += GameManager.instance.Wave * 0.1f;
        transform.LookAt(player.transform.position);
        if (transform.position.y < -10f)
        {
            Die();
        }

    }

    void MoveTowardsPlayer()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    IEnumerator FollowPlayerRecursive()
    {
        yield return new WaitForSeconds(moveDelay);
        MoveTowardsPlayer();
        StartCoroutine(FollowPlayerRecursive());
    }
    IEnumerator DisableSpawnInvincibility()
    {
        yield return new WaitForSeconds(1.5f);
        canDie = true;
    }
    public void Die()
    {
        OnDeath.Invoke();
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(parent);
    }

    public void PushInDir(Vector3 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.mass = tier;
        rb.AddForce(dir * 5f * rb.mass, ForceMode.Impulse);
    }
}
