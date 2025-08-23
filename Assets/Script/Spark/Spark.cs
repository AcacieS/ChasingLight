using System.Collections;
using UnityEngine;

public abstract class Spark : MonoBehaviour
{   
    [Header("DEV")]
    [SerializeField] private bool ShowGizmos = true;
    [SerializeField] protected bool die = true;
    [SerializeField] protected Boat boatScript;
    [SerializeField] protected GameObject boat;

    [Header("Property")]

    [SerializeField] protected SparkType sparkType;
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
        
    }
    public void SetBoat(GameObject pBoat)
    {
        boat = pBoat;
        boatScript = pBoat.GetComponent<Boat>();
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
        if (!isCatched && die)
        {
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
            boatScript.Captured(sparkType);
            ContactCollision(collision);
        }
        
    }
    public virtual void ContactCollision(Collision collision)
    {
         int sound_index = Random.Range(0, catched_sounds.Length);
        AudioClip catched_sound = catched_sounds[sound_index];
        audioSource.PlayOneShot(catched_sound);

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
