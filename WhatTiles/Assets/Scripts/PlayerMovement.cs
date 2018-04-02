using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {

    public MeshRenderer meshRenderer;

    private Transform myTransform;              // This transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition

    private Ray ray;
    private Plane playerPlane;

    [SyncVar]
    private float moveSpeed;
    public GameObject wolf;

    [SyncVar]
    public bool isMoving;

    [SyncVar]
    public bool spotting;

    [SyncVar]
    public Color syncColor;

    [SyncVar]
    public float timer;
    [SyncVar]
    int minutes;
    [SyncVar]
    int seconds;

    [SyncVar]
    public bool handlingPenalty = false;

    public uint id;

    private NetworkIdentity objNetId;

    private GameObject tileObject;

    private Text timerText;

    //tile count for both text
    private Text redText;
    private Text blueText;

    private bool init;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;                            // Sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;         // Prevents myTransform reset

        id = netId.Value;
        print(id);

        isMoving = false;

        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();

        redText = GameObject.FindGameObjectWithTag("PlayerRed").GetComponent<Text>();
        blueText = GameObject.FindGameObjectWithTag("PlayerBlue").GetComponent<Text>();
     }

    void Update()
    {
        if (!init)
        {
            if (GameObject.FindGameObjectWithTag("GameController"))
            {
                print("done");
                wolf = GameObject.FindGameObjectWithTag("GameController");
                init = true;
            }
        }

        if (!isLocalPlayer)
            return;

        //time left
        minutes = Mathf.FloorToInt(timer / 60F);
        seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        redText.text = "Red" + wolf.GetComponent<wolfScript>().red.ToString();
        blueText.text = "Blue" + wolf.GetComponent<wolfScript>().blue.ToString();


        // Keep track of the distance between this gameObject and destinationPosition
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < .5f)  //prevent shaking behvaior when approaching destination
        {
            moveSpeed = 0;
            isMoving = false;
        }
        else
        {
            moveSpeed = 3;
            isMoving = true;
        }

        //give penalty to players
        if (!handlingPenalty)
        {
            StartCoroutine(givePenaltyToMovingPlayer());
        }

        // Moves player by click
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            playerPlane = new Plane(Vector3.up, myTransform.position);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 1000 * Time.deltaTime);
            }
        }

        // Prevent code from running if not needed
        if (destinationDistance > .5f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
        }
    }

    [Command]
    public void CmdPaint(GameObject obj, Color col)
    {
        tileObject = obj;
        objNetId = obj.GetComponent<NetworkIdentity>();         // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);     // assign authority to the player who is changing the color
        RpcPenalty(obj, col);                                   // use a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);     // remove the authority from the player who changed the color
    }

    [ClientRpc]
    void RpcPenalty(GameObject tileObject, Color col)
    {
        if (tileObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == Color.red)
        {
            if (col == Color.blue)
            {
                wolf.GetComponent<wolfScript>().blue++;
                wolf.GetComponent<wolfScript>().red--;
              
                tileObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = col;
            }


        }
        else if (tileObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == Color.blue)
        {
            if (col == Color.red)
            {

                wolf.GetComponent<wolfScript>().blue--;
                wolf.GetComponent<wolfScript>().red++;
           
                tileObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = col;
            }


        }

    }


    public override void OnStartLocalPlayer()
    {
        meshRenderer.material.color = Color.red;
    }

    private IEnumerator givePenaltyToMovingPlayer()
    {
        handlingPenalty = true;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        while (isMoving && spotting)
        {
            GameObject tile = tiles[Random.Range(0, tiles.Length)];
            if (id % 2 == 1)
            {
                CmdPaint(tile, Color.blue);
            }
            else
            {
                CmdPaint(tile, Color.red);
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
    }
}
