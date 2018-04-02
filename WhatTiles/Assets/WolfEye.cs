
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WolfEye :NetworkBehaviour {

    private float countTimer;

    //rotation-related variables  
    public float rotationAmount = 180;
   
    
    //booleans control
    private bool handlingPenalty = false;
    private bool facingPlayers; //might be redundant
    private bool ordered = false;

    //store the renderers of tiles generated in game
    private List<GameObject> tiles;
    private List<GameObject> tilesGettingPenalty;
    //private Renderer[] tilesRenderers;

    private GameObject player;
    private GameObject map;
    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    private bool init = true;
    public GameObject[] players;
    List<GameObject> playerOrderedList = new List<GameObject>();
    
    void Start()
    {
        countTimer = Random.Range(3f, 6f);
        facingPlayers = false;
        //map = GameObject.FindGameObjectWithTag("TileMap");
    }



	// Update is called once per frame
	//void Update () {

 //       if (!isServer)
 //           return;
        
 //       else
 //       {
 //           if (init && GameObject.FindGameObjectWithTag("TileMap")!=null && GameObject.FindGameObjectWithTag("Player") != null)
 //           {
 //               init = false;
 //               map = GameObject.FindGameObjectWithTag("TileMap");
 //               player = GameObject.FindGameObjectWithTag("Player");
 //               tiles = map.GetComponent<HexMap>().tilesPlayer;
 //           }

 //           if(!init && GameObject.FindGameObjectWithTag("TileMap") != null && GameObject.FindGameObjectWithTag("Player") != null)
 //           {
 //               countTimer -= Time.deltaTime;

 //               //rotate the wolf when the randomized timer is up
 //               if (countTimer <= 0f)
 //               {
 //                   StartCoroutine(rotate(rotationAmount));

 //                   //to ignore the countdown timer
 //                   countTimer = Mathf.Infinity;
 //               }

 //               //if player moving, give penalty

 //               if (player.GetComponent<playerMovement>().isMoving && facingPlayers)
 //               {
 //                   if (!handlingPenalty)
 //                   {
 //                       StartCoroutine(givePenaltyToMovingPlayer());
 //                   }
 //               }
 //           }
 //       }

 //   }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (init && GameObject.FindGameObjectWithTag("Player") != null)
        {
            init = false;
            map = GameObject.FindGameObjectWithTag("TileMap");
            //tiles = map.GetComponent<HexMap>().tilesPlayer;
            
        }
        else if (!init && GameObject.FindGameObjectWithTag("Player") != null)
        {
            //get reference to players in game from server
            players = GameObject.FindGameObjectsWithTag("Player");

            countTimer -= Time.deltaTime;

            if (countTimer <= 0f)
            {
                StartCoroutine(rotate(rotationAmount));

                countTimer = Mathf.Infinity;
            }

            //order players
            if (!ordered)
            {
                foreach (GameObject p in players)
                {
                    if (p.GetComponent<playerMovement>().id % 2 == 1)
                    {
                        playerOrderedList.Insert(0, p);
                    }
                    else playerOrderedList.Add(p);
                }

                ordered = true;
            }
        }
        
    }

    

    
    public void CheckIfCanGivePenalty(uint playerNetId)
    {
        if (!handlingPenalty)
        {
            if (playerNetId == 105)
            {
                tilesGettingPenalty = map.GetComponent<HexMap>().redTiles;
            }
            else if (playerNetId == 106)
            {
                tilesGettingPenalty = map.GetComponent<HexMap>().blueTiles;
            }

            int lostTileCount = 3;
            for (int i = 0; i < lostTileCount; i++)
            {
                GameObject penaltyTile = tilesGettingPenalty[Random.Range(0, tilesGettingPenalty.Count - 1)];
                RpcGivePenaltyToMovingPlayer(penaltyTile);
                tilesGettingPenalty.Remove(penaltyTile);
            }
            
        }
    }

    [ClientRpc]
    private void RpcGivePenaltyToMovingPlayer(GameObject tile)
    {
        handlingPenalty = true;
        if (tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color == red)
        {
            tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = blue;
            //tilesGettingPenalty.Remove(tile);
           
            Debug.Log("minusing tile count - red");
        }
        else if (tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color == blue)
        {
            tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = red;
            //tilesGettingPenalty.Remove(tile);
                
            Debug.Log("minusing tile count - blue");
        }

        StartCoroutine(penaltyInterval());
    }

    private IEnumerator penaltyInterval()
    {
        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
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

        foreach (GameObject p in players)
            p.GetComponent<playerMovement>().wolfSpotting ^= true;

        transform.rotation = to;
        resetTimer();
     
    }

    void resetTimer()
    {
        countTimer = Random.Range(3f, 6f);
    }


}
