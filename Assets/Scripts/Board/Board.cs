using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enums;
using UnityEngine;

public class Board
{
    private Hexagon[][] hexagonsArray;
    private Hexagon[][] hexagonDiceNumbers =
    {
        new Hexagon[0], // 0
        new Hexagon[0], // 1
        new Hexagon[1], // 2
        new Hexagon[2], // 3
        new Hexagon[2], // 4
        new Hexagon[2], // 5
        new Hexagon[2], // 6
        new Hexagon[0], // 7
        new Hexagon[2], // 8
        new Hexagon[2], // 9
        new Hexagon[2], // 10
        new Hexagon[2], // 11
        new Hexagon[1], // 12
    };
    
    private Node[] nodesArray = new Node[54];
    private Edge[] edgesArray = new Edge[72];
    private Stack<int> numberStack;

    private readonly int[][] boardConfig = {
        new[] {0,0,0,4,1,4,1},
        new[]  {0,0,1,2,2,2,4},
        new[]   {0,4,2,2,2,2,1},
        new[]    {1,2,2,3,2,2,4},
        new[]     {4,2,2,2,2,1,0},
        new[]      {1,2,2,2,4,0,0},
        new[]       {4,1,4,1,0,0,0}
    };

    private int[] neighborOffsetX = {  1, 0,-1,-1, 0, 1 }; //specifies the position of adjacent hexagons in horizontal direction
    private int[] neighborOffsetY = { -1,-1, 0, 1, 1, 0 }; //specifies the position of adjacent hexagons in vertical direction
    private int[] availableNumbers = { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };

    private readonly HEXAGON_TYPE[] landHexagons = {
        HEXAGON_TYPE.SHEEP, HEXAGON_TYPE.SHEEP, HEXAGON_TYPE.SHEEP, HEXAGON_TYPE.SHEEP,
        HEXAGON_TYPE.WOOD, HEXAGON_TYPE.WOOD, HEXAGON_TYPE.WOOD, HEXAGON_TYPE.WOOD,
        HEXAGON_TYPE.WHEAT, HEXAGON_TYPE.WHEAT, HEXAGON_TYPE.WHEAT, HEXAGON_TYPE.WHEAT,
        HEXAGON_TYPE.BRICK, HEXAGON_TYPE.BRICK, HEXAGON_TYPE.BRICK,
        HEXAGON_TYPE.ORE, HEXAGON_TYPE.ORE, HEXAGON_TYPE.ORE
    };

    private readonly HEXAGON_TYPE[] portHexagons = {
        HEXAGON_TYPE.PORTNORMAL, HEXAGON_TYPE.PORTNORMAL, HEXAGON_TYPE.PORTNORMAL, HEXAGON_TYPE.PORTNORMAL,
        HEXAGON_TYPE.PORTSHEEP,
        HEXAGON_TYPE.PORTWOOD,
        HEXAGON_TYPE.PORTBRICK,
        HEXAGON_TYPE.PORTORE,
        HEXAGON_TYPE.PORTWHEAT
    };

    private const string configPath = "Assets/Scripts/Board/";

    public Board()
    {
        initializeNodes();
        initializeEdges();
        initializeHexagons();
        checkPlacementConstraints();
        assignNeighborsToHexagons();
        assignNeighborsToNodes();
        assignNeighborsToEdges();
    }
    
    /// <summary>
    /// Contructor for test purposes. Takes a pre defined Numberstack an initalizes a Board Object
    /// </summary>
    /// <param name="numberStack">Stack of int values representing the fieldnumbers of the boards hexagons</param>
    public Board(Stack<int> numberStack)
    {
        this.numberStack = numberStack;
        initializeNodes();
        initializeEdges();
        initializeHexagons();
        checkPlacementConstraints();
    }

    public Hexagon[][] getHexagonsArray()
    {
        return hexagonsArray;
    }
    
    private void initializeNodes()
    {
        for (int i = 0; i < 54; i++)
        {
            nodesArray[i] = new Node();
        }
    }

    private void initializeEdges()
    {
        for (int i = 0; i < 72; i++)
        {
            edgesArray[i] = new Edge(i);
        }
    }

