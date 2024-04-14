using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioSource shootSound;

    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private float bulletSpeed = 20;

    [SerializeField] private int minInterval = 1;
    [SerializeField] private int maxInterval = 3;

    private Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        var repeatRate = Random.Range(minInterval * 100, maxInterval * 100) / 100f;

        InvokeRepeating(nameof(Fire), repeatRate, repeatRate);
    }

    private void Update()
    {
        var direction = target.position - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);
        var rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void Fire()
    {
        shootSound.Play();
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        var direction = target.position - transform.position;
        bulletRigidbody.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
    }

    public int Health { get; set; } = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public Action<int> OnHealthChanged { get; set; }

    public void Die()
    {
        Destroy(gameObject);
    }
}