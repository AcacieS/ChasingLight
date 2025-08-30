using UnityEngine;

public class ShowEnd : MonoBehaviour
{
    [SerializeField] private string words;
    [SerializeField] private float IntervalSpawns;
    [SerializeField] private GameObject word;
    private float timeBtwSpawns = 0;
    private int index = 0;
    [SerializeField] private float scaleDown = 1f;
    void Update()
    {
        SpawnWord();
    }
    private void SpawnWord()
    {
        if (timeBtwSpawns <= 0)
        {
            GameObject instance = (GameObject) Instantiate(word, transform.position, Quaternion.identity);
            instance.transform.localScale = new Vector3(-scaleDown, scaleDown, scaleDown);
            instance.GetComponent<ChangeSprite>().ChangeNewChar(words[index]);
            index = (index + 1) % words.Length;
            timeBtwSpawns = IntervalSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }

    }
}
