using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float positionChangeRadius = 10f;
    [SerializeField] private float playerPositionInfluenceMin = 0.2f;
    [SerializeField] private float playerPositionInfluenceMax = 0.8f;

    [SerializeField] private float timeToChangePositionMin = 3f;
    [SerializeField] private float timeToChangePositionMax = 6f;

    private Vector3 targetPosition;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;

        var timeToChangePosition = Random.Range(timeToChangePositionMin, timeToChangePositionMax);

        InvokeRepeating(nameof(UpdateTargetPosition), timeToChangePosition, timeToChangePosition);
    }

    private void FixedUpdate()
    {
        var direction = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > 1)
        {
            rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * direction);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    private void UpdateTargetPosition()
    {
        var rangeX = Random.Range(-10, 10);
        var rangeZ = Random.Range(-10, 10);

        var playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        var playerPositionInfluence = Random.Range(playerPositionInfluenceMin, playerPositionInfluenceMax);

        targetPosition = transform.position + playerPosition * playerPositionInfluence + new Vector3(rangeX, 0, rangeZ);
    }
}