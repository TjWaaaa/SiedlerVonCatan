using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardGenerator : MonoBehaviour
{

    public GameObject robber;
    
    public GameObject[] hexagons;

    public int[,] gameboardConfig =
    {
        {0, 0, 4, 1, 4, 1, 0},
        { 0, 1, 2, 2, 2, 4, 0},
        {0, 4, 2, 2, 2, 2, 1},
        { 1, 2, 2, 3, 2, 2, 4},
        {0, 4, 2, 2, 2, 2, 1},
        { 0, 1, 2, 2, 2, 4, 0},
        {0, 0, 4, 1, 4, 1, 0},
    };
    
    public GameObject hexagonPrefabDesert;
    public GameObject hexagonPrefabBrick;
    public GameObject hexagonPrefabOre;
    public GameObject hexagonPrefabSheep;
    public GameObject hexagonPrefabWheat;
    public GameObject hexagonPrefabWood;
    
    public GameObject hexagonPrefabWater;
    public GameObject hexagonPrefabRandomPort;
    public GameObject hexagonPrefabBrickPort;
    public GameObject hexagonPrefabOrePort;
    public GameObject hexagonPrefabSheepPort;
    public GameObject hexagonPrefabWheatPort;
    public GameObject hexagonPrefabWoodPort;

    private Stack<GameObject> randomHexStack;
    private Stack<int> randomNumStack;
    private Stack<GameObject> randomPortHexStack;

    private const float r = 0.866f;
    private const float a = 1f;
    private const float d = 2f;
    private const float s = 1.73205f;
    private const float offset = 3f / 2f * a;


    // Start is called before the first frame update
    void Start()
    {
        initalizeRandomHexagons();
        initalizeRandomPortHexagons();
        initializeRandomNumbers();
        

        for (int z = 0; z < gameboardConfig.GetLength(0); z++)
        {
            for (int x = 0; x < gameboardConfig.GetLength(1); x++)
            {
                int currentConfig = gameboardConfig[z, x];
                Debug.Log("Hello World z:" + z + " x:" + x + " config:" + currentConfig);
                GameObject whatHexagon = currentConfig switch
                {
                    0 => null,
                    1 => hexagonPrefabWater,
                    2 => getNextRandomHexagon(),
                    3 => hexagonPrefabDesert,
                    4 => getNextRandomPortHexagon(),
                    _ => null,
                };
                if (whatHexagon == null) {
                    continue;
                }

                GameObject newHexagon;
                

                if (z % 2 == 0)
                {
                    newHexagon = Instantiate(whatHexagon, new Vector3(x * s, 0, z / 2f * (d + a)), Rotation(currentConfig, z, x));
                }
                else
                {
                    newHexagon = Instantiate(whatHexagon, new Vector3(x * s + r, 0, (z - 1) / 2f * (d + a) + offset), Rotation(currentConfig, z, x));
                }
                
                
                // change hexagon numbers
                if (currentConfig == 2) {
                    GameObject text = newHexagon.transform.GetChild(1).GetChild(0).gameObject;

                    text.GetComponent<TextMesh>().text = getNextRandomNumber().ToString();
                }
                
                //get position of desert
                if (whatHexagon == hexagonPrefabDesert)
                {
                    whatHexagon.transform.position = newHexagon.transform.position;
                }
                
            }
        }
        
        //robber
        Instantiate(robber, hexagonPrefabDesert.transform.position, Quaternion.identity);
    }

    Quaternion Rotation(int currentConfig, int z, int x)
    {
        //Ports need to be rotated
        if (currentConfig == 4)
        {
            switch (z, x)
            {
                case (0,2): return Quaternion.Euler(0,120,0);
                case (0,4): return Quaternion.Euler(0,60,0);
                case (2,1): return Quaternion.Euler(0,180,0);
                case (4,1):
                case (6,2): 
                    return Quaternion.Euler(0,-120,0);
                case (6,4): 
                case (5,5):    
                    return Quaternion.Euler(0,-60,0);
            }
            
        }
        return Quaternion.identity;
    }

    void initalizeRandomHexagons()
    {
        GameObject[] randomHexArray = new[]
        {
            hexagonPrefabBrick, hexagonPrefabBrick, hexagonPrefabBrick,
            hexagonPrefabOre, hexagonPrefabOre, hexagonPrefabOre,
            hexagonPrefabSheep, hexagonPrefabSheep, hexagonPrefabSheep, hexagonPrefabSheep,
            hexagonPrefabWheat, hexagonPrefabWheat, hexagonPrefabWheat, hexagonPrefabWheat,
            hexagonPrefabWood, hexagonPrefabWood, hexagonPrefabWood, hexagonPrefabWood,
        };

        randomHexStack = new Stack<GameObject>(randomHexArray.OrderBy(n => Guid.NewGuid()).ToArray());
    }
    
    void initalizeRandomPortHexagons()
    {
        GameObject[] randomHexArray = new[]
        {
            hexagonPrefabRandomPort, hexagonPrefabRandomPort, hexagonPrefabRandomPort, hexagonPrefabRandomPort,
            hexagonPrefabBrickPort,
            hexagonPrefabOrePort,
            hexagonPrefabSheepPort,
            hexagonPrefabWheatPort,
            hexagonPrefabWoodPort
        };

        randomPortHexStack = new Stack<GameObject>(randomHexArray.OrderBy(n => Guid.NewGuid()).ToArray());
    }

    void initializeRandomNumbers()
    {
        int[] randomNumArray = new[] {2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12};

        randomNumStack = new Stack<int>(randomNumArray.OrderBy(n => Guid.NewGuid()).ToArray());
    }
    
    private GameObject getNextRandomHexagon()
    {
        return randomHexStack.Pop();
    }
    
    private GameObject getNextRandomPortHexagon()
    {
        return randomPortHexStack.Pop();
    }

    private int getNextRandomNumber()
    {
        return randomNumStack.Pop();
    }

    // Update is called once per frame
    void Update()
    {
    }
}