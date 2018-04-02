
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[ExecuteInEditMode]
public class HexMap : NetworkBehaviour {
    // Use this for initialization
    public Material[] HexMaterials;

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    int mapHeight = 10;
    int mapWidth = 10;
    public bool init = true;

    public GameObject HexPrefab;
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




    //[ClientRpc]
    //private void RpcSpawnMap()
    //{
    //    Debug.Log("spawning map for client");
    //    foreach(GameObject tile in tiles)
    //    {
    //        NetworkServer.SpawnWithClientAuthority(tile, connectionToClient);
    //    }
    //}

    void Update()
    {
    }
   
    private void GenerateMap()
    {
        for (int column = 0; column < mapWidth; column++)
        {
            for (int row = 0; row < mapHeight; row++)
            {
                Hex h = new Hex(column, row);

                int color = UnityEngine.Random.Range(0, 2);
                GameObject hexGO;

                //0: blue, 1: red
                if(color == 0)
                {
                    hexGO = (GameObject)Instantiate(HexPrefabBlue, h.Position(), Quaternion.identity, this.transform);
                    blueTiles.Add(hexGO);
                }
                else
                {
                    hexGO = (GameObject)Instantiate(HexPrefabRed, h.Position(), Quaternion.identity, this.transform);
                    redTiles.Add(hexGO);
                }

                tiles.Add(hexGO);

                //Instantiate a Hex
                //GameObject hexGO = (GameObject)Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);

                //Tag all the text mesh with the correct tile coordinate
                //hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                //randomize tile color 
                //MeshRenderer mr = hexGO.transform.GetChild(1).GetComponent<MeshRenderer>();
                //mr.material = HexMaterials[UnityEngine.Random.Range(0, HexMaterials.Length)];


                //update player data
                //if (mr.material.color == red)
                //{
                //    tilesPlayer.Add(hexGO);
                //}
                //else
                //{
                //    tilesOpponent.Add(hexGO);
                //}


                //NetworkServer.Spawn(hexGO);

                //assign each tiles to have a trigger event

            }
        }
        Debug.Log("Map data generated");
    }

}
