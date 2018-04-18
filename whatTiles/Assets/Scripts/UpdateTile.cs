using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UpdateTile : NetworkBehaviour {

    private GameObject map;
    private GameObject[] players;
    private GameObject clientPlayer;
    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    // For SE
    AudioSource audioSource;
    public AudioClip tileSE;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void UpdateTiles(playerMovement player, Color color)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<playerMovement>().isLocalPlayer)
            {
                //Debug.Log("found client player");
                clientPlayer = p;
            }
        }
        
        //only allow updatetileslist funciton to call from local player
        if (clientPlayer != null && player.id == clientPlayer.GetComponent<playerMovement>().id)
        {
            Debug.Log("Updating tiles");
            player.CmdUpdateTilesList(gameObject, color, player.id);
        }

    }
}
