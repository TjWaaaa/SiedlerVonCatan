using System;
using Enums;
using UnityEngine;

namespace UI
{
    public class PlayerRepresentation : MonoBehaviour
    {
        private PrefabFactory prefabFactory;
        private GameObject allPlayers;
        
        GameObject playerRepresentation_2;
        
        public void Start()
        {
            allPlayers = GameObject.Find("AllPlayers");
            //GameObject playerRepresentation_1 = playerRepresentation_1 = prefabFactory.getPrefab(PREFABS.PLAYER_REPRESENTATION, allPlayers.transform);
            
            
            
        }
    }
}