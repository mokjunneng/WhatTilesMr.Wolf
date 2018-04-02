//using System;
//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////[ExecuteInEditMode]
//public class HexMap : MonoBehaviour
//{

//    public GameObject player;

//    //private Player playerData;

//    Node[,] graph;

//    public GameObject HexPrefab;

//    public Material[] HexMaterials;

//    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
//    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

//    public int playerCount = 0;
//    public int totalCount = 0;

//    int mapHeight = 10;
//    int mapWidth = 10;

//    // Use this for initialization
//    void Start()
//    {
//        //playerData = player.GetComponentInChildren<Player>();
//        GenerateMap();

//       //print("tiles that belong to player: " + playerData.tiles.Count);
//        //GeneratePathFindingGraph();
//        //Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity, this.transform);

//    }

//    private void GenerateMap()
//    {
//        for (int column = 0; column < mapWidth; column++)
//        {
//            for (int row = 0; row < mapHeight; row++)
//            {
//                Hex h = new Hex(column, row);

//                //Instantiate a Hex
//                GameObject hexGO = (GameObject)Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);

//                //Tag all the text mesh with the correct tile coordinate
//                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

//                //Get tile coordinates for mouse logic script
//                //ClickableTile ct = hexGO.GetComponentInChildren<ClickableTile>();
//                //ct.map = this;
//                //ct.tilePos = h.Position();
//                //ct.tileCol = column;
//                //ct.tileRow = row;
//                //ct.player = player.GetComponentInChildren<Player>();

//                //randomize tile color 
//                MeshRenderer mr = hexGO.transform.GetChild(1).GetComponent<MeshRenderer>();
//                mr.material = HexMaterials[UnityEngine.Random.Range(0, HexMaterials.Length)];

//                //update player data
//                if (mr.material.color == red)
//                {
//                    //playerData.tiles.Add(hexGO);
//                    playerCount++;
//                }
//                totalCount++;
//                //assign each tiles to have a trigger event

//            }
//        }

//        //StaticBatchingUtility.Combine(this.gameObject);
//    }


//    //Temporarily not including in game
//    public void GenerateShortestPathTo(Vector3 pos, int col, int row)
//    {
//        Debug.Log("Finding path..");
//        //clear old path of player unit
//       // player.GetComponentInChildren<Player>().currentPath = null;


//        Dictionary<Node, float> dist = new Dictionary<Node, float>();
//        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

//        List<Node> unvisitedNodes = new List<Node>();

//        Node source = graph[
//        //player.GetComponentInChildren<Player>().getTileCol(),
//        //player.GetComponentInChildren<Player>().getTileRow()];

//        Node target = graph[col, row];

//        dist[source] = 0;
//        parent[source] = null;

//        //initialize-single-source
//        foreach (Node v in graph)
//        {
//            if (v != source)
//            {
//                dist[v] = Mathf.Infinity;
//                parent[v] = null;
//                unvisitedNodes.Add(v);
//            }
//        }

//        while (unvisitedNodes.Count > 0)
//        {
//            //TODO: consider some priority queue or some other self-sorting, optimized
//            //data structure

//            //u is the unvisited node with the shortest distance
//            Node u = null;

//            foreach (Node node in unvisitedNodes)
//            {
//                if (u == null || dist[node] < dist[u])
//                {
//                    u = node;
//                    //print(node.position);
//                }
//            }

//            //Node u = unvisitedNodes.OrderBy(n => dist[n]).First();

//            //found target, exit while loop
//            //if (u == target)
//            //{
//            //    Debug.Log("Found target");
//            //    break;
//            //}

//            unvisitedNodes.Remove(u);

//            foreach (Node v in u.neighbours)
//            {
//                float alt = dist[u] + u.distanceTo(v);

//                if (alt < dist[v])
//                {
//                    dist[v] = alt;
//                    parent[v] = u;
//                }

//            }
//        }

