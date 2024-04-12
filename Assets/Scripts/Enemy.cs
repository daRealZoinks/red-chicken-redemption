using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private int minInterval = 1;
    [SerializeField] private int maxInterval = 3;

    private Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        var time = Random.Range(minInterval * 100, maxInterval * 100) / 100f;

        InvokeRepeating(nameof(Fire), time, time);
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
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        var bulletMovement = bullet.GetComponent<EnemyBulletMovement>();
        bulletMovement.SetTarget(target);
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

    public void Die()
    {
        Destroy(gameObject);
    }
}