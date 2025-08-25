
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word
{
    [SerializeField] private GameObject wordObj;
    [SerializeField] private string wordType;

    public string GetWordType()
    {
        return wordType;
    }
    public GameObject GetWordObj()
    {
        return wordObj;
    }

}
[System.Serializable]
public class WordFind
{
    [SerializeField] private Word word;
    [SerializeField] private bool isActivate = false;

    public void SetIsActivate()
    {
        isActivate = true;
        word.GetWordObj().SetActive(true);
    }
    public bool GetIsActivate()
    {
        return isActivate;
    }
    public Word GetWord()
    {
        return word;
    }
}

[System.Serializable]
public class Sentence
{
    [SerializeField] private GameObject showSentenceObj;
    private bool isFinish = false;
    [SerializeField] private WordFind[] words;

    public void ShowSentence(bool isActive)
    {
        if (isActive)
        {
            DesactivateWords();
        }
        showSentenceObj.SetActive(isActive);
    }
    private void DesactivateWords()
    {
        for (int i = 0; i < words.Length; i++)
        {
            GetWordType(i).SetActive(false);
        }
    }
    
    public bool GetIsFinishSentence()
    {
        if (!isFinish)
        {
            Debug.Log("hey");
            for (int i = 0; i < words.Length; i++)
            {
                if (!words[i].GetIsActivate())
                {
                    Debug.Log("which world not activate: "+words[i].GetWord().GetWordType());
                    return false;
                }
            }
            isFinish = true;
        }

        return isFinish;
    }
    public void SetIsActivate(int index)
    {
        Debug.Log("set active in ");
        if (!isFinish)
        {
            words[index].SetIsActivate();
            Debug.Log("set active in in");
        }
    }

    public string GetWord(int pIndex)
    {
        return words[pIndex].GetWord().GetWordType();
    }
    public GameObject GetWordType(int pIndex)
    {
        return words[pIndex].GetWord().GetWordObj();
    }
    

    public int Size()
    {
        return words.Length;
    }

}

public class WordBoat : MonoBehaviour
{
    public static WordBoat Instance { get; private set; }

    [SerializeField] private Sentence[] sentences;
    public static int index_sentence = 0;
    [SerializeField] private Dictionary<string, int> words_place = new();

    private void Start()
    {
        AddDicWords();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // optional safeguard for multiple instances
            return;
        }
        Instance = this;
    }
    public string[] GetSentence()
    {
        Sentence sentence = sentences[index_sentence];
        string[] listW = new string[sentence.Size()];
        Debug.Log("Get Sentence");
        for (int i = 0; i < sentence.Size(); i++)
        {
            listW[i] = sentence.GetWord(i);
            Debug.Log("Sentence added: " + sentence.GetWord(i));
        }

        return listW;
    }
    private void AddDicWords()
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            for (int e = 0; e < sentences[i].Size();e++)
            {
                // Debug.Log("time");
                // Debug.Log("Added word" + sentences[i].GetWord(e));
                words_place.Add(sentences[i].GetWord(e), e);
            }
        }
    }
    public void SpawnWordBoat()
    {
        Debug.Log("Hey spawn Word Boat");
    }
    public void ShowWordBoat(string wordType)
    {
        Debug.Log("Should Show words: "+wordType);
        int index_word = words_place[wordType];
        sentences[index_sentence].SetIsActivate(index_word);
    }

    public bool IsSentenceFinish()
    {
        Debug.Log("index sentence first: " + index_sentence);
        bool isFinish = sentences[index_sentence].GetIsFinishSentence();
        if (isFinish)
        {
            StartCoroutine(ShowSentenceWithDelay(2f));
            sentences[index_sentence].ShowSentence(true);
            index_sentence++;
            Debug.Log("index sentence: " + index_sentence);
        }
        return isFinish;

    }
    private IEnumerator ShowSentenceWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    
}
