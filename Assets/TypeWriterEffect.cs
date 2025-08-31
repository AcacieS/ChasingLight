using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;        // Assign in Inspector
    [TextArea(1,6)]
    [SerializeField] private string fullText;         // The full text you want to reveal
    [SerializeField] private float interval = 0.1f;   // Time between each character
    [SerializeField] private float delayBeforeSceneChange = 20f; // Delay before scene change

    private float timer;
    private int index;

    void Start()
    {
        tmpText.text = ""; // start empty
        timer = interval;
        index = 0;
        StartCoroutine(ChangeSceneAfterDelay());
        
    }
    private System.Collections.IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene("Scenes/End");
    }

    void Update()
    {
        if (index < fullText.Length)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                tmpText.text += fullText[index]; // add next character
                index++;
                timer = interval;
            }
        }
    }
}
