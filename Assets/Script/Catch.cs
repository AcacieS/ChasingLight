using UnityEngine;

public class Catch : MonoBehaviour
{
    [SerializeField] private bool isTouched = false;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollision");
        if (collision.gameObject.layer == LayerMask.NameToLayer("AttackLayer") && isTouched)
        {
            GameManager.Instance.AddPoints(1);
            Destroy(collision.gameObject);
        }
    }
    
}
