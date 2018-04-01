using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapSpawnerScript : NetworkBehaviour
{
    public GameObject tilePrefab;
    public Material[] tileMaterials;

    public List<GameObject> tiles;
    public List<Material> AppliedMaterials;

    public int f;
    public float nextSpawnTime = 0;

    public override void OnStartServer()
    {
        Spawn();
    }

    // Use this for initialization
    void Spawn() {
        for (int i = 0; i < 3; i++)
        {
            var pos = new Vector3(
                Random.Range(-1.0f, 3.0f),
                0.2f,
                Random.Range(-1.0f, 3.0f)
                );
            var rotation = Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
            var tile = (GameObject)Instantiate(tilePrefab, pos, rotation);

            //randomize tile color 
            MeshRenderer mr = tile.transform.GetComponent<MeshRenderer>();
            int random = UnityEngine.Random.Range(0, tileMaterials.Length);
            mr.material = tileMaterials[random];
            AppliedMaterials.Add(mr.material);


            //update player data
            NetworkServer.Spawn(tile);
            tiles.Add(tile);

            //tile.GetComponent<TestCubeScript>().addColor();
            //if (tile.GetComponent<TestCubeScript>() != null)
            //{
            //    tile.GetComponent<TestCubeScript>().addColor();
                

            //}
        }



    }

    

    //private void GenerateMap()
    //{
    //    for (int column = 0; column < mapWidth; column++)
    //    {
    //        //for (int row = 0; row < mapHeight; row++)
    //        //{
    //            Hex h = new Hex(column, row);

    //            //Instantiate a Hex
    //            GameObject hexGO = (GameObject)Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);

    //            //Tag all the text mesh with the correct tile coordinate
    //            hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

    //            //randomize tile color 
    //            MeshRenderer mr = hexGO.transform.GetChild(1).GetComponent<MeshRenderer>();
    //            mr.material = HexMaterials[UnityEngine.Random.Range(0, HexMaterials.Length)];


    //            //update player data
    //            if (mr.material.color == red)
    //            {
    //                tilesPlayer.Add(hexGO);
    //            }
    //            else
    //            {
    //                tilesOpponent.Add(hexGO);
    //            }

    //            NetworkServer.Spawn(hexGO);
    //            //assign each tiles to have a trigger event

    //        }
    //    }
    //}
}
