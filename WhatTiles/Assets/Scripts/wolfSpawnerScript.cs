using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class wolfSpawnerScript : NetworkBehaviour
{
    public GameObject wolfPrefab;

    // Use this for initialization
    public override void OnStartServer()
    {
        GameObject wolf = (GameObject)Instantiate(wolfPrefab, new Vector3(18.37f, 1.265f, 7.78f), Quaternion.Euler(0.0f, 180f, 0.0f));
        print("wolf sent");
        NetworkServer.Spawn(wolf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
