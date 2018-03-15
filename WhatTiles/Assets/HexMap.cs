
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

    //Store tiles that belongs to player
    
    public List<GameObject> tilesPlayer;
    public List<GameObject> tilesOpponent;

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
                Hex h = new Hex(column, row);

                //Instantiate a Hex
                GameObject hexGO = (GameObject)Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);

                //Tag all the text mesh with the correct tile coordinate
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                //randomize tile color 
                MeshRenderer mr = hexGO.transform.GetChild(1).GetComponent<MeshRenderer>();
                mr.material = HexMaterials[UnityEngine.Random.Range(0, HexMaterials.Length)];
                

                //update player data
                if (mr.material.color == red)
                {
                    tilesPlayer.Add(hexGO);
                }
                else
                {
                    tilesOpponent.Add(hexGO);
                }

                NetworkServer.Spawn(hexGO);
                //assign each tiles to have a trigger event

            }
        }
    }

}
