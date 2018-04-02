using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PaintingTiles : NetworkBehaviour
{
    private int range = 200;
    [SerializeField] private Transform camTransform;
    private RaycastHit hit;
    [SyncVar] private Color objectColor;
    [SyncVar] private GameObject objectID;
    private NetworkIdentity objNetId;

    private GameObject tileObject;

    //for tile count
    public GameObject wolf;
    private bool init = true;

    void Update()
    {
        if (!init)
        {
            if (GameObject.FindGameObjectWithTag("GameController"))
            {
                wolf = GameObject.FindGameObjectWithTag("GameController");
                init = true;
            }
        }

        if (isLocalPlayer)
        {
        }
    }

    void Start()
    {
        wolf = GameObject.FindGameObjectWithTag("GameController");
    }

    void CheckIfPainting()
    {
        if (isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            objectID = GameObject.FindGameObjectWithTag("test");//GameObject.Find(hit.transform.name);                                    // this gets the object that is hit
            objectColor = new Color(Random.value, Random.value, Random.value, Random.value);    // I select the color here before doing anything else
            CmdPaint(objectID, objectColor);
        }
    }

    [ClientRpc]
    public void RpcPaint(GameObject obj, Color col)
    {
        if (col == Color.red)
        {
            if (obj.transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material.color == Color.blue)
            {
                wolf.GetComponent<wolfScript>().subblue();
            }

            wolf.GetComponent<wolfScript>().addred();
        }

        else if (col == Color.blue)
        {
            if (obj.transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material.color == Color.red)
            {
                wolf.GetComponent<wolfScript>().subred();
            }

            wolf.GetComponent<wolfScript>().addblue();
        }

        obj.transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material.color = col;
        print("changed to " + col);

    }

    [Command]
    public void CmdPaint(GameObject obj, Color col)
    {
        tileObject = obj;
        objNetId = obj.transform.GetComponentInParent<NetworkIdentity>();         // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);     // assign authority to the player who is changing the color
        RpcPaint(obj.transform.parent.gameObject, col);                                     // use a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);     // remove the authority from the player who changed the color
    }
}

