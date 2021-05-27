using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    // Board stuff
    
    // other stuff
    public GameObject PlayerRepresentation;
    // Lobby stuff
    public GameObject PlayerListItem;
    
    void Start()
    {
       DontDestroyOnLoad(this); 
    }

    public GameObject getPrefab(PREFABS prefabType, Transform location)
    {
        switch (prefabType) 
        {
            case(PREFABS.PLAYER_LIST_ITEM):
                return Instantiate(PlayerListItem, location);
            case(PREFABS.PLAYER_REPRESENTATION):
                return Instantiate(PlayerRepresentation, location);
            default: 
                return null;
        }
    }
}
