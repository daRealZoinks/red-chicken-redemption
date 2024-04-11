using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
            EnemyBulletMovement bulletMovement = bullet.GetComponent<EnemyBulletMovement>();
            bulletMovement.SetTarget(target);
        }
    }
}
