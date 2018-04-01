using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    private Transform myTransform;              // This transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition

    private Ray ray;
    private Plane playerPlane;

    private float moveSpeed = 1.0f;

    [SyncVar] private Color syncColor;
    public GameObject myGO;

    // Use this for initialization
    void Start () {
        myTransform = transform;                            // Sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;         // Prevents myTransform reset



    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
            return;

        // Keep track of the distance between this gameObject and destinationPosition
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        // Moves player by click... can be used in mobile devices also
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            playerPlane = new Plane(Vector3.up, myTransform.position);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                //Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 1000 * Time.deltaTime);
                //myTransform.rotation = targetRotation;
            }
        }

        // Prevent code from running if not needed
        if (destinationDistance > .5f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
            print(destinationDistance + "> .5f");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }





    }




}