    /// <summary>
    /// creates an array of Hexagons with randomly placed hexagon_Types and hexagonNumbers
    /// </summary>
    /// <returns>returns the array</returns>
    private void initializeHexagons()
    {
        Stack<HEXAGON_TYPE> landStack = createRandomHexagonStack(landHexagons);
        Stack<HEXAGON_TYPE> portStack = createRandomHexagonStack(portHexagons);
        numberStack = createRandomHexagonNumberStack(availableNumbers);

        hexagonsArray = new Hexagon[7][];

        for (int row = 0; row < boardConfig.Length; row++)
        {
            hexagonsArray[row] = new Hexagon[boardConfig[row].Length];
            for (int col = 0; col < boardConfig[row].Length; col++)
            {
                int currentConfig = boardConfig[row][col];

                switch (currentConfig)
                {
                    case 0:
                        break;
                    case 1:
                        hexagonsArray[row][col] = new Hexagon(HEXAGON_TYPE.WATER);
                        break;
                    case 2:
                        int fieldNumber = numberStack.Pop();
                        Hexagon newHexagon = new Hexagon(landStack.Pop(), fieldNumber);
                        addHexagonToDiceNumbersArray(newHexagon, fieldNumber);
                        hexagonsArray[row][col] = newHexagon;
                        break;
                    case 3:
                        hexagonsArray[row][col] = new Hexagon(HEXAGON_TYPE.DESERT);
                        break;
                    case 4:
                        hexagonsArray[row][col] = new Hexagon(portStack.Pop());
                        break;
                }
            }
        }
    }

    /// <summary>
    /// creates a HEXAGON_TYPE stack with types of a given array
    /// </summary>
    /// <param name="array">array of HEXAGON_TYPEs with specific types</param>
    /// <returns>random HEXAGON_TYPE stack</returns>
    private Stack<HEXAGON_TYPE> createRandomHexagonStack(HEXAGON_TYPE[] array)
    {
        return new Stack<HEXAGON_TYPE>(array.OrderBy(n => Guid.NewGuid()).ToArray());
    }

    /// <summary>
    /// creates a int stack with numbers of a given array
    /// </summary>
    /// <param name="array">int array with specific numbers</param>
    /// <returns>random int stack</returns>
    private Stack<int> createRandomHexagonNumberStack(int[] array)
    {
        return new Stack<int>(array.OrderBy(n => Guid.NewGuid()).ToArray());
    }

    /// <summary>
    /// adds Hexagon to empty slot in hexagonDiceNumbers array, because there can only be 1 or 2
    /// Hexagons in each sub array 
    /// </summary>
    /// <param name="newHexagon"></param>
    /// <param name="fieldNumber"></param>
    /// <returns></returns>
    private bool addHexagonToDiceNumbersArray(Hexagon newHexagon, int fieldNumber)
    {
        if (hexagonDiceNumbers[fieldNumber][0] == null)
        {
            hexagonDiceNumbers[fieldNumber][0] = newHexagon;
            return true;
        }
        if (hexagonDiceNumbers[fieldNumber][1] == null)
        {
            hexagonDiceNumbers[fieldNumber][1] = newHexagon;
            return true;
        }

        return false;
    }

    /// <summary>
    /// This function has to get called to place a village or city onto a specific node.
    /// If the player is allowed to place a building on the Node with id 'nodeId', a village
    /// gets built, or gets upgraded into a city
    /// </summary>
    /// <param name="nodeId">position of a node in the nodes[] array</param>
    /// <param name="player">color of the player who tries to build</param>
    public bool canPlaceBuilding(int nodeId, PLAYERCOLOR player, BUILDING_TYPE buildingType, bool preGamePhase)
    {
        Node requestedNode = nodesArray[nodeId];

        // error avoidance
        if (player == PLAYERCOLOR.NONE || buildingType == BUILDING_TYPE.NONE || buildingType == BUILDING_TYPE.ROAD) return false;
        // check rules if its
        if (!allowedToBuildOnNode(requestedNode, player, preGamePhase)) return false;

        switch (buildingType)
        {
            case BUILDING_TYPE.VILLAGE when requestedNode.getBuildingType() == BUILDING_TYPE.NONE:
                Debug.Log("SERVER: village can be placed");
                return true;
            case BUILDING_TYPE.CITY when requestedNode.getBuildingType() == BUILDING_TYPE.VILLAGE:
                Debug.Log("SERVER: city can be placed");
                return true;
            default:
                Debug.Log("SERVER: canPlaceBuilding(): building of type " + buildingType + " on " + requestedNode.getBuildingType() + " cant be built");
                return false;
        }
    }

