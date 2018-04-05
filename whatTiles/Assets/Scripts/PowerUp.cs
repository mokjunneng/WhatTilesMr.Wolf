using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject effect;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print("get power up");
        if (other.gameObject.tag == "Player")
        {
            print("trigger fade animation && do something");
            if (effect)
            {
                //to add animation
                Instantiate(effect, transform.position, transform.rotation);
            }

            other.GetComponent<playerMovement>().setHighSpeed();
            DestroyObject(this.gameObject);


        }
    }
}
