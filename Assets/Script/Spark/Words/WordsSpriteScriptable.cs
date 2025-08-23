using UnityEngine;

[CreateAssetMenu(fileName = "WordsSpriteScriptable", menuName = "ScriptableObjects/WordsSpriteScriptable", order = 1)]
public class WordsSpriteScriptable : ScriptableObject
{
    public WordsType wordType;
    public Sprite[] charsSprite;

}
