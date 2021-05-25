using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardGenerator : MonoBehaviour
{
    public GameObject robber;
    
    public GameObject[] hexagons;

    // public int[,] gameboardConfig =
    // {
    //     {0, 0, 4, 1, 4, 1, 0},
    //     { 0, 1, 2, 2, 2, 4, 0},
    //     {0, 4, 2, 2, 2, 2, 1},
    //     { 1, 2, 2, 3, 2, 2, 4},
    //     {0, 4, 2, 2, 2, 2, 1},
    //     { 0, 1, 2, 2, 2, 4, 0},
    //     {0, 0, 4, 1, 4, 1, 0},
    // };

    //public GameObject hexagonPrefabDesert;
    private string prefabPathDesert = "Assets/Prefabs/Tiles3D/desert.prefab"; 
    //public GameObject hexagonPrefabBrick;
    private string prefabPathBrick = "Assets/Prefabs/Tiles3D/brick.prefab";
    //public GameObject hexagonPrefabOre;
    private string prefabPathOre = "Assets/Prefabs/Tiles3D/ore.prefab";
    //public GameObject hexagonPrefabSheep;
    private string prefabPathSheep = "Assets/Prefabs/Tiles3D/sheep.prefab";
    //public GameObject hexagonPrefabWheat;
    private string prefabPathWheat = "Assets/Prefabs/Tiles3D/wheat.prefab";
    //public GameObject hexagonPrefabWood;
    private string prefabPathWood = "Assets/Prefabs/Tiles3D/wood.prefab";
    
    //public GameObject hexagonPrefabWater;
    private string prefabPathWater = "Assets/Prefabs/Tiles3D/water.prefab";
    //public GameObject hexagonPrefabRandomPort;
    private string prefabPathRandomPort = "Assets/Prefabs/Tiles3D/randomPort.prefab";
    //public GameObject hexagonPrefabBrickPort;
    private string prefabPathBrickPort = "Assets/Prefabs/Tiles3D/brickPort.prefab";
    //public GameObject hexagonPrefabOrePort;
    private string prefabPathOrePort = "Assets/Prefabs/Tiles3D/orePort.prefab";
    //public GameObject hexagonPrefabSheepPort;
    private string prefabPathSheepPort = "Assets/Prefabs/Tiles3D/sheepPort.prefab";
    //public GameObject hexagonPrefabWheatPort;
    private string prefabPathWheatPort = "Assets/Prefabs/Tiles3D/wheatPort.prefab";
    //public GameObject hexagonPrefabWoodPort;
    private string prefabPathWoodPort = "Assets/Prefabs/Tiles3D/woodPort.prefab";
    
    // private Stack<GameObject> randomHexStack;
    // private Stack<int> randomNumStack;
    // private Stack<GameObject> randomPortHexStack;

    private const float r = 0.866f;
    private const float a = 1f;
    private const float d = 2f;
    private const float s = 1.73205f;
    private const float offset = 3f / 2f * a;

    void Start()
    {
        
    }
    
    public void instantiateGameBoard(Hexagon[][] gameBoard)
    {
        Debug.Log("instantiateGameBoard called");

        for (int row = 0; row < gameBoard.Length; row++)
        {
            for (int col = 0; col < gameBoard[row].Length; col++)
            {
                Hexagon hexagon = gameBoard[row][col];
                string hexagonPath = "Tiles3D/" + findPrefabByHexagonType(hexagon.getType());
                Debug.Log(hexagonPath);
                GameObject prefab = (GameObject) Resources.Load(hexagonPath);

                int offsetX = row switch
                {
                    0 => 2,
                    1 => 1,
                    2 => 1,
                    3 => 0,
                    4 => 1,
                    5 => 1,
                    6 => 2,
                    _ => 0,
                };
                
                GameObject newHexagon;
                if (row % 2 == 0)
                {
                    newHexagon = Instantiate(prefab, new Vector3(col * s + offsetX * s, 0, row / 2f * (d + a)), Rotation(hexagon, row, col));
                }
                else
                {
                    newHexagon = Instantiate(prefab, new Vector3(col * s + r + offsetX * s, 0, (row - 1) / 2f * (d + a) + offset), Rotation(hexagon, row, col));
                }
            }
        }
    }
    
    private Quaternion Rotation(Hexagon hexagon, int row, int col)
    {
        //Ports need to be rotated
        if (hexagon.isPort())
        {
            switch (z: row, x: col)
            {
                case (0,0): return Quaternion.Euler(0,-120,0);
                case (0,2): return Quaternion.Euler(0,180,0);
                case (1,4): return Quaternion.Euler(0,180,0);
                case (2,0): return Quaternion.Euler(0,-60,0);
                case (3,6): return Quaternion.Euler(0,120,0);
                case (4,0): return Quaternion.Euler(0,-60,0);
                case (5,4): return Quaternion.Euler(0,60,0);
                case (6,0): return Quaternion.Euler(0,0,0);
                case (6,2): return Quaternion.Euler(0,60,0);
            }
            
        }
        return Quaternion.identity;
    }
    
    private string findPrefabByHexagonType(HEXAGON_TYPE hexagonType)
    {
        switch (hexagonType)
        {
            case HEXAGON_TYPE.WATER: return "water";
            case HEXAGON_TYPE.DESERT: return "desert";
            
            case HEXAGON_TYPE.SHEEP: return "sheep";
            case HEXAGON_TYPE.WOOD: return "sheep";
            case HEXAGON_TYPE.BRICK: return "brick";
            case HEXAGON_TYPE.ORE: return "ore";
            case HEXAGON_TYPE.WHEAT: return "wheat";
            
            case HEXAGON_TYPE.PORTNORMAL: return "randomPort";
            case HEXAGON_TYPE.PORTSHEEP: return "sheepPort";
            case HEXAGON_TYPE.PORTWOOD: return "woodPort";
            case HEXAGON_TYPE.PORTBRICK: return "brickPort";
            case HEXAGON_TYPE.PORTORE: return "orePort";
            case HEXAGON_TYPE.PORTWHEAT: return "wheatPort";
            
            default: //Debug.LogError("BoardGenerator: no HexagonType set in current Hexagon");
                return null;
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     initalizeRandomHexagons();
    //     initalizeRandomPortHexagons();
    //     initializeRandomNumbers();
    //     
    //
    //     for (int z = 0; z < gameboardConfig.GetLength(0); z++)
    //     {
    //         for (int x = 0; x < gameboardConfig.GetLength(1); x++)
    //         {
    //             int currentConfig = gameboardConfig[z, x];
    //             Debug.Log("Hello World z:" + z + " x:" + x + " config:" + currentConfig);
    //             GameObject whatHexagon = currentConfig switch
    //             {
    //                 0 => null,
    //                 1 => hexagonPrefabWater,
    //                 2 => getNextRandomHexagon(),
    //                 3 => hexagonPrefabDesert,
    //                 4 => getNextRandomPortHexagon(),
    //                 _ => null,
    //             };
    //             if (whatHexagon == null) {
    //                 continue;
    //             }
    //
    //             GameObject newHexagon;
    //             
    //
    //             if (z % 2 == 0)
    //             {
    //                 newHexagon = Instantiate(whatHexagon, new Vector3(x * s, 0, z / 2f * (d + a)), Rotation(currentConfig, z, x));
    //             }
    //             else
    //             {
    //                 newHexagon = Instantiate(whatHexagon, new Vector3(x * s + r, 0, (z - 1) / 2f * (d + a) + offset), Rotation(currentConfig, z, x));
    //             }
    //
    //
    //             newHexagon.transform.parent = Board.transform;
    //             
    //             // change hexagon numbers
    //             if (currentConfig == 2) {
    //                 GameObject text = newHexagon.transform.GetChild(1).GetChild(0).gameObject;
    //
    //                 text.GetComponent<TextMesh>().text = getNextRandomNumber().ToString();
    //             }
    //             
    //             //get position of desert
    //             if (whatHexagon == hexagonPrefabDesert)
    //             {
    //                 whatHexagon.transform.position = newHexagon.transform.position;
    //             }
    //             
    //         }
    //     }
    //     
    //     //robber
    //     Instantiate(robber, hexagonPrefabDesert.transform.position, Quaternion.identity);
    // }

    // void initalizeRandomHexagons()
    // {
    //     GameObject[] randomHexArray = new[]
    //     {
    //         hexagonPrefabBrick, hexagonPrefabBrick, hexagonPrefabBrick,
    //         hexagonPrefabOre, hexagonPrefabOre, hexagonPrefabOre,
    //         hexagonPrefabSheep, hexagonPrefabSheep, hexagonPrefabSheep, hexagonPrefabSheep,
    //         hexagonPrefabWheat, hexagonPrefabWheat, hexagonPrefabWheat, hexagonPrefabWheat,
    //         hexagonPrefabWood, hexagonPrefabWood, hexagonPrefabWood, hexagonPrefabWood,
    //     };
    //
    //     randomHexStack = new Stack<GameObject>(randomHexArray.OrderBy(n => Guid.NewGuid()).ToArray());
    // }
    //
    // void initalizeRandomPortHexagons()
    // {
    //     GameObject[] randomHexArray = new[]
    //     {
    //         hexagonPrefabRandomPort, hexagonPrefabRandomPort, hexagonPrefabRandomPort, hexagonPrefabRandomPort,
    //         hexagonPrefabBrickPort,
    //         hexagonPrefabOrePort,
    //         hexagonPrefabSheepPort,
    //         hexagonPrefabWheatPort,
    //         hexagonPrefabWoodPort
    //     };
    //
    //     randomPortHexStack = new Stack<GameObject>(randomHexArray.OrderBy(n => Guid.NewGuid()).ToArray());
    // }
    //
    // void initializeRandomNumbers()
    // {
    //     int[] randomNumArray = new[] {2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12};
    //
    //     randomNumStack = new Stack<int>(randomNumArray.OrderBy(n => Guid.NewGuid()).ToArray());
    // }
    //
    // private GameObject getNextRandomHexagon()
    // {
    //     return randomHexStack.Pop();
    // }
    //
    // private GameObject getNextRandomPortHexagon()
    // {
    //     return randomPortHexStack.Pop();
    // }
    //
    // private int getNextRandomNumber()
    // {
    //     return randomNumStack.Pop();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    // }
}