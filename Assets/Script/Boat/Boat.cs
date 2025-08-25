using System;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] SmallBoat smallBoatScript;
    [SerializeField] WordBoat wordBoatScript;

    // Update is called once per frame
    void Update()
    {

    }
    public void Captured(SparkType sparkType)
    {
        Debug.Log("Captured Boat");
        switch (sparkType)
        {
            case SparkType.NormalSpark:
                smallBoatScript.SpawnSmallBoat();
                break;

            case SparkType.WordSpark:
                wordBoatScript.SpawnWordBoat();
                break;

            case SparkType.SpecialSpark:
                smallBoatScript.SpawnSmallBoat();
                break;
            default:
                break;

        }
    }
    public void WordBoat(string wordType)
    {
        Debug.Log("Show words");
        wordBoatScript.ShowWordBoat(wordType);
        
    }
}
