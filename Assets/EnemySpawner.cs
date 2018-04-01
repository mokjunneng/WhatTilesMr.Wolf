using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    //public GameObject tilePrefab;
    public int numEnemies;

    //private List<GameObject> tiles;

    //[SyncVar]
    //public int random = 0;

    public override void OnStartServer()
    {

        //var position = new Vector3(2.97843f, 1.850032f, 4090482f);
        //var position = new Vector3(0f, 0f, 0f);
        //var rotation = Quaternion.Euler(0f, 0f, 0f);

        //GameObject enemy = (GameObject)Instantiate(enemyPrefab, position, rotation);
        //NetworkServer.Spawn(enemy);

        //tiles = tileStorage.GetComponent<TileStorage>().tiles;

        for (int i = 0; i < numEnemies; i++)
        {
            var pos = new Vector3(
                Random.Range(-8.0f, 8.0f),
                0.2f,
                Random.Range(-8.0f, 8.0f)
                );

            var rotation = Quaternion.Euler(0f, 0f, 0f);
            //print("...");

            //random = UnityEngine.Random.Range(0, 2);
            //print("randomn is " + random);
            GameObject enemy;


            enemy = (GameObject)Instantiate(enemyPrefab, pos, rotation);

            //if (i == 2) enemy = (GameObject)Instantiate(enemyPrefab, pos, rotation);

            //if (random == 0)
            //{
            //    enemy = (GameObject)Instantiate(enemyPrefab, pos, rotation);
            //    print("switch to enemy");
            //}
            //else
            //{
            //   enemy = (GameObject)Instantiate(tilePrefab, pos, rotation);
            //    print("switch to cube");
            //}



            NetworkServer.Spawn(enemy);

            //var hitCombat = enemy.GetComponent<Combat>();
            //if (hitCombat != null)
            //{
            //    if (i == 1)
            //    {
            //        print("hit!!");
            //        hitCombat.TakeDamage(10);

            //    }
            //    else hitCombat.TakeDamage(50);


            //}


            //tiles.Add(enemy);
            //meshes.Add(enemy.GetComponent<MeshRenderer>());






            }


        }




    }

