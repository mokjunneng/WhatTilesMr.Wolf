using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TriggerTile : NetworkBehaviour {

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);   
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    //local network id
    int localId = 5;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnCollisionEnter(Collision other)
    //{

    //    if (other.gameObject.GetComponent<playerMovement>().id % 2 == 1 && gameObject.GetComponent<Renderer>().material.color != red)
    //    {
   
    //        other.gameObject.GetComponent<PaintScript>().CmdPaint(gameObject, red);
    //        //gameObject.GetComponent<Renderer>().material.color = Color.red;
    //        //parent.increaseRed(1);
    //    }


    //    else if (other.gameObject.GetComponent<playerMovement>().id % 2 == 0 && gameObject.GetComponent<Renderer>().material.color != blue)
    //    {
    //        other.gameObject.GetComponent<PaintScript>().CmdPaint(gameObject, blue);
    //        //gameObject.GetComponent<Renderer>().material.color = blue;
    //        //parent.blueTile++;

    //    }
    //}
}


