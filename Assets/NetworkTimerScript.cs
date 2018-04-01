using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkTimerScript : NetworkBehaviour
{

    //[SyncVar]
    //public float countDown = 30f;

    private Text timerText;

    public bool isNetworkTimeSynced = false;

    // timestamp received from server
    private int networkTimestamp;

    // server to client delay
    private int networkTimestampDelayMS;

    // when did we receive timestamp from server
    private float timeReceived;

    protected virtual void Start()
    {
        //timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();

        if (isLocalPlayer)
        {
            CmdRequestTime();
        }
    }

    void Update()
    {
        if (!isServer)
            return;

        //timerText.text = GetServerTime().ToString();

        //countDown -= Time.deltaTime;
    }


    [Command]
    private void CmdRequestTime()
    {
        int timestamp = NetworkTransport.GetNetworkTimestamp();
        RpcNetworkTimestamp(timestamp);
    }

    [ClientRpc]
    private void RpcNetworkTimestamp(int timestamp)
    {
        isNetworkTimeSynced = true;
        networkTimestamp = timestamp;
        timeReceived = Time.time;

        // if client is a host, assume that there is 0 delay
        if (isServer)
        {
            networkTimestampDelayMS = 0;
        }
        else
        {
            byte error;
            networkTimestampDelayMS = NetworkTransport.GetRemoteDelayTimeMS(
                NetworkManager.singleton.client.connection.hostId,
                NetworkManager.singleton.client.connection.connectionId,
                timestamp,
                out error);
        }
    }

    public float GetServerTime()
    {
        return networkTimestamp + (networkTimestampDelayMS / 1000f) + (Time.time - timeReceived);
    }
}