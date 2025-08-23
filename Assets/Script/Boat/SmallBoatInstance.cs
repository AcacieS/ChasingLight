using UnityEngine;

public class SmallBoatInstance : MonoBehaviour
{
    [SerializeField] GameObject areaTarget;
    private Vector3 targetPosition;

    [SerializeField] float speed = 2f;
    [SerializeField] float rotateSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNewTarget();
    }

    void Update()
{
    if (areaTarget == null) return;

    // Move towards target
    transform.position = Vector3.MoveTowards(
        transform.position,
        targetPosition,
        speed * Time.deltaTime
    );

    // Rotation
    float initialX = transform.rotation.eulerAngles.x;

    Vector3 direction = targetPosition - transform.position;
    direction.x = 0f; // ignore X

    if (direction != Vector3.zero)
    {
        float angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(initialX, transform.rotation.eulerAngles.y, -angle);

        // Only rotate if angle difference is significant
        if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    // Check if reached target position
    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    {
        // Stop or do floating behavior
    }
}
    public void SetAreaTarget(GameObject pAreaTarget)
    {
        areaTarget = pAreaTarget;
        SetNewTarget();
    }
    private void SetNewTarget()
    {
        if (areaTarget == null) return;

        Collider col = areaTarget.GetComponent<Collider>();
        if (col != null)
        {
            Bounds bounds = col.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            targetPosition = new Vector3(x, transform.position.y, z);
        }
        else
        {
            Debug.LogWarning("AreaTarget needs a Collider!");
        }
    }
}
