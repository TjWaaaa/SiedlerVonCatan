using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [SerializeField] private List<GameObject> neighboringVillageSlots = new List<GameObject>();

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

    public List<GameObject> getNeighboringVillageSlots() {
        return neighboringVillageSlots;
    }
}
