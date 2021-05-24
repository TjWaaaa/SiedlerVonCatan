using System;
using Enums;
using UnityEngine;

namespace UI
{
    public class PlayerRepresentation : MonoBehaviour
    {
        //private PrefabFactory prefabFactory;
        private GameObject allPlayers;
        
        
        public void Start()
        {
            allPlayers = GameObject.Find("AllPlayers");
            
            
            
            
        }
    }
}