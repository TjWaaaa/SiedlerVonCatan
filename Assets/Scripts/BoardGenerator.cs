using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject[] hexagons;

    public int[,] gameboardConfig =
    {
        {0, 0, 1, 1, 1, 1, 0},
        { 0, 1, 2, 2, 2, 1, 0},
        {0, 1, 2, 2, 2, 2, 1},
        { 1, 2, 2, 3, 2, 2, 1},
        {0, 1, 2, 2, 2, 2, 1},
        { 0, 1, 2, 2, 2, 1, 0},
        {0, 0, 1, 1, 1, 1, 0},
    };

    public GameObject hexagonPrefabWater;
    public GameObject hexagonPrefabDesert;
    public GameObject hexagonPrefabBrick;
    public GameObject hexagonPrefabOre;
    public GameObject hexagonPrefabSheep;
    public GameObject hexagonPrefabWheet;
    public GameObject hexagonPrefabWood;

    private Stack<GameObject> randomHexStack;
    private Stack<int> randomNumStack;

    private const float r = 0.866f;
    private const float a = 1f;
    private const float d = 2f;
    private const float s = 1.73205f;
    private const float offset = 3f / 2f * a;


    // Start is called before the first frame update
    void Start()
    {
        initalizeRandomHexagons();
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
                    _ => null,
                };
                if (whatHexagon == null) {
                    continue;
                }

                GameObject newHexagon;

                if (z % 2 == 0)
                {
                    newHexagon = Instantiate(whatHexagon, new Vector3(x * s, 0, z / 2f * (d + a)), Quaternion.identity);
                }
                else
                {
                    newHexagon = Instantiate(whatHexagon, new Vector3(x * s + r, 0, (z - 1) / 2f * (d + a) + offset), Quaternion.identity);
                }
                
                // change hexagon numbers
                if (currentConfig == 2) {
                    GameObject text = newHexagon.transform.GetChild(1).GetChild(0).gameObject;

                    text.GetComponent<TextMesh>().text = getNextRandomNumber().ToString();
                }
            }
        }
    }

    void initalizeRandomHexagons()
    {
        GameObject[] randomHexArray = new[]
        {
            hexagonPrefabBrick, hexagonPrefabBrick, hexagonPrefabBrick,
            hexagonPrefabOre, hexagonPrefabOre, hexagonPrefabOre,
            hexagonPrefabSheep, hexagonPrefabSheep, hexagonPrefabSheep, hexagonPrefabSheep,
            hexagonPrefabWheet, hexagonPrefabWheet, hexagonPrefabWheet, hexagonPrefabWheet,
            hexagonPrefabWood, hexagonPrefabWood, hexagonPrefabWood, hexagonPrefabWood,
        };

        randomHexStack = new Stack<GameObject>(randomHexArray.OrderBy(n => Guid.NewGuid()).ToArray());
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

    private int getNextRandomNumber()
    {
        return randomNumStack.Pop();
    }

    // Update is called once per frame
    void Update()
    {
    }
}