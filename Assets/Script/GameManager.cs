
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("General")]
    [SerializeField] private GameObject boat;
    [SerializeField] private Transform player;



    [Header("Points")]

    [SerializeField] private Text pointTxt;
    public static int points = 0;

    [Header("CutScene")]
    [SerializeField] private PlayableDirector director;
    [SerializeField] private bool testSkipDirector = false;

    [Header("Spark")]
    [SerializeField] private GameObject[] sparks;
    [SerializeField] Camera camera;
    [SerializeField] Attack attack;
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float maxSpawnDelay = 2f;
    [SerializeField] private List<GameObject> currentActiveSpark;
    private int indexSpark = 0;

    [Header("Bright")]
    [SerializeField] private Material netFrame_Mat;
    [SerializeField] private float intensity = -10f;
    [SerializeField] private float AddIntensity;
    [SerializeField] private float maxIntensity = 3f;
    [SerializeField] private Color baseColor;


    public static event System.Action<int> OnPointsChanged;

    private bool spawnActive = true;

    // public static int scene = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
        currentActiveSpark.Add(sparks[indexSpark]);
        netFrame_Mat.SetColor("_EmissionColor", baseColor * Mathf.Pow(2.0f, intensity));

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnPointsChanged += HandlePointsChanged;
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
        OnPointsChanged?.Invoke(points);
    }
    public int GetPoints()
    {
        return points;
    }

    public void RemovePoints(int amount)
    {
        points = Mathf.Max(points - amount, 0);
        pointTxt.text = points.ToString();
        OnPointsChanged?.Invoke(points);
        //Debug.Log("Points: " + points);

    }
    public void ResetPoints()
    {
        pointTxt.text = points.ToString();
        points = 0;
        OnPointsChanged?.Invoke(points);
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {

            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
            if (!spawnActive) continue;
            float rMin = attack.getSpawnMinDistance(); // minimum distance from camera
            float rMax = attack.getAttackDistance(); // maximum distance

            Vector3 dir = Random.onUnitSphere;
            float distance = Mathf.Sqrt(Random.Range(rMin * rMin, rMax * rMax)); // sqrt for uniform distribution
            Vector3 randomPos = camera.transform.position + dir * distance;

            Spawn(randomPos);
        }
    }

    private void HandlePointsChanged(int newPoints)
    {
        ChangeNetFrame();
        if (newPoints == 5)
        {
            AddSpark();
            if (!testSkipDirector)
            {
                director.Play();
                spawnActive = false;
            }
        }
    }
    private void ChangeNetFrame()
    {
        if (intensity < 3)
        {
            intensity += AddIntensity;
            netFrame_Mat.SetColor("_EmissionColor", baseColor * Mathf.Pow(2.0f, intensity));
        }

    }

    public void AddSpark()
    {
        indexSpark++;
        currentActiveSpark.Add(sparks[indexSpark]);
    }

    private void Spawn(Vector3 pos)
    {
        int index = Random.Range(0, currentActiveSpark.Count);
        GameObject spark = Instantiate(sparks[index], pos, Quaternion.identity);
        GameObject childSpark = spark.transform.GetChild(0).gameObject;  // 0 = first child
        childSpark.GetComponent<Spark>().SetBoat(boat);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, attack.getAttackDistance());
    }
    private void Update()
    {
        if (director.state != PlayState.Playing)
        {
            Debug.Log("active again");
            spawnActive = true;
        }
    }
}
