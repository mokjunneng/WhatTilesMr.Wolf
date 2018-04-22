﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// class for changing of tile color upon contact
public class TriggerTile : MonoBehaviour {

    private Color yellow = new Color(255f / 255f, 195f / 255f, 84f / 255f, 255f / 255f);
    private Color blue = new Color(0, 174f / 255f, 178f / 255f, 255f / 255f);
    private WolfEye WolfAI;

    private void Start()
    {
        WolfAI = GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>();
    }

    private void OnCollisionEnter(Collision other)
    {
        playerMovement playerScript = other.gameObject.GetComponent<playerMovement>();

        Vector3 tileCentre = gameObject.GetComponent<Renderer>().bounds.center;
        Vector3 playerPosition = other.gameObject.transform.position;

        if (Vector3.Distance(playerPosition, tileCentre) <= 1f)
        {
            int index = WolfAI.playerList.IndexOf(playerScript.id);

            //Debug.Log("[Inside Trigger Tile] Player Index is " + playerScript.playerIndex);
            if (index == 1)
            {
                gameObject.GetComponent<Renderer>().material.color = yellow;
                transform.parent.GetComponent<UpdateTile>().UpdateTiles(playerScript, yellow);
            }
            else if (index == 0)
            {
                transform.parent.GetComponent<UpdateTile>().UpdateTiles(playerScript, blue);
                gameObject.GetComponent<Renderer>().material.color = blue;
            }
        }
    }

    

}


