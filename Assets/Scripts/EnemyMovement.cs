using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float minDistance = 1f;
    [SerializeField]
    private float maxDistance = 5f;
    [SerializeField]
    private float changeDirectionInterval = 2f;

    private Vector3 direction;
    private float timer;

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        float xDirection = Random.Range(-1f, 1f);
        float zDirection = Random.Range(-1f, 1f);

        direction = new Vector3(xDirection, 0, zDirection).normalized;

        timer = changeDirectionInterval;
    }
}
