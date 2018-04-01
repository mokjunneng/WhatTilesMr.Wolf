using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTrigger : MonoBehaviour {
    private StepTriggerParent parent;
    private Color red = Color.red;// new Color(1.0f, 0.0f, 68f / 255f);
    private Color blue = new Color(41f / 255f, 181f / 255f, 193f / 255f);

    private float v = 255f;

    // Use this for initialization
    void Start () {
        parent = transform.parent.GetComponent<StepTriggerParent>();

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //gameObject.GetComponentInParent<StepTriggerParent>().UpdateTiles(c);

        //Debug.Log("Triggered on: " + gameObject.GetComponent<Renderer>().material.color);


        if (other.gameObject.tag == "Player Red" && gameObject.GetComponent<Renderer>().material.color != red)
        {
            gameObject.GetComponent<Renderer>().material.color = red;
            parent.increaseRed(1);
        }
            

        else if (other.gameObject.tag == "Player Blue" && gameObject.GetComponent<Renderer>().material.color == red)
        {
            gameObject.GetComponent<Renderer>().material.color = blue;
            //parent.blueTile++;

        }
            
    }


}
