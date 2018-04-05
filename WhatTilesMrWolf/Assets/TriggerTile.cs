using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TriggerTile : NetworkBehaviour {

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);   
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);
    private GameObject map;

    // Use this for initialization
    void Start () {
       if (isServer)
        {
            map = GameObject.FindGameObjectWithTag("TileMap");
        }
       
	}
	
    private void OnCollisionEnter(Collision other)
    {
        if (isServer)
        {
            return;
        }
        playerMovement playerScript = other.gameObject.GetComponent<playerMovement>();
    
        Vector3 tileCentre = gameObject.GetComponent<Renderer>().bounds.center;
        Vector3 playerPosition = other.gameObject.transform.position;
      
        print("Distance: " + Vector3.Distance(playerPosition, tileCentre));

        if (Vector3.Distance(playerPosition, tileCentre) <= 0.9f)
        {
            if (playerScript.id %2 == 1)
            { 
                gameObject.GetComponent<Renderer>().material.color = red;
                //update tile counts at server side
                playerScript.CmdUpdateTilesList(gameObject, red);
            }
            else if (playerScript.id %2 == 0)
            {
                gameObject.GetComponent<Renderer>().material.color = blue;
                playerScript.CmdUpdateTilesList(gameObject, blue);
            }
        }
    }

    

}


