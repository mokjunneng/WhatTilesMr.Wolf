using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class hexSpawner : NetworkBehaviour {

    public GameObject mapPreFab;

    public override void OnStartServer()
    {
        GameObject map = (GameObject)Instantiate(mapPreFab, new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0, 0));
        NetworkServer.Spawn(map);
    }

}
