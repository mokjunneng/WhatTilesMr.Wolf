
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WolfEye :NetworkBehaviour {

    [SyncVar]
    private float countTimer;

    //rotation-related variables
    [SyncVar]
    public float rotationAmount = 180;

    //booleans control
    private bool handlingPenalty = false;

    //store the renderers of tiles generated in game
    private List<GameObject> tiles;

    private GameObject player;

    private GameObject map;

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    private bool init = true;

    //local network id
    int localId = 5;

    [SyncVar]
    public bool facingPlayers;
    // Use this for initialization
    void Start()
    {
        countTimer = Random.Range(3f, 6f);
        facingPlayers = false;
    }



	// Update is called once per frame
	void Update () {

        if (!isServer)
            return;
        
        else
        {
            if (init && GameObject.FindGameObjectWithTag("TileMap")!=null && GameObject.FindGameObjectWithTag("Player") != null)
            {
                init = false;
                map = GameObject.FindGameObjectWithTag("TileMap");
                player = GameObject.FindGameObjectWithTag("Player");

                //tiles = map.GetComponent<HexMap>().tilesPlayer;
                tiles = map.GetComponent<HexMap>().tiles;

            }

            if(!init && GameObject.FindGameObjectWithTag("TileMap") != null && GameObject.FindGameObjectWithTag("Player") != null)
            {
                countTimer -= Time.deltaTime;

                //rotate the wolf when the randomized timer is up
                if (countTimer <= 0f)
                {
                    StartCoroutine(rotate(rotationAmount));

                    //to ignore the countdown timer
                    countTimer = Mathf.Infinity;
                }

                //if player moving, give penalty

                if (player.GetComponent<playerMovement>().isMoving && facingPlayers)
                {
                    if (!handlingPenalty)
                    {
                        StartCoroutine(givePenaltyToMovingPlayer());
                    }
                }
            }
        }

    }

    //private IEnumerator givePenaltyToMovingPlayer()
    //{
    //    handlingPenalty = true;
    //    Debug.Log("giving penalty");
    //    Renderer lostTile;
    //    int lostTileCount = 3;
    //    Debug.Log(tiles.Count);
    //    for(int i = 0; i < tiles.Count; i++)
    //    {
    //        if(lostTileCount <= 0)
    //        {
    //            Debug.Log("exiting penalty loop");
    //            break;
    //        }
    //        GameObject tile = tiles[Random.Range(0, tiles.Count - 1)];

    //        if (tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color == red)
    //        {
    //            tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = blue;
    //            tiles.Remove(tile);
    //            lostTileCount -= 1;
    //            Debug.Log("minusing tile count");
    //        }
    //        if (tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color == blue)
    //        {
    //            tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = red;
    //            tiles.Remove(tile);
    //            lostTileCount -= 1;
    //            Debug.Log("minusing tile count");
    //        }
    //    }

    //    yield return new WaitForSeconds(1.5f);
    //    handlingPenalty = false;
    //}

    private IEnumerator givePenaltyToMovingPlayer()
    {
        handlingPenalty = true;
        //Debug.Log("giving penalty");
        //Renderer lostTile;


        //Player playerData = player.GetComponent<Player>();
        //List<GameObject> tiles = playerData.tiles;

        
        
        while (player.GetComponent<playerMovement>().isMoving && facingPlayers)
        {

            //int tile = Mathf.RoundToInt(Random.Range(0f, childRenderers.Length - 2));

            GameObject tile = tiles[Random.Range(0, tiles.Count - 1)];


            Renderer r = tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>();


            if (player.GetComponent<playerMovement>().id == localId)
            {
                if (r.material.color == red)
                {
                    RpcPenalty(tile, blue);
                    //r.material.color = blue;
                    //stepTriggerParent.increaseRed(-1);
                }
            }
            else
            {
                if (r.material.color == blue)
                {
                    RpcPenalty(tile, red);
                    //r.material.color = red;
                    //stepTriggerParent.increaseRed(-1);
                }
            }
            yield return new WaitForSeconds(0.1f);

        }
       
        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
    }

    [ClientRpc]
    void RpcPenalty(GameObject tileObject, Color col)
    {
        tileObject.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = col;
    }

    private IEnumerator rotate(float angle, float duration = 0.3f)
    {
        
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(Vector3.up * angle);

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        facingPlayers ^= true;

        

        transform.rotation = to;
        resetTimer();
     
    }

    void resetTimer()
    {
        countTimer = Random.Range(3f, 6f);
    }


}
