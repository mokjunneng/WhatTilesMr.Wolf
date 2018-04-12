using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerMovement : NetworkBehaviour {

    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;

    private float moveSpeed;

    //for changing speed with power-up
    private float moveSpeedLow = 3f;
    private float moveSpeedHigh = 4.5f;

    private bool highSpeed;
    private float highSpeedTimer;
    private float highSpeedThreshold = 5f;

    public uint id;

    private GameObject WolfAI;
    private NetworkIdentity WolfAiId;

    private bool receivingPenalty;

    //variables that need to be synced
    [SyncVar]
    public bool isMoving = false;
    
    public bool wolfSpotting;

    private bool handlingPenalty = false;

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);
    private GameObject map;

    // For stop button
    private Button stopButton;

    public int playerIndex;

    // Use this for initialization
    void Start () {
        myTransform = transform;     //sets myTransform to this GameObject.Transform
        destinationPos = myTransform.position;    

        id = netId.Value;  //store player's network id
    }

    [Command]
    public void CmdAddWolf(uint playerId)
    {
        playerIndex = GameObject.FindGameObjectWithTag("WolfAI").GetComponent<WolfEye>().addPlayerList(playerId) - 1;
    }

    //for power up
    public void setHighSpeed()
    {
        highSpeed = true;
    }

    public override void OnStartLocalPlayer()
    {
        // Initialising the stop button 
        stopButton = GameObject.FindGameObjectWithTag("StopButton").GetComponent<Button>();
        stopButton.onClick.AddListener(OnClickStop);
        stopButton.gameObject.SetActive(false);
      
        WolfAI = GameObject.FindGameObjectWithTag("WolfAI");
        if(WolfAI == null)
        {
            Debug.Log("No wolf found.");
        }
        CmdAddWolf(netId.Value);

        Debug.Log("playerIndex is " + playerIndex);

        if(playerIndex == 0)
        {
            Debug.Log("BLUE CUBE");
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }else if(playerIndex == 1)
        {
            Debug.Log("RED CUBE");
            GetComponent<MeshRenderer>().material.color = Color.red;
        }


        //wolfSpotting = WolfAI.GetComponent<WolfEye>().facingPlayers;
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

            highSpeedTimer += Time.deltaTime;
            if (highSpeedTimer > highSpeedThreshold)
            {
                highSpeed = false;
                highSpeedTimer = 0f;
            }
        }

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

    void checkIfStopped()
    {
        if (WolfAI.GetComponent<WolfEye>().facingPlayers)
        {
            stopButton.gameObject.SetActive(true);
        }
        else
        {
            stopButton.gameObject.SetActive(false);
        }
    }

    void OnClickStop()
    {
        // reconfirm that the wolfAI.facingPlayers is true 
        if (WolfAI.GetComponent<WolfEye>().facingPlayers)
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
                if (isLocalPlayer)
                {
                    //Handheld.Vibrate();
                }
                StartCoroutine(penaltyInterval());
            }           
            //Debug.Log("player with id " + id + " checking whether receive penalty");
        }
    }

    [Command]
    private void CmdAssignPenalty(uint playerNetId)
    {
        int lostTileCount = 5; //changed to 5 to increase game pace
        Debug.Log("[Inside Penalty] Player Index is " + playerIndex);
        if (playerIndex == 1)
        {
            Debug.Log("Cube is Red, Penalty is Blue");
            List<GameObject> tilesGettingPenalty = map.GetComponent<HexMap>().redTiles;
           
            for (int i = 0; i < lostTileCount; i++)
            {
                GameObject penaltyTile = tilesGettingPenalty[Random.Range(0, tilesGettingPenalty.Count - 1)];

                //Change tile color
                //penaltyTile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = blue;
                RpcPaintTiles(penaltyTile, blue);

                //update tiles list on server
                //CmdUpdateTileCount(penaltyTile);
                map.GetComponent<HexMap>().redTiles.Remove(penaltyTile);
                map.GetComponent<HexMap>().blueTiles.Add(penaltyTile);

            }
        }
        else if (playerIndex == 0)
        {

            Debug.Log("Cube is Blue, Penalty is Red");
            List<GameObject> tilesGettingPenalty = map.GetComponent<HexMap>().blueTiles;
            
            for (int i = 0; i < lostTileCount; i++)
            {
                GameObject penaltyTile = tilesGettingPenalty[Random.Range(0, tilesGettingPenalty.Count - 1)];

                //penaltyTile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = red;
                RpcPaintTiles(penaltyTile, red);

                //CmdUpdateTileCount(penaltyTile);
                map.GetComponent<HexMap>().blueTiles.Remove(penaltyTile);
                map.GetComponent<HexMap>().redTiles.Add(penaltyTile);
            }
        }
        Debug.Log("Red Tiles Count: " + map.GetComponent<HexMap>().redTiles.Count);
        Debug.Log("Blue Tiles Count: " + map.GetComponent<HexMap>().blueTiles.Count);
    }

    [ClientRpc]
    private void RpcPaintTiles(GameObject tile, Color color)
    {
        tile.transform.Find("HexModel").gameObject.GetComponent<Renderer>().material.color = color;
    }

    private IEnumerator penaltyInterval()
    {
        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
    }



    [Command]
    public void CmdUpdateTilesList(GameObject tile, Color color)
    {
        
        //Debug.Log(tile.name);
        //NetworkIdentity tileNetId = tile.GetComponentInParent<NetworkIdentity>();
        //tileNetId.AssignClientAuthority(connectionToClient);

        map = GameObject.FindGameObjectWithTag("TileMap");

        List<GameObject> redTiles = map.GetComponent<HexMap>().redTiles;
        List<GameObject> blueTiles = map.GetComponent<HexMap>().blueTiles;

        if (color == red)
        {

            if (!redTiles.Contains(tile))
            {
                //Debug.Log("updating red ");
                map.GetComponent<HexMap>().redTiles.Add(tile);
                map.GetComponent<HexMap>().blueTiles.Remove(tile);
            }

        }
        else if (color == blue)
        {
            if (!blueTiles.Contains(tile))

            {
                //Debug.Log("updating blue");

                map.GetComponent<HexMap>().blueTiles.Add(tile);
                map.GetComponent<HexMap>().redTiles.Remove(tile);
            }
        }

        //tileNetId.RemoveClientAuthority(connectionToClient);

        Debug.Log("red tiles no: " + redTiles.Count);
        Debug.Log("blue tiles no: " + blueTiles.Count);

    }

    // Functions for Testing
    public float returnSpeed()
    {
        return moveSpeed;
    }
}
