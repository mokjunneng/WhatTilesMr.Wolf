using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerMovement : NetworkBehaviour {

    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;

    private float moveSpeed;
    
    public uint id;

    private GameObject WolfAI;
    private NetworkIdentity WolfAiId;

    private bool receivingPenalty;

    //variables that need to be synced
    [SyncVar]
    public bool isMoving;
    [SyncVar]
    public bool wolfSpotting;

    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);
    private GameObject map;

    // Use this for initialization
    void Start () {
        myTransform = transform;     //sets myTransform to this GameObject.Transform
        destinationPos = myTransform.position;    

        id = netId.Value;  //store player's network id
        print("Player network id: " + id);

        isMoving = false;
        

    }

    public override void OnStartLocalPlayer()
    {
        WolfAI = GameObject.FindGameObjectWithTag("WolfAI");
        if(WolfAI == null)
        {
            Debug.Log("No wolf found.");
        }
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) { return; }

        checkIfSpotted();

        //movement control: move upon tapping on screen
        destinationDist = Vector3.Distance(destinationPos, myTransform.position);

        if (destinationDist < .5f)  //prevent shaking behvaior when approaching destination
        {
            moveSpeed = 0;
            isMoving = false;
        }
        else
        {
            moveSpeed = 3;
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

    void checkIfSpotted()
    {
        if (isMoving && wolfSpotting)
        {
            //tell wolf to give penalty to this player
            //WolfAiId = WolfAI.GetComponent<NetworkIdentity>();
            //WolfAiId.AssignClientAuthority(connectionToClient);
            //WolfAI.GetComponent<WolfEye>().CmdCheckIfCanGivePenalty(id);
            //WolfAiId.RemoveClientAuthority(connectionToClient);
            CmdCheckIfGettingPenalty(id);

        }
    }

    [Command]
    void CmdCheckIfGettingPenalty(uint playerId)
    {
        
        WolfAI = GameObject.FindGameObjectWithTag("WolfAI");
        //WolfAiId = WolfAI.GetComponent<NetworkIdentity>();
       // WolfAiId.AssignClientAuthority(connectionToClient);
        WolfAI.GetComponent<WolfEye>().CheckIfCanGivePenalty(playerId);
        //WolfAiId.RemoveClientAuthority(connectionToClient);
    }

    [Command]
    public void CmdUpdateTilesList(GameObject tile, Color color)
    {
        
        Debug.Log(tile.name);
        //NetworkIdentity tileNetId = tile.GetComponentInParent<NetworkIdentity>();
        //tileNetId.AssignClientAuthority(connectionToClient);

        map = GameObject.FindGameObjectWithTag("TileMap");

        List<GameObject> redTiles = map.GetComponent<HexMap>().redTiles;
        List<GameObject> blueTiles = map.GetComponent<HexMap>().blueTiles;

        if (color == red)
        {

            if (!redTiles.Contains(tile))
            {
                Debug.Log("updating red ");
                map.GetComponent<HexMap>().redTiles.Add(tile);
                map.GetComponent<HexMap>().blueTiles.Remove(tile);
            }

        }
        else if (color == blue)
        {
            if (!blueTiles.Contains(tile))

            {
                Debug.Log("updating blue");

                map.GetComponent<HexMap>().blueTiles.Add(tile);
                map.GetComponent<HexMap>().redTiles.Remove(tile);
            }
        }

        //tileNetId.RemoveClientAuthority(connectionToClient);

        Debug.Log("red tiles no: " + redTiles.Count);
        Debug.Log("blue tiles no: " + blueTiles.Count);

    }
}
