using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TriggerTile : NetworkBehaviour {

    public Material[] tileMaterials;
    [SyncVar]
    public Color color;

    [SyncVar]
    public int redCount;

    [SyncVar]
    public int blueCount;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        print("what hit me " + other.gameObject.name);

        //inside collide function
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {

            if (other.gameObject.GetComponent<PlayerMovement>().id % 2 == 1)
            {
                if (gameObject.GetComponent<Renderer>().material.color != Color.red)
                {
                    other.gameObject.GetComponent<PaintingTiles>().CmdPaint(gameObject, Color.red);
                }
            }


            else if (other.gameObject.GetComponent<PlayerMovement>().id % 2 == 0)
            {
                if (gameObject.GetComponent<Renderer>().material.color != Color.blue)
                {
                    other.gameObject.GetComponent<PaintingTiles>().CmdPaint(gameObject, Color.blue);
                }
            }
        }
        else
        {
            print("error...");
        }
    }


    public void addBlue()
    {
        if (!isServer)
            return;
        blueCount++;
    }

    public void subBlue()
    {
        if (!isServer || blueCount <= 0)
            return;
         blueCount--;
    }


    public void addRed()
    {
        if (!isServer)
            return;
        redCount++;
    }

    public void subRed()
    {
        if (!isServer || redCount <= 0)
            return;
        redCount--;
    }
}


