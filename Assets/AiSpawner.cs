using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AiSpawner : NetworkBehaviour
{

    public GameObject aiPrefab;

    // Use this for initialization
    public override void OnStartServer() {

        GameObject wolf = (GameObject)Instantiate(aiPrefab, new Vector3(1.07f, 1.65f, 1.44f), Quaternion.Euler(0.0f, 180f, 0.0f));
        NetworkServer.Spawn(wolf);
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
