using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : MonoBehaviour {

    private float timer;
    private float timerThreshold = 1.2f;
    private CharacterControlScript characterControlScript;
    // Use this for initialization
    void Start () {
        characterControlScript = GameObject.FindGameObjectWithTag("Player Red").GetComponent<CharacterControlScript>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player Red")
        {

            print("enters");


        }
    }

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        print(timer);

        if (other.gameObject.tag == "Player Red" && timer > timerThreshold)
        {

            print("trigger animation && player dies");
            timer = 0f;
            characterControlScript.setDead(true);
           

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player Red")
        {
            print("exit");
            timer = 0f;
        }
    }


}
