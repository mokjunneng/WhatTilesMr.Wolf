using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WolfSpawner : NetworkBehaviour {

    public GameObject wolfPrefab;

    // Use this for initialization

    public override void OnStartServer() {

        Debug.Log("spawn wolf");
        GameObject wolf = (GameObject)Instantiate(wolfPrefab, new Vector3(18.37f, 1.265f, 7.78f), Quaternion.Euler(0.0f,180f,0.0f));
        NetworkServer.Spawn(wolf);

    }
}
