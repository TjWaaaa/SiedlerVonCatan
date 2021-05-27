using System;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private ClientRequest clientRequest = new ClientRequest();

    private bool buildStreetMode = false;
    private bool buildVillageMode = false;
    private bool buildCityMode = false;

    private GameObject buildStreetButton;
    private GameObject buildVillageButton;
    private GameObject buildCityButton;
    
    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        
        buildStreetButton = GameObject.Find("buildStreet");
        buildVillageButton = GameObject.Find("buildVillage");
        buildCityButton = GameObject.Find("buildCity");
        
        buildStreetButton.GetComponent<Button>().onClick.AddListener(startBuildStreetMode);
        buildVillageButton.GetComponent<Button>().onClick.AddListener(startBuildVillageMode);
        buildCityButton.GetComponent<Button>().onClick.AddListener(startBuildCityMode);
    }

    // Update is called once per frame
    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (buildVillageMode)
        {
            if (Input.GetMouseButtonDown(0)
                && Physics.Raycast(ray, out hit, 100f)
                && hit.collider.tag == "VillageSlot")
            {
                Debug.Log("Client: want to build a village");
                int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                clientRequest.requestBuild(BUYABLES.VILLAGE, posInArray);
                stopBuildMode();
            }
        }
        else if (buildCityMode)
        {
            if (Input.GetMouseButtonDown(0)
                && Physics.Raycast(ray, out hit, 100f)
                && hit.collider.tag == "Village")
            {
                Debug.Log("Client: want to build a city");
                int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                clientRequest.requestBuild(BUYABLES.CITY, posInArray);
                stopBuildMode();
            }
        }
        else if (buildStreetMode)
        {
            if (Input.GetMouseButtonDown(0)
                && Physics.Raycast(ray, out hit, 100f)
                && hit.collider.tag == "RoadSlot")
            {
                Debug.Log("Client: want to build a road");
                int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                clientRequest.requestBuild(BUYABLES.ROAD, posInArray);
                stopBuildMode();
            }
        }
    }

    private void stopBuildMode()
    {
        buildStreetMode = false;
        buildVillageMode = false;
        buildCityMode = false;
        Debug.Log("BUILDMODE IS OFF");
    }
    
    private void startBuildStreetMode()
    {
        buildStreetMode = true;
        buildCityMode = false;
        buildVillageMode = false;
        Debug.Log("BUILDSTREETMODE IS ON");
    }
    
    private void startBuildVillageMode()
    {
        buildVillageMode = true;
        buildStreetMode = false;
        buildCityMode = false;
        Debug.Log("BUILDVILLAGEMODE IS ON");
    }
    
    private void startBuildCityMode()
    {
        buildCityMode = true;
        buildVillageMode = false;
        buildStreetMode = false;
        Debug.Log("BUILDCITYMODE IS ON");
    }
}
