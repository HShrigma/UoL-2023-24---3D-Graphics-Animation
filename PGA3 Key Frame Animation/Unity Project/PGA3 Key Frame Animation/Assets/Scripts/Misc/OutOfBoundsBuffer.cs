using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsBuffer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Caught");
            if (other.GetComponent<Enemy>().canDie)
            {
                other.GetComponent<Enemy>().Die();
            }
        }
    }
}
