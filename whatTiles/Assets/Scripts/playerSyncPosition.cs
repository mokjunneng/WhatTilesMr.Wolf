using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//script to sync players' movements over the network smoothly
public class playerSyncPosition : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;
    [SyncVar]
    private Quaternion syncRotation;

    [SerializeField] Transform myTransform;
    [SerializeField] float lerpRate = 15;
    [SerializeField] float SlerpRate = 10;

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, syncRotation, SlerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer (Vector3 pos, Quaternion rotation)
    {
        syncPos = pos;
        syncRotation = rotation;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(myTransform.position, myTransform.rotation);

        }
    }
}
