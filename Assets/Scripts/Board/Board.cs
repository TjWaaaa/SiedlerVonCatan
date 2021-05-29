using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enums;
using Player;
using UnityEngine;

//TODO: assignNeighborsToHexagons(); prüfen ob die Methode mit de neuen Array klar kommt
//TODO: assignNeighborsToNodes();    prüfen ob die Methode mit de neuen Array klar kommt
//TODO: assignNeighborsToEdges();    prüfen ob die Methode mit de neuen Array klar kommt

public class Board
{
    private Hexagon[][] hexagonsArray;
    private Node[] nodesArray = new Node[54];
    private Edge[] edgesArray = new Edge[72];
    private Stack<int> numberStack;

    private Hexagon[][] hexagonDiceNumbers =
    {
        null,           // 0
        null,           // 1
        new Hexagon[1], // 2
        new Hexagon[2], // 3
        new Hexagon[2], // 4
        new Hexagon[2], // 5
        new Hexagon[2], // 6
        null,           // 7
        new Hexagon[2], // 8
        new Hexagon[2], // 9
        new Hexagon[2], // 10
        new Hexagon[2], // 11
        new Hexagon[1], // 12
    };

    private readonly int[][] boardConfig = {
        new[] {0,0,0,4,1,4,1},
        new[]  {0,0,1,2,2,2,4},
        new[]   {0,4,2,2,2,2,1},
        new[]     {1,2,2,3,2,2,4},
        new[]     {4,2,2,2,2,1,0},
        new[]      {1,2,2,2,4,0,0},
        new[]       {4,1,4,1,0,0,0}
    };

