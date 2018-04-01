
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

    public GameObject hexPrefab;
    public GameObject alternatePrefab;


    [SyncVar]
    public int random = 0;

    //Store tiles that belongs to player

    //public List<GameObject> tilesPlayer;
    //public List<GameObject> tilesOpponent;

    //store all tiles generated
    public List<GameObject> tiles;

    // Use this for initialization
    void Start () { 
        GenerateMap();
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
                Hex hex = new Hex(column, row);

                //randomize prefab
                random = UnityEngine.Random.Range(0,2);

                //Instantiate a Hex
                GameObject hexagon;

                //if (random == 0)
                //    hexagon = (GameObject)Instantiate(hexPrefab, hex.Position(), Quaternion.identity, this.transform);
                //else
                    hexagon = (GameObject)Instantiate(alternatePrefab, hex.Position(), Quaternion.identity, this.transform);

                //Tag all the text mesh with tile coordinate
                hexagon.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                //update player data
                //if (mr.material.color == red)
                //{
                //    tilesPlayer.Add(hexGO);
                //}
                //else
                //{
                //    tilesOpponent.Add(hexGO);
                //}
                tiles.Add(hexagon);

                NetworkServer.Spawn(hexagon);

            }
        }
    }

}
