using UnityEngine;
using System.Collections;

public class inplace_Spark : MonoBehaviour
{
    [SerializeField] private bool ShowGizmos = true;
    [SerializeField] private float minDeath = 3;
    [SerializeField] private float maxDeath = 6;
    [SerializeField] private float WanderSpeed = 1;
    [SerializeField] private float MinTimeBetweenDirectionChange = 0.2f;
    [SerializeField] private float MaxTimeBetweenDirectionChange = 0.4f;
    [SerializeField] private float WanderRadius = 0.5f;
    [SerializeField] private GameObject spark;
    private Vector3 targetPosition;
    [SerializeField] private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChangeDirRoutine());
        StartCoroutine(Death());
    }
    private IEnumerator Death()
    {
        float death_time = Random.Range(minDeath, maxDeath);
        float time = 0;
        while (time < death_time)
        {
            yield return new WaitForSeconds(1);
            time++;
        }
        Destroy(gameObject);
    }
     private IEnumerator ChangeDirRoutine()
    {
        while (true)
        {
            float delay = Random.Range(MinTimeBetweenDirectionChange, MaxTimeBetweenDirectionChange);
            yield return new WaitForSeconds(delay);
            RandomPickDir();
        }
    }
    private void RandomPickDir() {
        // Pick a random point within WanderRadius
        Vector3 randomOffset = Random.insideUnitSphere * WanderRadius;
        
        targetPosition = transform.position + randomOffset;
        // Calculate velocity to move toward the target
        Vector3 dir = (targetPosition - transform.position).normalized;
        rb.linearVelocity = dir * WanderSpeed;

          
    }



    void FixedUpdate()
    {
        // Clamp object to stay inside the sphere
        Vector3 offset = spark.transform.position - transform.position;
        if (offset.magnitude > WanderRadius)
        {
            // Project back to the edge of the sphere
            spark.transform.position = transform.position + offset.normalized * WanderRadius;

            RandomPickDir();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (ShowGizmos)
        {  
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, WanderRadius);
        }
    }
}
