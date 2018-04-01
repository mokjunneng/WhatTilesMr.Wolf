using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HexSpawner : NetworkBehaviour {

    public GameObject mapPreFab;


    // Use this for initialization
    int mapHeight = 10;
    int mapWidth = 10;

    [SyncVar]
    public bool init = true;

    public GameObject hexPrefab;
    public GameObject alternatePrefab;

    //store all tiles generated
    public List<GameObject> tiles;


    [SyncVar]
    public int random = 0;

    public override void OnStartServer()
    {
        GameObject map = (GameObject)Instantiate(mapPreFab, new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0, 0));
        NetworkServer.Spawn(map);

        //for (int column = 0; column < mapWidth; column++)
        //{
        //    for (int row = 0; row < mapHeight; row++)
        //    {
        //        Hex hex = new Hex(column, row);

        //        randomize prefab
        //        random = UnityEngine.Random.Range(0, 2);

        //        Instantiate a Hex
        //        GameObject hexagon;

        //        if (random == 0)
        //            hexagon = (GameObject)Instantiate(hexPrefab, hex.Position(), Quaternion.identity, this.transform);
        //        else
        //            hexagon = (GameObject)Instantiate(alternatePrefab, hex.Position(), Quaternion.identity, this.transform);

        //        Tag all the text mesh with tile coordinate
        //        hexagon.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

        //        update player data
        //        if (mr.material.color == red)
        //        {
        //            tilesPlayer.Add(hexGO);
        //        }
        //        else
        //        {
        //            tilesOpponent.Add(hexGO);
        //        }
        //        tiles.Add(hexagon);

        //        NetworkServer.Spawn(hexagon);

        //    }
        //}
    }
}
