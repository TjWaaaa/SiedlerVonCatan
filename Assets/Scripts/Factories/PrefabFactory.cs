using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    // Board stuff
    
    // other stuff
    
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
            case(PREFABS.PlayerListItem):
                return Instantiate(PlayerListItem, location);

            default: 
                return null;
        }
    }
}
