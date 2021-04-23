using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    [SerializeField] private List<GameObject> neighboringVillageSlots = new List<GameObject>();
    [SerializeField] private List<GameObject> neighboringRoadSlots = new List<GameObject>();
    [SerializeField] private List<GameObject> neighboringHexagons = new List<GameObject>();


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

    public void setNeighboringRoadSlots(List<GameObject> neighbors) {
        neighboringRoadSlots = neighbors;
    }

    public void setNeighboringHexagons(List<GameObject> neighbors) {
        neighboringHexagons = neighbors;
    }

    public void addNeighboringHexagon(GameObject neighbor) {
        neighboringHexagons.Add(neighbor);
    }

    public List<GameObject> getNeighboringVillageSlots() {
        return neighboringVillageSlots;
    }

    public List<GameObject> getNeighbotingRoadSlots() {
        return neighboringRoadSlots;
    }

    public List<GameObject> getNeighboringHexagons() {
        return neighboringHexagons;
    }
}

