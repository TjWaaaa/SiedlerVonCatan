using System;
using Enums;
using Networking.Communication;
using UnityEngine;
using UnityEngine.UI;


public class BuildController : MonoBehaviour
{

    private ClientRequest clientRequest = new ClientRequest();

    private Camera mainCamera;

    private GameObject buildStreetButton;
    private GameObject buildVillageButton;
    private GameObject buildCityButton;

    private bool buildStreetMode;
    private bool buildVillageMode;
    private bool buildCityMode;


    // Start is called before the first frame update
    private void Start()
    {
        // Camera
        mainCamera = Camera.main;

        // Find buttons and add event listener
        buildStreetButton = GameObject.Find("buildStreet");
        buildVillageButton = GameObject.Find("buildVillage");
        buildCityButton = GameObject.Find("buildCity");
        buildStreetButton.GetComponent<Button>().onClick.AddListener(startBuildStreetMode);
        buildVillageButton.GetComponent<Button>().onClick.AddListener(startBuildVillageMode);
        buildCityButton.GetComponent<Button>().onClick.AddListener(startBuildCityMode);

    }

    /// <summary>
    /// Player sends a buildrequest, if the corresponding button is clicked and the raycast hits a collider
    /// </summary>
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
        Debug.Log("build mode is off");
    }

    private void startBuildStreetMode()
    {
        buildStreetMode = true;
        buildCityMode = false;
        buildVillageMode = false;
        Debug.Log("build street mode is on");
    }

    private void startBuildVillageMode()
    {
        buildVillageMode = true;
        buildStreetMode = false;
        buildCityMode = false;
        Debug.Log("build village mode is on");
    }

    private void startBuildCityMode()
    {
        buildVillageMode = false;
        buildStreetMode = false;
        buildCityMode = true;
        Debug.Log("build city mode is on");
    }








}
