using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour {

    Transform parent;
    private float h = 1.1f;

    // Use this for initialization
    void Start () {
        parent = transform.parent.parent;
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
        Vector3 worldPosition = new Vector3(parent.position.x, parent.position.y + h, parent.position.z);
        transform.position = worldPosition;
    }
}