    /// <summary>
    /// Checks rules whether a player is allowed to build on a given node or not 
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="player">color of the player who tries to build</param>
    /// <returns>true if player is allowed to build on given node</returns>
    private bool allowedToBuildOnNode(Node currentNode, PLAYERCOLOR player, bool preGamePhase)
    {
        // false if node is already occupied by enemy OR is city 
        if (currentNode.getOccupant() != PLAYERCOLOR.NONE
            && currentNode.getOccupant() != player
            || currentNode.getBuildingType() == BUILDING_TYPE.CITY)
        {
            Debug.Log("SERVER: node occupied by enemy or is city");
            return false;
        }

        LinkedList<int> adjacentNodesPos = currentNode.getAdjacentNodesPos();
        LinkedList<int> adjacentEdgesPos = currentNode.getAdjacentEdgesPos();
        
        // all adjacent nodes have to be empty that a village/city can be built
        foreach (int nodePos in adjacentNodesPos)
        {
            Node node = nodesArray[nodePos];

            // false if a adjacent node is already occupied
            if (node.getBuildingType() != BUILDING_TYPE.NONE) return false;
        }
        
        // if not in pre game phase, adjacent edges have to get checked
        if (!preGamePhase)
        {
            // at least one edge has to be occupied by the current player
            foreach (int edgePos in adjacentEdgesPos)
            {
                Edge edge = edgesArray[edgePos];
                // true if at least 1 edge is occupied by player
                if (edge.getOccupant() == player)
                {
                    Debug.Log("near road on " + edgePos);
                    return true;
                }
            }
            // false if none is occupied by player
            return false;
        }
        // true if in pre game phase
        // in pre game phase villages can be build without being adjacent to a node
        return true;
    }

    /// <summary>
    /// TODO 
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="player"></param>
    /// <param name="buildingType"></param>
    /// <returns></returns>
    public bool placeBuilding(int nodeId, PLAYERCOLOR player, BUILDING_TYPE buildingType)
    {
        Node requestedNode = nodesArray[nodeId];

        switch (buildingType)
        {
            case BUILDING_TYPE.VILLAGE:
                requestedNode.setBuildingType(BUILDING_TYPE.VILLAGE);
                requestedNode.setOccupant(player);
                return true;
            case BUILDING_TYPE.CITY:
                requestedNode.setBuildingType(BUILDING_TYPE.CITY);
                return true;
            default:
                Debug.Log("SERVER: buildBuilding() building of type " + buildingType + " cant be built");
                return false;
        }
    }

    /// <summary>
    /// This function has to get called to place a road onto a specific edge.
    /// If the player is allowed to place a road on the Edge with id 'edgeId',
    /// a road gets built
    /// </summary>
    /// <param name="edgeId">position of an edge in the edges[] array</param>
    /// <param name="player">color of the player who tries to build</param>
    public bool canPlaceRoad(int edgeId, PLAYERCOLOR player)
    {
        Edge currentEdge = edgesArray[edgeId];
        
        if (!allowedToBuildOnEdge(currentEdge, player)) return false;
        
        if (currentEdge.getOccupant() == PLAYERCOLOR.NONE)
        {
            Debug.Log("SERVER: road can be placed");
            return true;
        }

        return false;
    }

