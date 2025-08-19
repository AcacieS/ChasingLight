using UnityEngine;

public class Spark_Particle_Collision : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("BugCatcher"))
        {

            // Award point
            //Debug.Log("Caught an ember!");
            //GameManager.Instance.AddPoints(1);
            //Destroy(gameObject);
        }
    }
}
