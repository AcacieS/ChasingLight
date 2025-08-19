using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRenderer;
    public void ChangeNewSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
    public void DestroyAnim()
    {
        Destroy(gameObject);
    }
}
