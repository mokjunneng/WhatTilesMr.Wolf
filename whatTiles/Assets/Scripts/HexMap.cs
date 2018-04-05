
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using NUnit.Framework;

//[ExecuteInEditMode]
public class HexMap : NetworkBehaviour {
    // Use this for initialization
    public Material[] HexMaterials;

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    private int mapHeight = 15; //changed to fit screen
    private int mapWidth = 7;
    private bool init = true;

    public GameObject HexPrefabBlue;
    public GameObject HexPrefabRed;

    //Store tiles that belongs to player

    public List<GameObject> tiles;
    public List<GameObject> redTiles;
    public List<GameObject> blueTiles;

    // Use this for initialization
    void Start ()
    {
        

        Debug.Log("Server Generating Map");
        GenerateMap();
        GenerateMapForClient();
        
	}

    
    private void GenerateMapForClient()
    {
        foreach(GameObject tile in tiles)
        {
            NetworkServer.Spawn(tile);
        }
    }

    void Update()
    {
    }
   
    private void GenerateMap()
    {
        for (int column = 0; column < mapWidth; column++)
        {
            for (int row = 0; row < mapHeight; row++)
            {
                Hex h = new Hex(column * 1.0f, row * 1.0f); //change here

                int color = UnityEngine.Random.Range(0, 2);
                GameObject hexGO;

                //0: blue, 1: red
                if(color == 0)
                {
                    hexGO = (GameObject) Instantiate(HexPrefabBlue, h.Position(), Quaternion.identity, this.transform);
                    blueTiles.Add(hexGO);
                }
                else
                {
                    hexGO = (GameObject) Instantiate(HexPrefabRed, h.Position(), Quaternion.identity, this.transform);
                    redTiles.Add(hexGO);
                }

                tiles.Add(hexGO);

            }
        }
        Debug.Log("Map data generated");
        Debug.Log("Init Red : " + redTiles.Count);
        Debug.Log("Init Blue : " + blueTiles.Count);
    }

}
