using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour {

    public const float maxHealth = 5;
    public bool destroyOnDeath;


    [SerializeField]
    GameObject playerUIPrefab;

    [SyncVar]
    public float health = maxHealth;

    [SyncVar]
    public Color color;

    [SyncVar]
    public bool dead;

    void Start()
    {
  
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        //hp 
        if (health > 5 && !dead)
        {
            health = 5;
        }
        else if (health <= 0 && !dead)
        {
            health = 0;
            print("dead");
            TakeDamage(0);
        }
            
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
                    other.gameObject.GetComponent<PaintScript>().CmdPaint(gameObject, Color.red);
                }
            }


            else if (other.gameObject.GetComponent<PlayerMove>().id % 2 == 0)
            {
                if (gameObject.GetComponent<Renderer>().material.color != Color.blue)
                {
                    other.gameObject.GetComponent<PaintScript>().CmdPaint(gameObject, Color.blue);
                }
            }
        }
        else
        {
            //we've collided with something that isn't a ladder, do something else.


        }



    }

    public void addHP()
    {
        health += Time.deltaTime;
    }


    public void TakeDamage(float hpOffset)
    {
        if (!isServer)
            return;

        print(health);


        health -= Time.deltaTime * hpOffset;
        if (health <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                // called on the server, will be invoked on the clients
                RpcRespawn();

            }
        }
    }


    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to spawn location
            StartCoroutine(penaltyWait());


            
        }
    }

    private IEnumerator penaltyWait()
    {
        print("penalty time...");
        dead = true;
        health = 0;
        yield return new WaitForSeconds(3f);
        health = maxHealth;
        print("exit penalty...");
        dead = false;
        yield return new WaitForSeconds(1f);
    }








}
