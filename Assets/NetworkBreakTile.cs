using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBreakTile : NetworkBehaviour
{
    private float timer;
    private float timerThreshold = 1.2f;

    public GameObject effect;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            print("enters trap tile");


        }
    }

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        print(timer);

        //destroy object
        if (other.gameObject.tag == "Player" && timer > timerThreshold)
        {

            print("trigger animation && player dies");
            if (effect)
            {
                //to add animation
                Instantiate(effect, transform.position, transform.rotation);
            }
            DestroyObject(this.gameObject);

            timer = 0f;
            other.gameObject.GetComponent<Combat>().health = 0;
            other.gameObject.GetComponent<Combat>().TakeDamage(0);




        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("exit trap");
            timer = 0f;
        }
    }
}
