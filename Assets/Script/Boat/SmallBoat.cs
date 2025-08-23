using System.Collections.Generic;
using UnityEngine;

public class SmallBoat : MonoBehaviour
{
    [SerializeField] private GameObject smallBoat;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject pAreaTarget;

    
    public void SpawnSmallBoat()
    {
        GameObject boat = Instantiate(smallBoat, startPos.position, smallBoat.transform.rotation);
        boat.GetComponent<SmallBoatInstance>().SetAreaTarget(pAreaTarget);
        float scale = Random.Range(20f, 50f);
        smallBoat.transform.localScale = new Vector3(scale, scale, scale);
    }
}
