using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNeighbors : MonoBehaviour
{
    public GameObject prefab;
    public GameObject[] neighborVillageSlots = new GameObject[3];
    private GameObject[] neighborRoadSlots;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNeighborVillageSlots(GameObject[] neighbors) {
        neighborVillageSlots = neighbors;
    }
}
