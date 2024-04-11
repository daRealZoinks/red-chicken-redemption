using System.Collections;
using System.Collections.Generic;
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
        body.drag = 0f;
        body.useGravity = false;
        body.angularDrag = 0f;
    }

    void Update()
    {
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        TargetPlayer();
    }

    private void TargetPlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        body.velocity = direction * speed;
        StartCoroutine(Lifetime());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Destroy(gameObject);
    }
    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