    /// <summary>
    /// This function has to get called in the pre game phase to place a road onto a specific edge.
    /// To be allowed to build the road, a mandatory village has to be adjacent to the desired position.
    /// If the player is allowed to place a road on the Edge with id 'edgeId',
    /// a road gets built
    /// </summary>
    /// <param name="edgeId">position of an edge in the edges[] array</param>
    /// <param name="mandatoryAdjacentNodePos">Position of the mandatory neighbor village</param>
    /// <param name="player">color of the player who tries to build</param>
    /// <returns>a bool which states, if the player is allowed to build a road at the disered positio</returns>
    public bool canPlaceRoad(int edgeId, int mandatoryAdjacentNodePos)
    {
        Edge currentEdge = edgesArray[edgeId];
        if (currentEdge.getOccupant() != PLAYERCOLOR.NONE) return false;
        
        LinkedList<int> neighborNodesPos = currentEdge.getAdjacentNodesPos();
        
        // in preGamePhase only an adjacent edge has to be occupied by current player
        foreach (int nodePos in neighborNodesPos)
        {
            if (nodePos == mandatoryAdjacentNodePos)
            {
                Debug.Log("SERVER: road can be placed");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks rules whether a player is allowed to build on a given edge or not 
    /// </summary>
    /// <param name="currentEdge"></param>
    /// <param name="player">color of the player who tries to build</param>
    /// <returns>true if player is allowed to build on given edge</returns>
    private bool allowedToBuildOnEdge(Edge currentEdge, PLAYERCOLOR player)
    {
        if (currentEdge.getOccupant() != PLAYERCOLOR.NONE) return false;

        LinkedList<int> adjacentNodesPos = currentEdge.getAdjacentNodesPos();
        LinkedList<int> adjacentEdgesPos = currentEdge.getAdjacentEdgesPos();

        // return true if at least one adjacent node is occupied by current player
        foreach (int nodePos in adjacentNodesPos)
        {
            Node node = nodesArray[nodePos];
            if (node.getOccupant() == player) return true;
        }

        // return true if at least one adjacent edge is occupied by current player
        foreach (int edgePos in adjacentEdgesPos)
        {
            Edge edge = edgesArray[edgePos];
            if (edge.getOccupant() == player) return true;
        }

        return false;
    }

    public bool placeRoad(int edgeId, PLAYERCOLOR player)
    {
        Edge currentEdge = edgesArray[edgeId];
        if (currentEdge.getOccupant() == PLAYERCOLOR.NONE)
        {
            currentEdge.setOccupant(player);
            Debug.Log("SERVER: road placed");
            return true;
        }
        return false;
    }

    /// <summary>
    /// this function goes through all (1 or 2) hexagons in hexagonDiceNumbers array with
    /// specific number to check if their adjacent nodes are occupied.
    /// If a node has BUILDING_TYPE.VILLAGE, the occupant gets 1 resource of type
    /// equivalent to the current hexagons' resource type
    /// If a node has BUILDING_TYPE.CITY, the occupant gets 2 of this resource
    /// </summary>
    /// <param name="hexagonNumber"></param>
    /// <param name="playerColor"></param>
    /// <returns></returns>
    public int[] distributeResources(int hexagonNumber, PLAYERCOLOR playerColor)
    {
        int[] resourcesToDistribute = new int[5];
        
        foreach (Hexagon hexagon in hexagonDiceNumbers[hexagonNumber])
        {
            RESOURCE_TYPE resourceType = hexagon.getResourceType();
            
            LinkedList<int> adjacentNodesPos = hexagon.getAdjacentNodesPos();
            foreach (int nodePos in adjacentNodesPos)
            {
                Node node = nodesArray[nodePos];
                switch (node.getBuildingType())
                {
                    case BUILDING_TYPE.NONE: break;
                    case BUILDING_TYPE.VILLAGE:
                        if (node.getOccupant() == playerColor)
                        {
                            resourcesToDistribute[(int) resourceType] += 1;
                        }
                        break;
                    case BUILDING_TYPE.CITY:
                        if (node.getOccupant() == playerColor)
                        {
                            resourcesToDistribute[(int) resourceType] += 2;
                        }
                        break;
                    default: Debug.Log("SERVER: distributeResources(): wrong BUILDING_TYPE"); break;
                }
            }
        }

        Debug.Log("SERVER: Player " + (int) playerColor + " gets: " + resourcesToDistribute[0] + resourcesToDistribute[1] + resourcesToDistribute[2] + resourcesToDistribute[3] + resourcesToDistribute[4]);
        return resourcesToDistribute;
    }

    /// <summary>
    /// this function goes through all adjacent hexagons of a specific node 
    /// </summary>
    /// <param name="nodeId"></param>
    /// <returns></returns>
    public int[] distributeFirstResources(int nodeId)
    {
        int[] resourcesToDistribute = new int[5];
        LinkedList<int[]> adjacentHexagonsPos = nodesArray[nodeId].getAdjacentHexagonsPos();
        
        foreach (int[] hexagonPos in adjacentHexagonsPos)
        {
            HEXAGON_TYPE hexagonType = hexagonsArray[hexagonPos[0]][hexagonPos[1]].getType();
            Debug.LogWarning("hexagonType: " + hexagonType);
            // all hexagons that are of type water/desert/port are skipped
            if ((int) hexagonType > 4)
            {
                continue;
            }
            resourcesToDistribute[(int) hexagonType]++;
        }
        return resourcesToDistribute;
    }

    /// <summary>
    /// reads from config file and adds to all hexagons in hexagons[] the adjacent nodes 
    /// </summary>
    private void assignNeighborsToHexagons()
    {
        StreamReader file = new StreamReader(configPath + "AdjacentNodesToHexagons.txt");

        for (int row = 0; row < boardConfig.Length; row++)
        {
            for (int col = 0; col < boardConfig[row].Length; col++)
            {
                // continue if there is no hexagon at given index
                if (hexagonsArray[row][col] == null) continue;

                Hexagon currentHexagon = hexagonsArray[row][col];
                
                // give all neighbors a hexagon needs to know
                string line = file.ReadLine();
                string[] subStrings = line.Split(',');

                for (int i = 0; i < 6; i++)
                {
                    if (subStrings[i] != "-")
                    {
                        int neighborPos = int.Parse(subStrings[i]);
                        currentHexagon.setAdjacentNodePos(neighborPos);
                    }
                }
            }
        }
        file.Close();
    }

    /// <summary>
    /// reads from config files and adds to all nodes in nodes[] the adjacent hexagons, nodes and edges 
    /// </summary>
    private void assignNeighborsToNodes()
    {
        StreamReader hexagonsFile = new StreamReader(configPath + "AdjacentHexagonsToNodes.txt");
        StreamReader nodesFile = new StreamReader(configPath + "AdjacentNodesToNodes.txt");
        StreamReader edgesFile = new StreamReader(configPath + "AdjacentEdgesToNodes.txt");

        foreach (Node currentNode in nodesArray)
        {
            string[] nHexagons = hexagonsFile.ReadLine().Split(',');
            string[] nNodes = nodesFile.ReadLine().Split(',');
            string[] nEdges = edgesFile.ReadLine().Split(',');

            for (int i = 0; i < 3; i++)
            {
                // sets adjacent hexagon
                string[] nHexagonCoordinates = nHexagons[i].Split('.');
                int nHexagonPosX = int.Parse(nHexagonCoordinates[0]);
                int nHexagonPosY = int.Parse(nHexagonCoordinates[1]);
                currentNode.setAdjacentHexagonPos(new[] {nHexagonPosX, nHexagonPosY});

                // sets adjacent node
                if (nNodes[i] != "-")
                {
                    int nNodePos = int.Parse(nNodes[i]);
                    currentNode.setAdjacentNodePos(nNodePos);
                }

                // sets adjacent edge
                if (nEdges[i] != "-")
                {
                    int nEdgePos = int.Parse(nEdges[i]);
                    currentNode.setAdjacentEdgePos(nEdgePos);
                }
            }
        }
        hexagonsFile.Close();
        nodesFile.Close();
        edgesFile.Close();
    }

    /// <summary>
    /// reads from config files and adds to all edges ind edges[] the adjacent nodes and edges
    /// </summary>
    private void assignNeighborsToEdges()
    {
        StreamReader nodesFile = new StreamReader(configPath + "AdjacentNodesToEdges.txt");
        StreamReader edgesFile = new StreamReader(configPath + "AdjacentEdgesToEdges.txt");

        foreach (Edge currentEdge in edgesArray)
        {
            string[] nNodes = nodesFile.ReadLine().Split(',');
            string[] nEdges = edgesFile.ReadLine().Split(',');

            for (int i = 0; i < 2; i++)
            {
                if (nNodes[i] != "-")
                {
                    int nNodePos = int.Parse(nNodes[i]);
                    currentEdge.setAdjacentNodePos(nNodePos, i);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (nEdges[i] != "-")
                {
                    int nEdgePos = int.Parse(nEdges[i]);
                    currentEdge.setAdjacentEdgePos(nEdgePos, i);
                }
            }
        }
        nodesFile.Close();
        edgesFile.Close();
    }
    
    /// <summary>
    /// Checks if Hexagons with a fieldnumber of 6 or 8 are adjacent
    /// </summary>
    private void checkPlacementConstraints()
    {
        for (int row = 1; row < hexagonsArray.Length - 1; row++)
        {
            for (int col = 1; col < hexagonsArray[row].Length - 1; col++)
            {
                Hexagon currentHex = hexagonsArray[row][col];

                //no need to check ports or dessert
                if (currentHex == null || currentHex.getFieldNumber() == 0)
                {
                    continue;
                }

                int fieldNumber = currentHex.getFieldNumber();

                //neighbors just needs to be checked if fieldnumber is 6 or 8
                if (fieldNumber != 6 && fieldNumber != 8)
                {
                    continue;
                }

                //iterating over adjacent hexagons
                for (int offsetIndex = 0; offsetIndex < neighborOffsetX.Length; offsetIndex++)
                {
                    int yOffset = row + neighborOffsetY[offsetIndex];
                    int xOffset = col + neighborOffsetX[offsetIndex];

                    //when index is out of range or at the index is no object there is no adjacent hexagon, therefore the constraint for this neighbor is met
                    if (hexagonsArray[yOffset][xOffset]==null || yOffset > hexagonsArray.Length-1 || xOffset > hexagonsArray[yOffset].Length-1)
                    {
                        continue;
                    }

                    Hexagon neighbor = hexagonsArray[yOffset][xOffset];

                    //if one of the neighbors fieldnumber is 6 or 8 the hexagon needs to be moved
                    if (neighbor.getFieldNumber() != 6 && neighbor.getFieldNumber() != 8)
                    {
                        continue;
                    }

                    int[] suitablePos = findSuitablePos();
                    swapHexagonPositions(row, col, suitablePos[0], suitablePos[1]);
                    break;

                }
            }
        }
    }

    /// <summary>
    /// checks the hexagons array for a suitable position where none of the adjacent hexagons has a 6 or 8 as fielnumber.
    /// </summary>
    /// <returns>an int array with the first occouring coordinates of a suitable position or empty array if no suitable position is found</returns>
    private int[] findSuitablePos()
    {

        for (int row = 1; row < hexagonsArray.Length - 1; row++)
        {
            for (int col = 1; col < hexagonsArray[row].Length - 1; col++)
            {
                Hexagon currentHex = hexagonsArray[row][col];

                //no need to check ports or dessert
                if (currentHex == null || currentHex.getFieldNumber() == 0)
                {
                    continue;
                }

                int fieldNumber = currentHex.getFieldNumber();

                //if hexgon itself is 6 or 8, there is no sense in switching
                if (fieldNumber != 6 && fieldNumber != 8 && fieldNumber != 0)
                {
                    bool suitable = true;
                    //iterate over neighbors to see if suitable
                    for (int offsetIndex = 0; offsetIndex < neighborOffsetX.Length; offsetIndex++)
                    {
                        int yOffset = row + neighborOffsetY[offsetIndex];
                        int xOffset = col + neighborOffsetX[offsetIndex];
                       
                        //if index is out of range there is no adjacent hexagon, therefore the constraint for this neighbor is met
                        if (hexagonsArray[yOffset][xOffset] == null || yOffset > hexagonsArray.Length - 1 || xOffset > hexagonsArray[yOffset].Length-1)
                        {
                            continue;
                        }

                        int neighborFieldnumber = hexagonsArray[yOffset][xOffset].getFieldNumber();

                        //if one of the neighbors is 6 || 8 position is mot suitable
                        if (neighborFieldnumber == 6 || neighborFieldnumber == 8)
                        {
                            suitable = false;
                            break;
                        }
                    }
                    // loop didnt break, therefore the position is suitable
                    if (suitable)
                    {
                        return new int[] { row, col };
                    }
                }
            }
        }

        return new int[] { };
    }

    /// <summary>
    /// Swaps the position of two hexagons in the hexagons array.
    /// </summary>
    /// <param name="firstHexRow">specifies the row in the hexagons array of the first hexagon to swap</param>
    /// <param name="firstHexCol">specifies the column in the hexagons array of the first hexagon to swap</param>
    /// <param name="secondHexRow">specifies the row in the hexagons array of the second hexagon to swap</param>
    /// <param name="secondHexCol">specifies the column in the hexagons array of the second hexagon to swap</param>

    private void swapHexagonPositions(int firstHexRow, int firstHexCol, int secondHexRow, int secondHexCol)
    {
        Hexagon tempHex = hexagonsArray[firstHexRow][firstHexCol];
        hexagonsArray[firstHexRow][firstHexCol] = hexagonsArray[secondHexRow][secondHexCol];
        hexagonsArray[secondHexRow][secondHexCol] = tempHex;
    }
}
