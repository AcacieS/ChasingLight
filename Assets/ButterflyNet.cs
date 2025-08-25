using UnityEngine;

public class ButterflyNet : MonoBehaviour
{
    
     [SerializeField] private float scrollSensitivity = 100f; // deg per scroll unit
    [SerializeField] private float minAngle = -72.6f;        // lower limit relative to start
    [SerializeField] private float maxAngle = 0f;            // upper limit relative to start

    private Quaternion initialLocalRot;
    private float angle; // current angle around local X relative to start

    void Awake()
    {
        initialLocalRot = transform.localRotation;
        angle = 0f; // start at initial orientation
    }

    void Update()
    {
        // --- Old Input System ---
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // typically around -0.1..+0.1 per notch
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            angle = Mathf.Clamp(angle + scroll * scrollSensitivity, minAngle, maxAngle);
            transform.localRotation = initialLocalRot * Quaternion.AngleAxis(angle, Vector3.right);
        }
    }
}
