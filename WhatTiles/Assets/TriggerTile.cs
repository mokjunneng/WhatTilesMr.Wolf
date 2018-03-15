﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTile : MonoBehaviour {

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);   
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);
    private GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
       
        //print(tileColor.r + " " + tileColor.g + " " + tileColor.b);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.tag == "Player" && gameObject.GetComponent<Renderer>().material.color == blue)
        {
            Debug.Log("Triggerred");
            //tileColor = red;
            gameObject.GetComponent<Renderer>().material.color = red;

            
            other.gameObject.GetComponent<Player>().setTile(gameObject.transform.parent.gameObject);
            //Debug.Log(other.gameObject.GetComponent<Player>().tiles.Count);
            //player.GetComponent<Player>().tiles.Add(gameObject);

        }else if(other.gameObject.tag == "Player blue" && gameObject.GetComponent<Renderer>().material.color == red)
        {
            Debug.Log("wrong trigger");
            gameObject.GetComponent<Renderer>().material.color = blue;
        }
    }
}


