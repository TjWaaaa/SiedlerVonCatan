using System;
using System.Collections;
using System.Collections.Generic;
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
        
        buildStreetButton.GetComponent<Button>().onClick.AddListener(changeBuildStreetMode);
        buildVillageButton.GetComponent<Button>().onClick.AddListener(changeBuildVillageMode);
        buildCityButton.GetComponent<Button>().onClick.AddListener(changeBuildCityMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void changeBuildStreetMode()
    {
        if (buildStreetMode)
        {
            buildStreetMode = false;
            Debug.Log("BUILDSTREETMODE IS OFF");
        }
        else
        {
            if (!buildVillageMode && !buildCityMode)
            {
                buildStreetMode = true;
                Debug.Log("BUILDSTREETMODE IS ON");
            }
        }
    }
    
    private static void changeBuildVillageMode()
    {
        if (buildVillageMode)
        {
            buildVillageMode = false;
            Debug.Log("BUILDVILLAGEMODE IS OFF");
        }
        else
        {
            if (!buildStreetMode && !buildCityMode)
            {
                buildVillageMode = true;
                Debug.Log("BUILDVILLAGEMODE IS ON");
            }
        }
    }
    
    private static void changeBuildCityMode()
    {
        if (buildCityMode)
        {
            buildCityMode = false;
            Debug.Log("BUILDCITYMODE IS OFF");
        }
        else
        {
            if (!buildVillageMode && !buildStreetMode)
            {
                buildCityMode = true;
                Debug.Log("BUILDCITYMODE IS ON");
            }
        }
    }
}
