using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTileScript : MonoBehaviour {
    public GameObject movable;
    private Transform target;
    private Vector3 destinationPosition;
    private Vector3 startPosition;
    private Vector3 originPosition;
    private float moveSpeed = 3.5f;
    private float timer = 0f;
    private float timerThreshold;
    private bool up;
    private bool triggered;
	// Use this for initialization
	void Start () {
        target = movable.GetComponent<Transform>();
        startPosition = new Vector3(target.position.x, target.position.y, target.position.z);
        originPosition = startPosition;
        timerThreshold = 0.6f;
    }
	
	// Update is called once per frame
	void Update () {
       if (triggered)
        {
            timer += Time.deltaTime;
        }

        //print("Update" + timer);

        if (timer > timerThreshold)
        {
            if (up)
            {
                destinationPosition = originPosition;
            }
            else
            {
                destinationPosition = new Vector3(originPosition.x, originPosition.y + 0.9f, originPosition.z);
            }

            target.position = Vector3.Lerp(target.position, destinationPosition, moveSpeed * Time.deltaTime);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        print("entered");

        if (other.gameObject.tag == "Player Red")
        {
            triggered = true;
            up = !up;    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player Red")
        {
            triggered = false;

            if (timer > timerThreshold)
            {
                if (up)
                {
                    destinationPosition = originPosition;
                }
                else
                {
                    destinationPosition = new Vector3(originPosition.x, originPosition.y + 0.9f, originPosition.z);
                }

                target.position = destinationPosition;
            }

            timer = 0f;
            
        }

    }
}
