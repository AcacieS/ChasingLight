using TMPro;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField] private TextMeshPro words;
    public void ChangeNewChar(char newChar)
    {
        words.text = newChar+"";
    }
    public void DestroyAnim()
    {
        Destroy(gameObject);
    }
}
