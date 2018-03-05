using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpControlScript : MonoBehaviour {
    bool available = true;
    private float amt = 0.5f;

    // temp public var for dubugging
    public GameObject player;
    private CharacterControlScript CharacterControlScript;

    // Use this for initialization
    void Start () {
        CharacterControlScript = player.GetComponent<CharacterControlScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print("get power up");
        if (other.gameObject.tag == "Player Red" && available)
        {

            print("trigger fade animation && do something");
            CharacterControlScript.setHighSpeed();
            available = false;
  

        }
    }

    public void respawn()
    {
        available = true;
    }
}
