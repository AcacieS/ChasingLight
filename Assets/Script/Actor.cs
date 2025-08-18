using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private int point = 1;
    [SerializeField] private bool isTouched = false;
    [SerializeField] private AudioClip[] catched_sounds;
    private AudioSource audioSource;
    private Transform catcher;
    private Vector3 localHitPoint;
    private void Start()
    {
        catcher = GameObject.FindGameObjectWithTag("BugCatcher").transform;
        audioSource = GetComponent<AudioSource>();
    }

    public void Catched()
    {
        //Add point
        Debug.Log("Catched?");
        if (isTouched)
        {
            Debug.Log("Yes Catched");
            GameManager.Instance.AddPoints(point);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No Catched");

        }



    }
    void OnCollisionEnter(Collision collision)
    {
        if (!isTouched && collision.gameObject.tag == "BugCatcher")
        {
            Debug.Log("isTouched: " + isTouched);

           // Get the first contact point
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;

            AudioClip catched_sound = catched_sounds[Random.Range(0, catched_sounds.Length)];
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
            isTouched = true;
        }
        
    }
    void Update()
    {
        if (isTouched)
        {
            // Because it's parented, it will follow automatically
            transform.localPosition = localHitPoint;
            //gameObject.transform.position = catcher.position;
        }
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "BugCatcher")
        {
            isTouched = true;
            Debug.Log("isTouched: " + isTouched);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "BugCatcher")
        {

            isTouched = false;
            Debug.Log("isTouched: " + isTouched);
        }
        
    } 
}
