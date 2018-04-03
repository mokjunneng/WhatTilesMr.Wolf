using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UISpawner : NetworkBehaviour
{
    public GameObject UIPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnStartServer()

    {
        {

            GameObject ui = (GameObject)Instantiate(UIPrefab, new Vector3(0, 200, 0), new Quaternion());
            print("...");
            NetworkServer.Spawn(ui);
        }
        base.OnStartClient();
    }

}
