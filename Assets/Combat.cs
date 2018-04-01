using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour {

    public const int maxHealth = 100;
    public bool destroyOnDeath;

    public Material[] tileMaterials;


    [SyncVar]
    public int health = maxHealth;

    [SyncVar]
    public int random = 0;

    [SyncVar]
    public Color color;

    [SyncVar]
    public int redCount;

    [SyncVar]
    public int blueCount;



    //display score
    //private GameObject display;
    //private Text text;



    void Start()
    {

        
        //display = GameObject.FindGameObjectWithTag("Tile Count");
        //text = display.GetComponent<Text>();
    }

    void Update()
    {
        //keep track of score
        //text.text = redCount.ToString();




    }

    void OnTriggerEnter(Collider other)
    {
        //gameObject.GetComponentInParent<StepTriggerParent>().UpdateTiles(c);

        //Debug.Log("Triggered on: " + gameObject.GetComponent<Renderer>().material.color);

        print("what hit me " + other.gameObject.name);

        //inside your collide function
        if (other.gameObject.GetComponent<PlayerMove>() != null)
        {

            if (other.gameObject.GetComponent<PlayerMove>().id % 2 == 1)
            {
                print(other.gameObject.name);
                if (gameObject.GetComponent<Renderer>().material.color != Color.red)
                {
                    //print(other.gameObject.name + "red count" + redCount);
                    //print(other.gameObject.name + "blue count" + blueCount);
                    other.gameObject.GetComponent<PaintScript>().CmdPaint(gameObject, Color.red);
                    //Cmdcountred();

                    
                    //Ai.GetComponent<AiScript>().addred();

                    //other.GetComponent<Combat>().addRed();
                    //other.GetComponent<Combat>().TakeDamage(-10);
                }


                //gameObject.GetComponent<Renderer>().material.color = Color.red;
                //parent.increaseRed(1);
            }


            else if (other.gameObject.GetComponent<PlayerMove>().id % 2 == 0)
            {
                //print(other.gameObject.name + "red count" + redCount);
                //print(other.gameObject.name + "blue count" + blueCount);
                if (gameObject.GetComponent<Renderer>().material.color != Color.blue)
                {
                    other.gameObject.GetComponent<PaintScript>().CmdPaint(gameObject, Color.blue);
                    //Cmdcountblue();

                    //Ai.GetComponent<AiScript>().addblue();

                    //other.GetComponent<Combat>().addBlue();

                    //other.GetComponent<Combat>().TakeDamage(-10);
                }



                //gameObject.GetComponent<Renderer>().material.color = Color.blue;
                //parent.blueTile++;

            }
        }
        else
        {
            //we've collided with something that isn't a ladder, do something else.


        }



    }


    public void addBlue() {
        if (!isServer)
            return;
        //print("add blue");
        blueCount++;


        
    }

    public void subBlue()
    {
        if (!isServer || blueCount <= 0)
            return;
        //print("sub blue");
        blueCount--;



    }


    public void addRed(){
        if (!isServer)
            return;
        //print("add red");
        redCount++;
    }

    public void subRed()
    {
        if (!isServer || redCount <= 0)
            return;
        //print("sub red");
        redCount--;
    }


    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;


        print("look i am here");
        



        health -= amount;
        print(health);
        if (health <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                health = maxHealth;

                // called on the server, will be invoked on the clients
                RpcRespawn();
            }
        }
    }


    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }








}
