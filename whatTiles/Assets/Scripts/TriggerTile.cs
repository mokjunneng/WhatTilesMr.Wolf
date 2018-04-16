using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TriggerTile : MonoBehaviour {

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);   
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    

    // Use this for initialization
   
	
    //[ClientCallback]
    private void OnCollisionEnter(Collision other)
    {
        playerMovement playerScript = other.gameObject.GetComponent<playerMovement>();

        Vector3 tileCentre = gameObject.GetComponent<Renderer>().bounds.center;
        Vector3 playerPosition = other.gameObject.transform.position;

        print("Distance: " + Vector3.Distance(playerPosition, tileCentre));

        if (Vector3.Distance(playerPosition, tileCentre) <= 1f)
        {
            int index = GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>().playerList.IndexOf(playerScript.id);

            Debug.Log("[Inside Trigger Tile] Player Index is " + playerScript.playerIndex);
            if (index == 1)
            {
                gameObject.GetComponent<Renderer>().material.color = red;
                transform.parent.GetComponent<UpdateTile>().UpdateTiles(playerScript, red);
            }
            else if (index == 0)
            {
                transform.parent.GetComponent<UpdateTile>().UpdateTiles(playerScript, blue);
                gameObject.GetComponent<Renderer>().material.color = blue;
            }
        }
    }

    

}


