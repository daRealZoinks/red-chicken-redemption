using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private Transform target;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.drag = 0f;
        body.useGravity = false;
        body.angularDrag = 0f;

        Vector3 direction = (target.position - transform.position).normalized;
        body.velocity = direction * speed;
    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
