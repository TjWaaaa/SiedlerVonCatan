using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public List<GameObject> neighboringVillageSlots = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNeighboringVillageSlots(List<GameObject> neighbors) {
        neighboringVillageSlots = neighbors;
    }
}
