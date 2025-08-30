using UnityEngine;
using UnityEngine.UI;

public class MouseLookAround : MonoBehaviour
{
    float rotationX = 0f;
    float rotationY = 0f;
    public float sensitivity = 15f;
    [SerializeField] private Slider slider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock to center
        Cursor.visible = false; // Hide it
        slider.value = sensitivity;
        slider.onValueChanged.AddListener(SetSensitivity);
    }

    // Update is called once per frame
    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
    public void SetSensitivity(float value)
    {
        sensitivity = value;
        Debug.Log("sensitivity" + sensitivity);
    }
}
