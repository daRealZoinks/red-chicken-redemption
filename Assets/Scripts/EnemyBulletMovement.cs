using System.Collections;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 3f;
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private Rigidbody body;
    private Transform target;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        TargetPlayer();
    }

    private void TargetPlayer()
    {
        var direction = (target.position - transform.position).normalized;
        body.velocity = direction * speed;
        StartCoroutine(Lifetime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
