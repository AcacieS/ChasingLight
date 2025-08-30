
using UnityEngine;

public class BringUpSetting : MonoBehaviour
{
    public GameObject setting;
    private bool isSettingActive;
    public MouseLookAround mouseLookScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isSettingActive == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

    }

    public void Pause()
    {
        setting.SetActive(true);
        isSettingActive = true;
        mouseLookScript.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume()
    {
        setting.SetActive(false);
        Cursor.visible = false;
        isSettingActive = false;
        mouseLookScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