    private int[] neighborOffsetX = new int[] {  1, 0,-1,-1, 0, 1 }; //specifies the position of adjacent hexagons in horizontal direction
    private int[] neighborOffsetY = new int[] { -1,-1, 0, 1, 1, 0 }; //specifies the position of adjacent hexagons in vertical direction
    private int[] availableNumbers = new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };

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

    private const string path = "Assets/Scripts/Board/";

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

    /// <summary>
    /// creates an array of Hexagons with randomly placed hexagon_Types and hexagonNumbers
    /// </summary>
    /// <returns>returns the array</returns>
    private void initializeHexagons()
    {
        Stack<HEXAGON_TYPE> landStack = createRandomHexagonStackFromArray(landHexagons);
        Stack<HEXAGON_TYPE> portStack = createRandomHexagonStackFromArray(portHexagons);
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
                        hexagonsArray[row][col] = newHexagon;

                        // adds Hexagon to empty slot in array

                        if (hexagonDiceNumbers[fieldNumber][0] == null)
                        {
                            hexagonDiceNumbers[fieldNumber][0] = newHexagon;
                        }
                        else if (hexagonDiceNumbers[fieldNumber][1] == null)
                        {
                            hexagonDiceNumbers[fieldNumber][1] = newHexagon;
                        }
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
    private Stack<HEXAGON_TYPE> createRandomHexagonStackFromArray(HEXAGON_TYPE[] array)
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

    private void initializeNodes()
    {
        for (int i = 0; i < 54; i++)
        {
            nodesArray[i] = new Node(i);
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
    /// This function has to get called to place a village or city onto a specific node.
    /// If the player is allowed to place a building on the Node with id 'nodeId', a village
    /// gets built, or gets upgraded into a city
    /// </summary>
    /// <param name="nodeId">position of a node in the nodes[] array</param>
    /// <param name="player">color of the player who tries to build</param>
    public bool placeBuilding(int nodeId, PLAYERCOLOR player, bool preGamePhase)
    {
        Node requestedNode = nodesArray[nodeId];
        if (!allowedToBuildOnNode(requestedNode, player, preGamePhase)) return false;

        if (requestedNode.getBuildingType() == BUILDING_TYPE.NONE)
        {
            Debug.Log("SERVER: place village");
            requestedNode.setBuildingType(BUILDING_TYPE.VILLAGE);
            requestedNode.setOccupant(player);
            return true;
        }
        if (requestedNode.getBuildingType() == BUILDING_TYPE.VILLAGE)
        {
            Debug.Log("SERVER: place city");
            requestedNode.setBuildingType(BUILDING_TYPE.CITY);
            requestedNode.setOccupant(player);
            return true;
        }

        return false;
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
            Debug.Log("SERVER: occupied by enemy or city");
            return false;
        }

        int[] neighborNodesPos = currentNode.getAdjacentNodesPos();
        int[] neighborEdgesPos = currentNode.getAdjacentEdgesPos();

        foreach (int nodePos in neighborNodesPos)
        {
            Node node = nodesArray[nodePos];

            // false if a neighborNode is already occupied
            if (node.getBuildingType() != BUILDING_TYPE.NONE) return false;
        }

        //in pre game phase cities can be build without being adjacent to a node
        if (!preGamePhase)
        {
            foreach (int edgePos in neighborEdgesPos)
            {
                Edge edge = edgesArray[edgePos];
                // true if at least 1 edge is occupied by player
                if (edge.getOccupant() == player) return true;
            }
        }
        return true;
        //return false;
    }

    /// <summary>
    /// This function has to get called to place a road onto a specific edge.
    /// If the player is allowed to place a road on the Edge with id 'edgeId',
    /// a road gets built
    /// </summary>
    /// <param name="edgeId">position of an edge in the edges[] array</param>
    /// <param name="player">color of the player who tries to build</param>
    public bool placeRoad(int edgeId, PLAYERCOLOR player)
    {
        Edge currentEdge = edgesArray[edgeId];
        
        if (!allowedToBuildOnEdge(currentEdge, player)) return false;
        
        if (currentEdge.getOccupant() == PLAYERCOLOR.NONE)
        {
            Debug.Log("SERVER: place road");
            currentEdge.setOccupant(player);
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
    /// <param name="mandatoryAdjacentNode">Position of the mandatory neighbor village</param>
    /// <param name="player">color of the player who tries to build</param>
    /// <returns>a bool which states, if the player is allowed to build a road at the disered positio</returns>
    public bool placeRoad(int edgeId, int mandatoryAdjacentNode, PLAYERCOLOR player)
    {
        Edge currentEdge = edgesArray[edgeId];
        bool mandatoryNodeIsNeighbor = false;
        int[] neighborNodesPos = currentEdge.getAdjacentNodesPos();

        foreach (int adjacentNodePos in neighborNodesPos)
        {
            if (adjacentNodePos == mandatoryAdjacentNode)
            {
                mandatoryNodeIsNeighbor = true;
                break;
            }
        }

        if (mandatoryNodeIsNeighbor && currentEdge.getOccupant() == PLAYERCOLOR.NONE)
        {
            Debug.Log("SERVER: place road");
            currentEdge.setOccupant(player);
            return true;
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

        int[] neighborNodesPos = currentEdge.getAdjacentNodesPos();
        int[] neighborEdgesPos = currentEdge.getAdjacentEdges();

        foreach (int nodePos in neighborNodesPos)
        {
            Node node = nodesArray[nodePos];
            if (node.getOccupant() == player) return true;
        }

        foreach (int edgePos in neighborEdgesPos)
        {
            Edge edge = edgesArray[edgePos];
            if (edge.getOccupant() == player) return true;
        }

        return false;
    }

    public int[] distributeResources(int hexagonNumber, PLAYERCOLOR playerColor)
    {
        int[] distributedResources = new int[5];
        
        foreach (Hexagon hexagon in hexagonDiceNumbers[hexagonNumber])
        {
            int resourceType = (int) hexagon.getResourceType();
            
            int[] adjacentNodesPos = hexagon.getAdjacentNodesPos();
            foreach (int nodePos in adjacentNodesPos)
            {
                Node node = nodesArray[nodePos];
                switch (node.getBuildingType())
                {
                    case BUILDING_TYPE.NONE: break;
                    case BUILDING_TYPE.VILLAGE:
                        if (node.getOccupant() == playerColor)
                        {
                            distributedResources[resourceType] = distributedResources[resourceType] + 1;
                        }
                        break;
                    case BUILDING_TYPE.CITY:
                        if (node.getOccupant() == playerColor)
                        {
                            distributedResources[resourceType] = distributedResources[resourceType] + 2;
                        }
                        break;
                    default: Debug.Log("SERVER: distributeResources(): wrong BUILDING_TYPE"); break;
                }
            }
        }

        return distributedResources;
    }

    /// <summary>
    /// reads from config file and adds to all hexagons in hexagons[] the adjacent nodes 
    /// </summary>
    private void assignNeighborsToHexagons()
    {
        StreamReader file = new StreamReader(path + "AdjacentNodesToHexagons.txt");

        for (int row = 0; row < boardConfig.Length; row++)
        {
            for (int col = 0; col < boardConfig[row].Length; col++)
            {
                try
                {
                    // continue if there is no hexagon at given index
                    if (hexagonsArray[row][col] == null) continue;
                }
                catch (Exception e)
                {
                    Debug.Log("SERVER: "+ e);
                }

                Hexagon currentHexagon = hexagonsArray[row][col];

                // give all neighbors a hexagon needs to know
                string line = file.ReadLine();
                string[] subStrings = line.Split(',');

                for (int i = 0; i < 6; i++)
                {
                    if (subStrings[i] == "-") continue;

                    int neighborPos = int.Parse(subStrings[i]);
                    currentHexagon.setAdjacentNodePos(neighborPos, i);
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
        StreamReader hexagonsFile = new StreamReader(path + "AdjacentHexagonsToNodes.txt");
        StreamReader nodesFile = new StreamReader(path + "AdjacentNodesToNodes.txt");
        StreamReader edgesFile = new StreamReader(path + "AdjacentEdgesToNodes.txt");

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
                currentNode.setAdjacentHexagonPos(nHexagonPosX, nHexagonPosY, i);

                // sets adjacent node
                if (nNodes[i] != "-")
                {
                    int nNodePos = int.Parse(nNodes[i]);
                    currentNode.setAdjacentNodePos(nNodePos, i);
                }

                // sets adjacent edge
                if (nEdges[i] != "-")
                {
                    int nEdgePos = int.Parse(nEdges[i]);
                    currentNode.setAdjacentEdgePos(nEdgePos, i);
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
        StreamReader nodesFile = new StreamReader(path + "AdjacentNodesToEdges.txt");
        StreamReader edgesFile = new StreamReader(path + "AdjacentEdgesToEdges.txt");

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
                    currentEdge.setAdjacentEdge(nEdgePos, i);
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
