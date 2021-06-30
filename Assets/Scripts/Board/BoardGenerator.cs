using Enums;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject robber;

    private const float R = 0.866f;
    private const float A = 1f;
    private const float D = 2f;
    private const float S = 1.73205f;
    private const float Offset = 3f / 2f * A;
    
    private readonly GameObject prefabWater = (GameObject) Resources.Load("Tiles3D/waterPrefab");
    private readonly GameObject prefabDesert = (GameObject) Resources.Load("Tiles3D/desertPrefab");
    
    private readonly GameObject prefabSheep = (GameObject) Resources.Load("Tiles3D/sheepPrefab");
    private readonly GameObject prefabWood = (GameObject) Resources.Load("Tiles3D/woodPrefab");
    private readonly GameObject prefabBrick = (GameObject) Resources.Load("Tiles3D/brickPrefab");
    private readonly GameObject prefabOre = (GameObject) Resources.Load("Tiles3D/orePrefab");
    private readonly GameObject prefabWheat = (GameObject) Resources.Load("Tiles3D/wheatPrefab");
    
    private readonly GameObject prefabRandomPort = (GameObject) Resources.Load("Tiles3D/randomPortPrefab");
    private readonly GameObject prefabSheepPort = (GameObject) Resources.Load("Tiles3D/sheepPortPrefab");
    private readonly GameObject prefabWoodPort = (GameObject) Resources.Load("Tiles3D/woodPortPrefab");
    private readonly GameObject prefabBrickPort = (GameObject) Resources.Load("Tiles3D/brickPortPrefab");
    private readonly GameObject prefabOrePort = (GameObject) Resources.Load("Tiles3D/orePortPrefab");
    private readonly GameObject prefabWheatPort = (GameObject) Resources.Load("Tiles3D/wheatPortPrefab");

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="gameBoard"></param>
    public void instantiateGameBoard(Hexagon[][] gameBoard)
    {
        for (int row = 0; row < gameBoard.Length; row++)
        {
            for (int col = 0; col < gameBoard[row].Length; col++)
            {
                if (gameBoard[row][col] == null)
                {
                    continue;
                }
                
                Hexagon hexagon = gameBoard[row][col];
                GameObject prefab = findPrefabByHexagonType(hexagon.getType());

                int offsetX = row switch
                {
                    0 => -1,
                    1 => -1,
                    2 => 0,
                    3 => 0,
                    4 => 1,
                    5 => 1,
                    6 => 2,
                    _ => 0,
                };

                GameObject newHexagon;
                if (row % 2 == 0)
                {
                    newHexagon = Instantiate(prefab, new Vector3(col * S + offsetX * S, 0, -(row / 2f * (D + A)) + 9), Rotation(hexagon, row, col));
                }
                else
                {
                    newHexagon = Instantiate(prefab, new Vector3(col * S + R + offsetX * S, 0, -((row - 1) / 2f * (D + A) + Offset) + 9), Rotation(hexagon, row, col));
                }

                // change hexagon numbers
                if (hexagon.isLand())
                {
                    GameObject text = newHexagon.transform.GetChild(1).GetChild(0).gameObject;

                    text.GetComponent<TextMesh>().text = hexagon.getFieldNumber().ToString();
                }

                //Instantiate Robber
                if (row == 3 && col == 3)
                {
                    robber = (GameObject)Resources.Load("Moving objects/robberPrefab");
                    Instantiate(robber, newHexagon.transform.position, Quaternion.identity);
                }
            }
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="hexagon"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private Quaternion Rotation(Hexagon hexagon, int row, int col)
    {
        //Ports need to be rotated
        if (hexagon.isPort())
        {
            switch (z: row, x: col)
            {
                case (0,3): return Quaternion.Euler(0,0,0);
                case (0,5): return Quaternion.Euler(0,60,0);
                case (1,6): return Quaternion.Euler(0,60,0);
                case (2,1): return Quaternion.Euler(0,-60,0);
                case (3,6): return Quaternion.Euler(0,120,0);
                case (4,0): return Quaternion.Euler(0,-60,0);
                case (5,4): return Quaternion.Euler(0,180,0);
                case (6,0): return Quaternion.Euler(0,-120,0);
                case (6,2): return Quaternion.Euler(0,180,0);
            }

        }
        return Quaternion.identity;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="hexagonType"></param>
    /// <returns></returns>
    private GameObject findPrefabByHexagonType(HEXAGON_TYPE hexagonType)
    {
        switch (hexagonType)
        {
            case HEXAGON_TYPE.WATER: return prefabWater;
            case HEXAGON_TYPE.DESERT: return prefabDesert;

            case HEXAGON_TYPE.SHEEP: return prefabSheep;
            case HEXAGON_TYPE.WOOD: return prefabWood;
            case HEXAGON_TYPE.BRICK: return prefabBrick;
            case HEXAGON_TYPE.ORE: return prefabOre;
            case HEXAGON_TYPE.WHEAT: return prefabWheat;

            case HEXAGON_TYPE.PORTNORMAL: return prefabRandomPort;
            case HEXAGON_TYPE.PORTSHEEP: return prefabSheepPort;
            case HEXAGON_TYPE.PORTWOOD: return prefabWoodPort;
            case HEXAGON_TYPE.PORTBRICK: return prefabBrickPort;
            case HEXAGON_TYPE.PORTORE: return prefabOrePort;
            case HEXAGON_TYPE.PORTWHEAT: return prefabWheatPort;

            default: //Debug.LogError("BoardGenerator: no HexagonType set in current Hexagon");
                return null;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="buildType"></param>
    /// <param name="buildId"></param>
    /// <param name="buildColor"></param>
    public void placeBuilding(BUYABLES buildType, int buildId, PLAYERCOLOR buildColor)
    {
        switch (buildType)
        {
            case BUYABLES.VILLAGE:
            {
                Debug.LogWarning($"CLIENT: Buyable: Village, Build ID: {buildId}");
                GameObject node = GameObject.Find("V" + buildId.ToString());
                Vector3 nodePos = node.transform.position;

                if (buildVillage(buildId, nodePos, buildColor))
                {
                    Destroy(node);
                }

                break;
            }
            case BUYABLES.CITY:
            {
                Debug.Log("CLIENT: Buyable: City");
                GameObject village = GameObject.Find("C" + buildId.ToString());
                Vector3 villagePos = village.transform.position;
            
                if (buildCity(villagePos, buildColor))
                {
                    Destroy(village);
                }

                break;
            }
            case BUYABLES.ROAD:
            {
                Debug.Log("CLIENT: Buyable: Road");
                GameObject edge = GameObject.Find("R" + buildId.ToString());
                Vector3 edgePos = edge.transform.position;
                Quaternion edgeRotation = edge.transform.rotation;

                if (BuildRoad(edgePos, edgeRotation, buildColor))
                {
                    Destroy(edge);
                }

                break;
            }
        }
    }
    
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="buildId"></param>
    /// <param name="position"></param>
    /// <param name="buildColor"></param>
    /// <returns></returns>
    private bool buildVillage(int buildId, Vector3 position, PLAYERCOLOR buildColor)
    {
        GameObject prefab;
        switch (buildColor)
        {
            case PLAYERCOLOR.RED: prefab = (GameObject) Resources.Load("PlayerObjects3D/redVillagePrefab"); break;
            case PLAYERCOLOR.BLUE: prefab = (GameObject) Resources.Load("PlayerObjects3D/blueVillagePrefab"); break;
            case PLAYERCOLOR.WHITE: prefab = (GameObject) Resources.Load("PlayerObjects3D/whiteVillagePrefab"); break;
            case PLAYERCOLOR.YELLOW: prefab = (GameObject) Resources.Load("PlayerObjects3D/yellowVillagePrefab"); break;
            default: Debug.Log("CLIENT: buildVillage: wrong player color"); return false;
        }

        position = new Vector3 (position.x, 0.06f, position.z);
        GameObject newVillage = Instantiate(prefab, position, Quaternion.identity);
        newVillage.transform.name = "C" + buildId.ToString();
        return true;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="position"></param>
    /// <param name="buildColor"></param>
    /// <returns></returns>
    private bool buildCity(Vector3 position, PLAYERCOLOR buildColor)
    {
        GameObject prefab;
        switch (buildColor)
        {
            case PLAYERCOLOR.RED: prefab = (GameObject) Resources.Load("PlayerObjects3D/redCityPrefab"); break;
            case PLAYERCOLOR.BLUE: prefab = (GameObject) Resources.Load("PlayerObjects3D/blueCityPrefab"); break;
            case PLAYERCOLOR.WHITE: prefab = (GameObject) Resources.Load("PlayerObjects3D/whiteCityPrefab"); break;
            case PLAYERCOLOR.YELLOW: prefab = (GameObject) Resources.Load("PlayerObjects3D/yellowCityPrefab"); break;
            default: Debug.Log("CLIENT: buildCity: wrong player color"); return false;
        }
        
        position = new Vector3 (position.x, 0.09f, position.z);
        Instantiate(prefab, position, Quaternion.identity);
        return true;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="buildColor"></param>
    /// <returns></returns>
    private bool BuildRoad(Vector3 position, Quaternion rotation, PLAYERCOLOR buildColor)
    {
        GameObject prefab;
        switch (buildColor)
        {
            case PLAYERCOLOR.RED: prefab = (GameObject) Resources.Load("PlayerObjects3D/redStreetPrefab"); break;
            case PLAYERCOLOR.BLUE: prefab = (GameObject) Resources.Load("PlayerObjects3D/blueStreetPrefab"); break;
            case PLAYERCOLOR.WHITE: prefab = (GameObject) Resources.Load("PlayerObjects3D/whiteStreetPrefab"); break;
            case PLAYERCOLOR.YELLOW: prefab = (GameObject) Resources.Load("PlayerObjects3D/yellowStreetPrefab"); break;
            default: Debug.Log("CLIENT: buildCity: wrong player color"); return false;
        }
        
        position = new Vector3 (position.x, 0.08f, position.z);
        Instantiate(prefab, position, rotation);
        return true;
    }
}