//        //Debug.Log("end of path finding.");
//        if (parent[target] == null)
//        {
//            Debug.Log("no viable path found");
//            //no route between target and source
//            return;
//        }

//        //update selected unit position
//        player.GetComponentInChildren<Player>().setTileCol(col);
//        player.GetComponentInChildren<Player>().setTileRow(row);


//        //get shortest path
//        List<Node> currentPath = new List<Node>();

//        Node currentNode = target;

//        while (currentNode != null)
//        {
//            currentPath.Add(currentNode);
//            currentNode = parent[currentNode];
//        }

//        currentPath.Reverse();

//        Debug.Log("path finding complete");
//       // player.GetComponentInChildren<Player>().currentPath = currentPath;
//    }



//    public class Node
//    {
//        public List<Node> neighbours;
//        public Vector3 position;
//        public int q, r;

//        public Node(int q, int r)
//        {
//            this.neighbours = new List<Node>();
//            this.q = q;
//            this.r = r;
//        }

//        public float distanceTo(Node n)
//        {
//            //return Vector3.Distance(
//            //  position, n.position);
//            float distance = (Mathf.Abs(this.q - n.q) + Mathf.Abs(this.q + this.r - n.q - n.r) + Mathf.Abs(this.r - n.r)) / 2f;
//            //print(distance);
//            return distance;
//        }
//    }



//    void GeneratePathFindingGraph()
//    {
//        graph = new Node[mapWidth, mapHeight];

//        for (int column = 0; column < mapWidth; column++)
//        {
//            for (int row = 0; row < mapHeight; row++)
//            {
//                Hex h = new Hex(column, row);
//                //6-way connected map
//                graph[column, row] = new Node(column, row);
//                graph[column, row].position = h.Position();
//            }
//        }

//        for (int column = 0; column < mapWidth; column++)
//        {
//            for (int row = 0; row < mapHeight; row++)
//            {
//                if (row % 2 == 1)
//                {
//                    if (column > 0 && row < mapHeight - 1)
//                    {
//                        graph[column, row].neighbours.Add(graph[column, row + 1]);
//                    }
//                    if (row > 0)
//                    {
//                        graph[column, row].neighbours.Add(graph[column, row - 1]);
//                    }
//                    if (row > 0 && column < mapWidth - 1)
//                    {
//                        graph[column, row].neighbours.Add(graph[column + 1, row - 1]);
//                    }
//                    if (row < mapHeight - 1 && column < mapWidth - 1)
//                    {
//                        graph[column, row].neighbours.Add(graph[column + 1, row + 1]);
//                    }
//                }
//                else
//                {
//                    if (column > 0 && row < mapHeight - 1)
//                    {
//                        graph[column, row].neighbours.Add(graph[column - 1, row + 1]);
//                    }
//                    if (row > 0 && column > 0)
//                    {
//                        graph[column, row].neighbours.Add(graph[column - 1, row - 1]);
//                    }
//                    if (row > 0)
//                    {
//                        graph[column, row].neighbours.Add(graph[column, row - 1]);
//                    }
//                    if (row < mapHeight - 1)
//                    {
//                        graph[column, row].neighbours.Add(graph[column, row + 1]);
//                    }
//                }
//                if (column > 0)
//                {
//                    graph[column, row].neighbours.Add(graph[column - 1, row]);
//                }
//                if (column < mapWidth - 1)
//                {
//                    graph[column, row].neighbours.Add(graph[column + 1, row]);
//                }

//            }
//        }

//        //for (int column = 0; column < mapWidth; column++)
//        //{
//        //    for (int row = 0; row < mapHeight; row++)
//        //    {
//        //        print("Node " + column + "," + row + ":" + graph[column, row].position);
//        //        for (int i = 0; i < graph[column, row].neighbours.Count; i++)
//        //        {
//        //            print(graph[column, row].neighbours[i].position);
//        //        }
//        //    }
//        //}
//    }
//}