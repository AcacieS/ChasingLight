using System.Collections;
using UnityEngine;

public abstract class Spark : MonoBehaviour
{   
    [Header("DEV")]
    [SerializeField] private bool ShowGizmos = true;
    [SerializeField] private bool die = true;

    [Header("Property")]
    

    [SerializeField] protected int point = 1;
    [SerializeField] protected float minDeath = 3;
    [SerializeField] protected float maxDeath = 6;

    [SerializeField] protected AudioClip[] catched_sounds;
    
    
    protected AudioSource audioSource;
    protected Transform catcher;
    protected Vector3 localHitPoint;

    protected bool isCatched = false;

    private void Start()
    {
        catcher = GameObject.FindGameObjectWithTag("BugCatcher").transform;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Death());
        StartFunction();
    }

    public virtual void StartFunction(){
        
    }

    private void Update()
    {
        UpdateSpark();
        if (isCatched)
        {
            // Because it's parented, it will follow automatically
            transform.localPosition = localHitPoint;
        }
    }

    public virtual IEnumerator Death()
    {
        float death_time = Random.Range(minDeath, maxDeath);
        float time = 0;
        while (time < death_time)
        {
            yield return new WaitForSeconds(1);
            time++;
        }
        if(!isCatched&&die){
            Destroy(gameObject);
        }
        
    }

    public virtual void UpdateSpark()
    {

    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCatched && collision.gameObject.tag == "BugCatcher")
        {
            Debug.Log("is Catched: " + isCatched);
            isCatched = true;
            ContactCollision(collision);
        }
        
    }

    public virtual void ContactCollision(Collision collision){

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



    void OnDrawGizmosSelected()
    {
        if (ShowGizmos)
        {  
            DrawGizmos();
        }
    }
    public virtual void DrawGizmos(){
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(gameObject.transform.parent.position, WanderRadius);
    }

}
