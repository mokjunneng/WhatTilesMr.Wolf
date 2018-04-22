﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerMovement : NetworkBehaviour {

    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;

    // network id
    public uint id;
    public int playerIndex;

    // for changing speed with power-up
    private float moveSpeed;
    private float moveSpeedLow = 3f;
    private float moveSpeedHigh = 4.5f;

    private bool highSpeed;
    private float highSpeedTimer;
    private float highSpeedThreshold = 5f;


    private WolfEye WolfAI;
    private GameObject map;
    private GameOverManager gameUiManager;

    // variables used for handling penalty
    [SyncVar]
    public bool isMoving = false;
    public bool wolfSpotting;
    private bool handlingPenalty = false;
    private bool receivingPenalty;

    private Color yellow = new Color(255f / 255f, 195f / 255f, 84f / 255f, 255f / 255f);
    private Color blue = new Color(0, 174f / 255f, 178f / 255f, 255f / 255f);
    
    // For stop button
    private Button stopButtonL;
    private Button stopButtonR;

    // For Sound Effects (SE)
    AudioSource audioSource;
    public AudioClip penaltySE;
    public AudioClip tileSE;

    // For HP (if have time then add so just leave here first)
    public const float maxHealth = 5;
    //[SyncVar(hook = "OnChangeHealth")]
    public float health = maxHealth;
    //[SerializeField]
    private Slider slider;
    private float hpOffset = 1f;

    [SyncVar]
    public bool dead;

    void Start () {
        myTransform = transform;     //sets myTransform to this GameObject.Transform
        destinationPos = myTransform.position;    
        id = netId.Value;  
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnStartServer()
    {
        map = GameObject.FindGameObjectWithTag("TileMap");
        WolfAI = GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>();
    }

    public override void OnStartLocalPlayer()
    {
        // Initialising the stop button 
        stopButtonL = GameObject.FindGameObjectWithTag("StopButton").GetComponent<Button>();
        stopButtonL.onClick.AddListener(OnClickStop);
        stopButtonL.gameObject.SetActive(false);

        stopButtonR = GameObject.FindGameObjectWithTag("StopButtonR").GetComponent<Button>();
        stopButtonR.onClick.AddListener(OnClickStop);
        stopButtonR.gameObject.SetActive(false);

        //hide power-up indicator
        transform.GetComponentInChildren<Image>().enabled = false;

        gameUiManager = GameObject.Find("Canvas").GetComponent<GameOverManager>();

        WolfAI = GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>();
        if (WolfAI == null)
        {
            Debug.Log("No wolf found.");
        }
        if (isServer)
        {
            RpcAddList(netId.Value);
        }
        else
        {
            CmdAddWolf(netId.Value);
        }
        //slider = GameObject.Find("Slider").GetComponent<Slider>();
    }


    // Functions - {RpcAddList, CmdAddWolf, IndexChanged} are used to cache player's identity and set the player object's color correspondingly
    // eg. Player 1 is Blue, Player 2 is Yellow
    [ClientRpc]
    public void RpcAddList(uint playerId)
    {
        CmdAddWolf(playerId);
    }

    [Command]
    public void CmdAddWolf(uint playerId)
    {
        GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>().playerList.Add(playerId);
    }

    public void indexChanged(int index)
    {
        if (!isLocalPlayer) { return; }
        playerIndex = index;
        //Debug.Log("playerIndex is " + playerIndex);

        if (playerIndex == 0)
        {
            //Debug.Log("BLUE CUBE");
            GetComponent<MeshRenderer>().material.color = new Color(0, 214f / 255f, 178f / 255f, 255f / 255f);

            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0, 214f / 255f, 178f / 255f, 255f / 255f);
        }
        else if (playerIndex == 1)
        {
            //Debug.Log("RED CUBE");
            GetComponent<MeshRenderer>().material.color = new Color(1,224f / 255f ,41f /255f,1);
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 224f /255f, 42f /255f, 1);

        }
    }

    //for power up
    public void setHighSpeed()
    {
        highSpeed = true;
    }

    void Update () {
        if (!isLocalPlayer) { return; }

        checkIfWolfSpotting();
        checkIfSpotted();

        //hide indicator
        if (gameUiManager.getTimer() < 47)
        {
            CmdHideIndicator();
        }

        //movement control: move upon tapping on screen
        destinationDist = Vector3.Distance(destinationPos, myTransform.position);

        //for power up
        if (highSpeed)
        {
            //display speed status
            //transform.GetComponentInChildren<Image>().enabled = true;
            CmdShowSpeed();

            highSpeedTimer += Time.deltaTime;
            if (highSpeedTimer > highSpeedThreshold)
            {
                highSpeed = false;
                highSpeedTimer = 0f;

                CmdHideSpeed();
            }
        }

        //for hp 
        //if (health > 5 && !dead)
        //{
        //    CmdSetMax();
        //}
        //else if (health <= 0 && !dead)
        //{
        //    print("dead");
        //    CmdSetZero();
        //}

        if (!dead && gameUiManager.startTimer)
        {
            //for HP
            //if (!isMoving)
            //{
            //    CmdTakeHealth();
            //}
            //else
            //{
            //    CmdAddHP();
            //}

            if (destinationDist < .5f)  //prevent shaking behvaior when approaching destination
            {
                moveSpeed = 0;
                isMoving = false;
            }
            else
            {
                //set speed accordingly
                if (highSpeed)
                {
                    moveSpeed = moveSpeedHigh;
                }
                else
                {
                    moveSpeed = moveSpeedLow;
                }

                isMoving = true;
            }

            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 targetPoint = ray.GetPoint(hit.distance);
                    destinationPos = ray.GetPoint(hit.distance);
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                    myTransform.rotation = targetRotation;

                }
            }

            if (destinationDist > .5f)
            {
                myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPos, moveSpeed * Time.deltaTime);
            }
        }

    }

    //If wolf facing players, show the stop buttons
    void checkIfWolfSpotting()
    {
        if (WolfAI.vibrating && gameUiManager.countdownTimer > 0)
        {
            stopButtonR.gameObject.SetActive(true);
            stopButtonL.gameObject.SetActive(true);
        }
        else
        {
            stopButtonR.gameObject.SetActive(false);
            stopButtonL.gameObject.SetActive(false);
        }
    }

    void OnClickStop()
    {
        if (WolfAI.vibrating)
        {
            destinationPos = myTransform.position;
        }
    }

    void checkIfSpotted()
    {
        //spotted moving
        if (isMoving && WolfAI.facingPlayers)
        {             
            if (!handlingPenalty)
            {
                handlingPenalty = true;
                CmdAssignPenalty(id);
                //play SE
                audioSource.PlayOneShot(penaltySE);

                if (isLocalPlayer)
                {
                    Handheld.Vibrate();
                }
                StartCoroutine(penaltyInterval());
            }           
        }
    }

    [Command]
    private void CmdAssignPenalty(uint playerNetId)
    {
        int lostTileCount = 5; //changed to 5 to increase game pace
        //Debug.Log("[Inside Penalty] Player Index is " + playerIndex);
        if (WolfAI.playerList.IndexOf(netId.Value) == 1)
        {
            RpcPlayAnimation("minusFiveBlue");

            //Debug.Log("Cube is Red, Penalty is Blue");
            List<GameObject> tilesGettingPenalty = map.GetComponent<HexMap>().redTiles;
           
            for (int i = 0; i < lostTileCount; i++)
            {
                GameObject penaltyTile = tilesGettingPenalty[Random.Range(0, tilesGettingPenalty.Count - 1)];

                //Change tile color
                RpcPaintTiles(penaltyTile, blue);

                //update tiles list on server
                map.GetComponent<HexMap>().redTiles.Remove(penaltyTile);
                map.GetComponent<HexMap>().blueTiles.Add(penaltyTile);

            }
        }
        else if (WolfAI.playerList.IndexOf(netId.Value) == 0)
        {
            RpcPlayAnimation("minusFiveYellow");

            Debug.Log("Cube is Blue, Penalty is Red");
            List<GameObject> tilesGettingPenalty = map.GetComponent<HexMap>().blueTiles;
            
            for (int i = 0; i < lostTileCount; i++)
            {
                GameObject penaltyTile = tilesGettingPenalty[Random.Range(0, tilesGettingPenalty.Count - 1)];

                RpcPaintTiles(penaltyTile, yellow);

                map.GetComponent<HexMap>().blueTiles.Remove(penaltyTile);
                map.GetComponent<HexMap>().redTiles.Add(penaltyTile);
            }
        }
        //Debug.Log("Red Tiles Count: " + map.GetComponent<HexMap>().redTiles.Count);
        //Debug.Log("Blue Tiles Count: " + map.GetComponent<HexMap>().blueTiles.Count);
    }

    [ClientRpc]
    private void RpcPaintTiles(GameObject tile, Color color)
    {
        StartCoroutine(tileColorFade(tile, color));
    }

    // For color fade visuals
    private IEnumerator tileColorFade(GameObject tile, Color color)
    {
        float timer = 0f;
        float duration = 1.5f;

        Renderer tileRenderer = tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>();

        while (timer < duration)
        {
            tileRenderer.material.color = Color.Lerp(tileRenderer.material.color, color, timer);
            timer += Time.deltaTime;
            yield return null;
        }
       
    }

    // A buffer time before a new penalty is assigned
    private IEnumerator penaltyInterval()
    {
        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
    }


    [Command]
    public void CmdUpdateTilesList(GameObject tile, Color color, uint playerId)
    {
        List<GameObject> redTiles = map.GetComponent<HexMap>().redTiles;
        List<GameObject> blueTiles = map.GetComponent<HexMap>().blueTiles;

        if (color == yellow)
        {

            if (!redTiles.Contains(tile))
            {
                //Debug.Log("updating red ");
                map.GetComponent<HexMap>().redTiles.Add(tile);
                map.GetComponent<HexMap>().blueTiles.Remove(tile);
                RpcPlaySound(playerId);
                RpcPlayAnimation("plusOneYellow");
            }

        }
        else if (color == blue)
        {
            if (!blueTiles.Contains(tile))

            {
                //Debug.Log("updating blue");
                map.GetComponent<HexMap>().blueTiles.Add(tile);
                map.GetComponent<HexMap>().redTiles.Remove(tile);
                RpcPlaySound(playerId);
                RpcPlayAnimation("plusOneBlue");
            }
        }

        float ratio = (float) blueTiles.Count / map.GetComponent<HexMap>().tiles.Count;
        RpcUpdateCounter(ratio);

        //Debug.Log("red tiles no: " + redTiles.Count);
        //Debug.Log("blue tiles no: " + blueTiles.Count);
    }

    [ClientRpc]
    private void RpcPlaySound(uint playerId)
    {
        Debug.Log("playsound");
        if (playerId == id)
        {
            Debug.Log("id confirmed, playing sound");
            audioSource.PlayOneShot(tileSE);
        }
       
    }

    //update bar counter in game
    [ClientRpc]
    private void RpcUpdateCounter(float ratio)
    {
        GameObject.Find("Canvas").GetComponent<GameOverManager>().bar.fillAmount = ratio;
    }

    [ClientRpc]
    private void RpcPlayAnimation(string name)
    {
        Debug.Log("play animation");
        GameObject.Find("Canvas").GetComponent<GameOverManager>().anim.SetTrigger(name);
    }

    [Command]
    private void CmdShowSpeed()
    {
        RpcShowSpeed();

    }

    [ClientRpc]
    private void RpcShowSpeed()
    {
        //display speed status
        transform.GetComponentInChildren<Image>().enabled = true;
    }

    [Command]
    private void CmdHideSpeed()
    {
        RpcHideSpeed();
        
    }

    [ClientRpc]
    private void RpcHideSpeed()
    {
        //display speed status
        transform.GetComponentInChildren<Image>().enabled = false;
    }

    [Command]
    private void CmdHideIndicator()
    {
        RpcHideIndicator();
    }

    [ClientRpc]
    private void RpcHideIndicator()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Functions for Testing
    public float returnSpeed()
    {
        return moveSpeed;
    }

    public void OnChangeHealth(float health)
    {
        slider.value = health;
    }

    public void addHP()
    {
        if (!isServer)
            return;

        health += Time.deltaTime;
    }

    //Additional functions for health feature (not implemented)
    [Command]
    void CmdTakeHealth()
    {
        //Apply damage to the GameObject
        takeDamage(hpOffset);
    }

    [Command]
    void CmdAddHP()
    {
        addHP();
    }

    [Command]
    void CmdSetMax()
    {
        if (!isServer)
            return;
        health = maxHealth;
    }

    [Command]
    void CmdSetZero()
    {
        if (!isServer)
            return;
        health = 0;
    }

    public void takeDamage(float hpOffset)
    {
        if (!isServer)
            return;

        health -= Time.deltaTime * hpOffset;
        if (health <= 0)
        {
            // called on the server, will be invoked on the clients
            RpcRespawn();

        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // pause object for a while
            StartCoroutine(penaltyWait());

        }
    }

    private IEnumerator penaltyWait()
    {
        print("penalty time...");
        dead = true;
        CmdSetZero();
        yield return new WaitForSeconds(3f);
        CmdSetMax();
        print("exit penalty...");
        dead = false;
        yield return new WaitForSeconds(1f);
    }

    
}
