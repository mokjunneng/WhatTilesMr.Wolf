using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlightTiles : MonoBehaviour {

    private Vector2 touchCoordinates;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            RaycastHit hit;

            Vector3 touchPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0f);

            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            //check which tile is touched
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        touchCoordinates = Input.GetTouch(0).position;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {

                    }
                }
            }

        }
    }
}
