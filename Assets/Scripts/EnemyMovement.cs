using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 13f;
    [SerializeField] private float acceleration = 96f;
    [SerializeField] private float deceleration = 128f;

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
        var direction = targetPosition - transform.position;
        var localDirection = transform.InverseTransformDirection(direction);
        var isTargetFarEnough = Vector3.Distance(transform.position, targetPosition) > 1f;
        Move(isTargetFarEnough ? new Vector2(localDirection.x, localDirection.z) : Vector2.zero);
    }

    private void Move(Vector2 moveInput)
    {
        var horizontalVelocity = new Vector3
        {
            x = rb.velocity.x,
            z = rb.velocity.z
        };

        var horizontalClampedVelocity =
            horizontalVelocity.normalized * Mathf.Clamp01(horizontalVelocity.magnitude / maxSpeed);

        var moveDirection = (moveInput.x * transform.right + moveInput.y * transform.forward).normalized;

        Vector3 finalForce;

        if (moveDirection != Vector3.zero)
        {
            var accelerationForce = moveDirection - horizontalClampedVelocity;
            accelerationForce *= acceleration;
            finalForce = accelerationForce;
        }
        else
        {
            var decelerationForce = -horizontalClampedVelocity;
            decelerationForce *= deceleration;
            finalForce = decelerationForce;
        }

        rb.AddForce(finalForce, ForceMode.Acceleration);
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