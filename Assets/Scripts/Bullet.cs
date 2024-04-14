using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifeTime = 3;

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}