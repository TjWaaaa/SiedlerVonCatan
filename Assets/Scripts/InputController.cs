using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public static Boolean buildStreetMode = false;
    public static  Boolean buildVillageMode = false;
    public static Boolean buildCityMode = false;
    
    private GameObject buildStreetButton;
    private GameObject buildVillageButton;
    private GameObject buildCityButton;
    
    // Start is called before the first frame update
    void Start()
    {
        buildStreetButton = GameObject.Find("buildStreet");
        buildVillageButton = GameObject.Find("buildVillage");
        buildCityButton = GameObject.Find("buildCity");
        
        buildStreetButton.GetComponent<Button>().onClick.AddListener(startBuildStreetMode);
        buildVillageButton.GetComponent<Button>().onClick.AddListener(startBuildVillageMode);
        buildCityButton.GetComponent<Button>().onClick.AddListener(startBuildCityMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void stopBuildMode()
    {
        buildStreetMode = false;
        buildVillageMode = false;
        buildCityMode = false;
    }
    
    private static void startBuildStreetMode()
    {
        buildStreetMode = true;
        buildCityMode = false;
        buildVillageMode = false;
        Debug.Log("BUILDSTREETMODE IS ON");
         
    }
    
    private static void startBuildVillageMode()
    {
        buildVillageMode = true;
        buildStreetMode = false;
        buildCityMode = false;
        Debug.Log("BUILDVILLAGEMODE IS ON");

    }
    
    private static void startBuildCityMode()
    {
        buildCityMode = true;
        buildVillageMode = false;
        buildStreetMode = false;
        Debug.Log("BUILDCITYMODE IS ON");
        
    }
}
