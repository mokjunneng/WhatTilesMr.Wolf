using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TileSpawner : NetworkBehaviour
{
    public GameObject HexPrefab;

    int mapHeight = 10;
    int mapWidth = 10;

    public override void OnStartServer()
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
               
                NetworkServer.Spawn(hexGO);
            }
        }
    }
}

