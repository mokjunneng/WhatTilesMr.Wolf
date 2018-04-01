using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PaintScript : NetworkBehaviour
{

    private int range = 200;
    [SerializeField] private Transform camTransform;
    private RaycastHit hit;
    [SyncVar] private Color objectColor;
    [SyncVar] private GameObject objectID;
    private NetworkIdentity objNetId;

    private GameObject tileObject;

    //for tile count
    public GameObject Ai;
    private bool init = true;

    void Update()
    {
        if (!init)
        {
            if (GameObject.FindGameObjectWithTag("GameController"))
            {
                Ai = GameObject.FindGameObjectWithTag("GameController");
                init = true;
            }
        }

        if (isLocalPlayer)
        {
            //CheckIfPainting();
        }
    }

    void Start()
    {
        Ai = GameObject.FindGameObjectWithTag("GameController");
        //camTransform = GameObject.FindGameObjectWithTag("test").GetComponent<Transform>();
    }

    void CheckIfPainting()
    {
        if (isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            //if (Physics.Raycast(camTransform.TransformPoint(0, 0, 0.5f), camTransform.forward, out hit, range))
            //{
            objectID = GameObject.FindGameObjectWithTag("test");//GameObject.Find(hit.transform.name);                                    // this gets the object that is hit
                objectColor = new Color(Random.value, Random.value, Random.value, Random.value);    // I select the color here before doing anything else
                CmdPaint(objectID, objectColor);
            //}
        }
    }

    [ClientRpc]
    public void RpcPaint(GameObject obj, Color col)
    {

        if (col == Color.red)
        {
            if (obj.GetComponent<Renderer>().material.color == Color.blue)
            {
                Ai.GetComponent<AiScript>().subblue();
            }

            Ai.GetComponent<AiScript>().addred();
        }
            
        else if (col == Color.blue)
        {
            if (obj.GetComponent<Renderer>().material.color == Color.red)
            {
                Ai.GetComponent<AiScript>().subred();
            }

            Ai.GetComponent<AiScript>().addblue();
        }

        obj.GetComponent<Renderer>().material.color = col;

    }

    //[ClientRpc]
    //void RpcPaint(Color col)
    //{
    //    tileObject.GetComponent<Renderer>().material.color = col;
    //}

    [Command]
    public void CmdPaint(GameObject obj, Color col)
    {
        tileObject = obj;
        objNetId = obj.GetComponent<NetworkIdentity>();        // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);    // assign authority to the player who is changing the color
        //RpcPaint(col); 
        RpcPaint(obj, col); // usse a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);    // remove the authority from the player who changed the color
    }
}

