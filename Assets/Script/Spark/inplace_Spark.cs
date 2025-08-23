using UnityEngine;
using System.Collections;

public class inplace_Spark : Spark
{
    //[Header("Helper")]
    //[SerializeField] private GameObject spark;
    private Rigidbody rb;

    [Header("Special Property")]
    [SerializeField] private float WanderSpeed = 1;
    [SerializeField] private float MinTimeBetweenDirectionChange = 0.2f;
    [SerializeField] private float MaxTimeBetweenDirectionChange = 0.4f;
    [SerializeField] private float WanderRadius = 0.5f;

    private Vector3 targetPosition;
    private Vector3 Center;

    public override void StartFunction()
    {
        StartCoroutine(ChangeDirRoutine());
        Center = gameObject.transform.parent.position;
        rb = GetComponent<Rigidbody>();
    }
    public override void UpdateSpark()
    {
        if (isCatched)
        {
            // Because it's parented, it will follow automatically
            transform.localPosition = localHitPoint;
        }
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

    private void RandomPickDir()
    {
        // Pick a random point within WanderRadius
        Vector3 randomOffset = Random.insideUnitSphere * WanderRadius;

        targetPosition = Center + randomOffset;
        // Calculate velocity to move toward the target
        Vector3 dir = (targetPosition - Center).normalized;
        rb.linearVelocity = dir * WanderSpeed;
    }



    void FixedUpdate()
    {

        // Clamp object to stay inside the sphere
        Vector3 offset = transform.position - Center;
        if (offset.magnitude > WanderRadius)
        {
            // Project back to the edge of the sphere
            transform.position = Center + offset.normalized * WanderRadius;
            RandomPickDir();
        }
    }
    
    public override void ContactCollision(Collision collision){

        // Get the first contact point
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;

        int sound_index = Random.Range(0, catched_sounds.Length);
        AudioClip catched_sound = catched_sounds[sound_index];
        audioSource.PlayOneShot(catched_sound);

        // Convert world hitPoint into local position relative to catcher
        localHitPoint = catcher.InverseTransformPoint(hitPoint);

        // Parent the ember to the catcher
        transform.SetParent(catcher);

        // Set its local position to the stored hit point
        transform.localPosition = localHitPoint;

        // Optionally align rotation to surface normal
        transform.rotation = Quaternion.LookRotation(contact.normal);

        GameManager.Instance.AddPoints(point);
    }


    
}
