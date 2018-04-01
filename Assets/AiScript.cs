using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AiScript : NetworkBehaviour
{

    [SyncVar]
    public bool facingPlayers;

    [SyncVar]
    private float countTimer;

    //overall timer
    [SyncVar]
    public float countDown = 30f;

    //booleans control
    [SyncVar]
    private bool handlingPenalty = false;

    //rotation-related variables
    [SyncVar]
    public float rotationAmount = 180;

    //store the renderers of tiles generated in game
    public GameObject[] tiles;

    private GameObject tileMaster;

    private NetworkIdentity objNetId;

    private GameObject tileObject;

    private bool init = true;

    private bool ordered = false;

    //Track each player
    List<GameObject> playerOrderedList = new List<GameObject>();

    //track tile count
    [SyncVar]
    public int red;

    [SyncVar]
    public int blue;



    // Use this for initialization
    void Start()
    {
        
        countTimer = Random.Range(3f, 6f);
        facingPlayers = false;
    }

    public GameObject[] players;
    private GameObject player;

    public override void OnStartClient()
    {
        
        players = GameObject.FindGameObjectsWithTag("Player");
        print("players " + players.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            return;

        if (init && GameObject.FindGameObjectWithTag("Player") != null)
        {
            init = false;
            player = GameObject.FindGameObjectWithTag("Player");
            tileMaster = GameObject.FindGameObjectWithTag("Tile Master");
            //tiles = tileMaster.GetComponent<TileStorage>().tiles;

        }

        if (!init && GameObject.FindGameObjectWithTag("Player") != null)
        {
            countTimer -= Time.deltaTime;

            if (countTimer <= 0f)
            {
                StartCoroutine(rotate(rotationAmount));

                //to ignore the countdown timer
                countTimer = Mathf.Infinity;
            }

            players = GameObject.FindGameObjectsWithTag("Player");

            //ordering player
            if (!ordered)
            {
                foreach (GameObject p in players)
                {
                    if (p.GetComponent<PlayerMove>().id % 2 == 1)
                    {
                        playerOrderedList.Insert(0, p);
                    }
                    else playerOrderedList.Add(p);
                }

                ordered = true;
            }

            //for tile count


            //for game timer
            countDown -= Time.deltaTime;

            if (countDown <= 0)
            {
                countDown = 0;
            }

            foreach (GameObject p in players)
                p.GetComponent<PlayerMove>().timer = countDown;
        }


    }

    [Command]
    public void CmdPaint(GameObject obj, Color col)
    {
        tileObject = obj;
        objNetId = obj.GetComponent<NetworkIdentity>();        // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);    // assign authority to the player who is changing the color
        //RpcPenalty(col); 
        RpcPenalty(obj, col); // usse a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);    // remove the authority from the player who changed the color
    }

    [ClientRpc]
    void RpcPenalty(GameObject tileObject, Color col)
    {
        tileObject.GetComponent<Renderer>().material.color = col;
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
        foreach (GameObject p in players)
            p.GetComponent<PlayerMove>().spotting ^= true;



        transform.rotation = to;
        resetTimer();

    }

    void resetTimer()
    {
        countTimer = Random.Range(3f, 6f);
    }

    public void addred()
    {
        if (!isServer)
            return;
        red++;
    }

    public void addblue()
    {
        if (!isServer)
            return;
        blue++;
    }

    public void subred()
    {
        if (!isServer)
            return;
        red--;
    }

    public void subblue()
    {
        if (!isServer)
            return;
        blue--;
    }


}

