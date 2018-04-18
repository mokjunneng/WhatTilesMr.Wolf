using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerMovement : NetworkBehaviour {

    //GameObjec references
    private GameObject WolfAI;
    private GameObject map;

    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;
    public uint id;

    //for changing speed with power-up
    private float moveSpeed;
    private float moveSpeedLow = 3f;
    private float moveSpeedHigh = 4.5f;

    private bool highSpeed;
    private float highSpeedTimer;
    private float highSpeedThreshold = 5f;

    //variables that need to be synced
    [SyncVar]
    public bool isMoving = false;
    private bool handlingPenalty = false;

    //private Color yellow = new Color(255f/255f, 227f / 255f, 0, 255f / 255f);//new Color(1F, 0.1911765F, 0.1911765F);
    //private Color blue = new Color(0, 208f / 255f, 113f / 255f, 255f / 255f);//new Color(0.3317474F, 0.6237204F, 0.8676471F);
    //Color schemes for tiles
    private Color yellow = new Color(255f / 255f, 195f / 255f, 84f / 255f, 255f / 255f);
    private Color blue = new Color(0, 174f / 255f, 178f / 255f, 255f / 255f);


    //UI - stop buttons
    private Button stopButtonL;
    private Button stopButtonR;

    //Sound Effects
    AudioSource audioSource;
    public AudioClip penaltySE;

    public int playerIndex;

    //For HP (if have time then add so just leave here first)
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
        id = netId.Value;  //store player's network id
        audioSource = GetComponent<AudioSource>();
    }

    

    //cache players' index
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
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (playerIndex == 1)
        {
            //Debug.Log("RED CUBE");
            GetComponent<MeshRenderer>().material.color = new Color(255,42,0,255);
        }
    }

    //for power up
    public void setHighSpeed()
    {
        highSpeed = true;
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

        WolfAI = GameObject.FindGameObjectWithTag("WolfAI");
        if (WolfAI == null)
        {
            Debug.Log("No wolf found.");
        }
        if (isServer)
        {
            RpcAddList(netId.Value);
            //WolfAI.GetComponent<WolfEye>().playerList.Add(netId.Value);
        }
        else
        {
            CmdAddWolf(netId.Value);
        }
        //slider = GameObject.Find("Slider").GetComponent<Slider>();
    }


    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) { return; }

        checkIfStopped();
        checkIfSpotted();
        

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

                
                //remove speed status
                //transform.GetComponentInChildren<Image>().enabled = false;

                //stop aura
                //GetComponent<ParticleSystem>().Stop();
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

        if (!dead && GameObject.Find("Canvas").GetComponent<GameOverManager>().startTimer)
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
                //moveSpeed = 3;

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

    //show stop button when wolf starts vibrating
    void checkIfStopped()
    {
        if (WolfAI.GetComponent<WolfEye>().vibrating)
        {
            if (GameObject.Find("Canvas").GetComponent<GameOverManager>().getTimer() > 0)
            {
                stopButtonR.gameObject.SetActive(true);
                stopButtonL.gameObject.SetActive(true);
            }

        }
        else
        {
            stopButtonR.gameObject.SetActive(false);
            stopButtonL.gameObject.SetActive(false);
        }
    }

    void OnClickStop()
    {
        // reconfirm that the wolfAI.facingPlayers is true 
        if (WolfAI.GetComponent<WolfEye>().vibrating)
        {
            destinationPos = myTransform.position;

        }
    }

    void checkIfSpotted()
    {
        //spotted moving
        if (isMoving && WolfAI.GetComponent<WolfEye>().facingPlayers)
        {             
            if (!handlingPenalty)
            {
                handlingPenalty = true;
                CmdAssignPenalty(id);
                //play SE
                audioSource.PlayOneShot(penaltySE);

                if (isLocalPlayer)
                {
                    //Handheld.Vibrate();
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
        if (GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>().playerList.IndexOf(netId.Value) == 1)
        {
            //Debug.Log("Cube is Red, Penalty is Blue");
            RpcPlayCounterAnimation("minusFiveYellow");
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
        else if (GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>().playerList.IndexOf(netId.Value) == 0)
        {
            RpcPlayCounterAnimation("minusFiveBlue");
            //Debug.Log("Cube is Blue, Penalty is Red");
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
        //Renderer tileRenderer = tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>();
        //tileRenderer.material.color = Color.Lerp(tileRenderer.material.color, color, Mathf.PingPong(Time.time, 1));
        StartCoroutine(tileColorFade(tile, color));
    }

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

    private IEnumerator penaltyInterval()
    {
        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
    }



    [Command]
    public void CmdUpdateTilesList(GameObject tile, Color color)
    {

        map = GameObject.FindGameObjectWithTag("TileMap");

        List<GameObject> redTiles = map.GetComponent<HexMap>().redTiles;
        List<GameObject> blueTiles = map.GetComponent<HexMap>().blueTiles;

        if (color == yellow)
        {

            if (!redTiles.Contains(tile))
            {
                //Debug.Log("updating red ");
                map.GetComponent<HexMap>().redTiles.Add(tile);
                map.GetComponent<HexMap>().blueTiles.Remove(tile);
                RpcPlayCounterAnimation("plusOneYellow");
            }

        }
        else if (color == blue)
        {
            if (!blueTiles.Contains(tile))

            {
                //Debug.Log("updating blue");
                map.GetComponent<HexMap>().blueTiles.Add(tile);
                map.GetComponent<HexMap>().redTiles.Remove(tile);
                RpcPlayCounterAnimation("plusOneBlue");
            }
        }

        float ratio = (float) blueTiles.Count / map.GetComponent<HexMap>().tiles.Count;
        RpcUpdateCounter(ratio);

        Debug.Log("red tiles no: " + redTiles.Count);
        Debug.Log("blue tiles no: " + blueTiles.Count);

    }

    [ClientRpc]
    private void RpcUpdateCounter(float ratio)
    {
        GameObject.Find("Canvas").GetComponent<GameOverManager>().bar.fillAmount = ratio;
    }

    [ClientRpc]
    private void RpcPlayCounterAnimation(string animationName)
    {
        Debug.Log("play animation");
        GameObject.Find("Canvas").GetComponent<GameOverManager>().anim.Play(animationName);
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

    //Additional functions for health feature
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
