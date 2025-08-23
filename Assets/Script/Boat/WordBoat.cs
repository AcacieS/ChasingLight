using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word
{
    [SerializeField] private GameObject word;
    [SerializeField] private WordsType wordType;

    public WordsType GetWordType()
    {
        return wordType;
    }
    public GameObject GetWord()
    {
        return word;
    }

}
public class WordBoat : MonoBehaviour
{
    [SerializeField] private Word[] words;
    [SerializeField] private Dictionary<WordsType, GameObject> words_place = new();
    private void Start()
    {
        AddDicWords();
    }
    private void AddDicWords()
    {
        for (int i = 0; i < words.Length; i++)
        {
            words_place.Add(words[i].GetWordType(), words[i].GetWord());
        }
    }
    public void SpawnWordBoat()
    {
        Debug.Log("Hey spawn Word Boat");
    }
    public void ShowWordBoat(WordsType wordType)
    {
        Debug.Log("Should Show words");
        GameObject word = words_place[wordType];
        if (word.activeSelf)
        {

        }
        else
        {
            Debug.Log("Activate");
            word.SetActive(true);
        }
    }
}
