using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;

    private float moveSpeed;
    public bool isMoving;

	// Use this for initialization
	void Start () {
        myTransform = transform;
        destinationPos = myTransform.position;
	}
	
	// Update is called once per frame
	void Update () {

        destinationDist = Vector3.Distance(destinationPos, myTransform.position);

        if(destinationDist < .5f)  //prevent shaking behvaior when approaching destination
        {
            moveSpeed = 0;
            isMoving = false;
        }
        else
        {
            moveSpeed = 3;
            isMoving = true;
        }

		if(Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                Vector3 targetPoint = ray.GetPoint(hit.distance);
                destinationPos = ray.GetPoint(hit.distance);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                myTransform.rotation = targetRotation;

            }
        }

        if(destinationDist > .5f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPos, moveSpeed * Time.deltaTime);
        }
	}
}
