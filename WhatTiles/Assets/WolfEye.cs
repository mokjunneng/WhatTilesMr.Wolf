
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEye : MonoBehaviour {

    private float countTimer;

    //rotation-related variables  
    public float rotationAmount = 180;
   
    
    //booleans control
    private bool facingPlayers;
    private bool handlingPenalty = false;


    //store the renderers of tiles generated in game
    private List<GameObject> tiles;
    //private Renderer[] tilesRenderers;

    private GameObject player;

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    // Use this for initialization
    void Start () {
        

        countTimer = Random.Range(3f, 6f);
        facingPlayers = false;
        player = GameObject.FindGameObjectWithTag("Player");
        tiles = player.GetComponent<Player>().tiles;
    }
	
	// Update is called once per frame
	void Update () {
        countTimer -= Time.deltaTime;

        //rotate the wolf when the randomized timer is up
        if(countTimer <= 0f)
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

    private IEnumerator givePenaltyToMovingPlayer()
    {
        handlingPenalty = true;
        Debug.Log("giving penalty");
        Renderer lostTile;
        int lostTileCount = 3;

        //Player playerData = player.GetComponent<Player>();
        //List<GameObject> tiles = playerData.tiles;

        //set 3 tiles to opposite colour
        //while (lostTileCount > 0 && tiles.Count > 0)
        //{
        //    GameObject tile = tiles[Random.Range(0, tiles.Count - 1)];

        //    if ( tile.GetComponentInChildren<Renderer>().material.color == red)
        //    {
        //        tile.GetComponentInChildren<Renderer>().material.color = blue;
        //        tiles.Remove(tile);
        //        lostTileCount -= 1;
        //        Debug.Log("minusing tile count");
        //    }
        //}
        Debug.Log(tiles.Count);
        for(int i = 0; i < tiles.Count; i++)
        {
            if(lostTileCount <= 0)
            {
                Debug.Log("exiting penalty loop");
                break;
            }
            GameObject tile = tiles[Random.Range(0, tiles.Count - 1)];
            if (tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color == red)
            {
                tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = blue;
                tiles.Remove(tile);
                lostTileCount -= 1;
                Debug.Log("minusing tile count");
            }
        }

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

        

        transform.rotation = to;
        resetTimer();
     
    }

    void resetTimer()
    {
        countTimer = Random.Range(3f, 6f);
    }


}
