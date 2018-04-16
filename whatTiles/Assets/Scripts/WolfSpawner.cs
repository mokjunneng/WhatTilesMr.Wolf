using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WolfSpawner : NetworkBehaviour {

    public GameObject wolfPrefab;

    public override void OnStartServer() {

        Debug.Log("spawn wolf");
        GameObject wolf = (GameObject)Instantiate(wolfPrefab, new Vector3(12.35f, 2.9f, 11.9f), Quaternion.Euler(0.0f, 90f, 0.0f));

        NetworkServer.Spawn(wolf);

    }
}
