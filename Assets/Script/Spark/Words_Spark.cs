
using System.Collections;
using UnityEngine;

public class Words_Spark : Spark
{
    [SerializeField] private Sprite[] words;
    [SerializeField] private float IntervalSpawns;
    [SerializeField] private GameObject word;
    private float timeBtwSpawns = 0;
    private int index = 0;
    private Rigidbody rb;

    [Header("Special Property")]
    [SerializeField] private WordsType wordType;
    [SerializeField] private float WanderSpeed = 1;
    [SerializeField] private float MinTimeBetweenDirectionChange = 0.2f;
    [SerializeField] private float MaxTimeBetweenDirectionChange = 0.4f;
    [SerializeField] private float WanderRadius = 0.5f;
    [SerializeField] private bool touchedBoat = false;

    private Vector3 targetPosition;
    private Vector3 Center;
    private float scaleDown = 1f;

    public override void StartFunction()
    {
        StartCoroutine(ChangeDirRoutine());
        Center = gameObject.transform.parent.position;
        rb = GetComponent<Rigidbody>();
        scaleDown = transform.parent.localScale.x * (2f / 3f);
    }

    public override void UpdateSpark()
    {
        SpawnWord();
        TowardsBoat();
        
    }
    // private void TowardsBoat()
    // {

    //     if (!isCatched || touchedBoat) return;
    //     Vector3 target = boat.transform.position;
    //     Debug.Log("Towards Boat: " + target);

    //     // Move towards target
    //     transform.position = Vector3.MoveTowards(
    //         transform.position,
    //         target,
    //         WanderSpeed * Time.deltaTime
    //     );
    //      Debug.Log("Transform: " + transform.position);

    //     // Check if reached target position
    //     if (Vector3.Distance(transform.position, target) < 0.1f)
    //     {
    //         Debug.Log("Finish Words Spark");
    //         touchedBoat = true;
    //         die = false;
    //         // Stop or do floating behavior
    //     }
    // }

    private void TowardsBoat()
    {
        if (!isCatched || touchedBoat) return;

        Vector3 target = boat.transform.position;
        

        // Direction toward the boat
        Vector3 dir = (target - transform.position).normalized;

        // Set velocity toward the target
        rb.linearVelocity = dir * WanderSpeed;
        Debug.Log("?not got there: " + Vector3.Distance(transform.position, target));
        // Check if close enough to "reach" the boat
        if (Vector3.Distance(transform.position, target) < 0.7f)
        {
            Debug.Log("Finish Words Spark");

            touchedBoat = true;
            die = false;
            rb.linearVelocity = Vector3.zero; // stop moving
            boatScript.WordBoat(wordType);
            Destroy(transform.parent.gameObject);
        }
    }


    private void SpawnWord()
    {
        if (timeBtwSpawns <= 0)
        {
            GameObject instance = (GameObject)Instantiate(word, transform.position, Quaternion.identity);
            instance.transform.localScale = new Vector3(scaleDown, scaleDown, scaleDown);
            instance.GetComponent<ChangeSprite>().ChangeNewSprite(words[index]);
            index = (index + 1) % words.Length;
            timeBtwSpawns = IntervalSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }

    }

    

    private IEnumerator ChangeDirRoutine()
    {
        while (!isCatched)
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
        if (isCatched) return;
        // Clamp object to stay inside the sphere
        Vector3 offset = transform.position - Center;
        if (offset.magnitude > WanderRadius)
        {
            // Project back to the edge of the sphere
            transform.position = Center + offset.normalized * WanderRadius;
            RandomPickDir();
        }
    }
    public override void DrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.parent.position, WanderRadius);
    }
}
