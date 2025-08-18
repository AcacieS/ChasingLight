using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Text pointTxt;
    [SerializeField] private GameObject[] sparks;
    [SerializeField] Camera camera;
    [SerializeField] Attack attack;
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float maxSpawnDelay = 2f;
    

    public static int points = 0;
    public static int scene = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        pointTxt.text = points.ToString();
        Debug.Log("Points: " + points);
    }
    public int GetPoints()
    {
        return points;
    }
    public void RemovePoints(int amount)
    {
        points = Mathf.Max(points - amount, 0);
        pointTxt.text = points.ToString();
        Debug.Log("Points: " + points);

    }
    public void ResetPoints()
    {
        pointTxt.text = points.ToString();
        points = 0;
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
            float rMin = attack.getSpawnMinDistance(); // minimum distance from camera
            float rMax = attack.getAttackDistance(); // maximum distance

            // Random direction
            Vector3 dir = Random.onUnitSphere; // normalized direction in all 3D
            // Random distance between rMin and rMax
            float distance = Random.Range(rMin, rMax);

            // Compute random position
            Vector3 randomPos = camera.transform.position + dir * distance;
            //Vector3 randomPos = camera.transform.position + Random.insideUnitSphere * attack.getAttackDistance();
            Spawn(randomPos);
        }   
    }
    
    private void Spawn(Vector3 pos)
    {
        int index = Random.Range(0, sparks.Length);
        //Debug.Log("sparks.Length - 1"+(sparks.Length - 1));
        //Debug.Log("index " + index );
        Instantiate(sparks[index], pos, Quaternion.identity);
    }
}
