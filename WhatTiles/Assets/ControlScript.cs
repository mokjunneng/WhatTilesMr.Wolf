﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

public class ControlScript : MonoBehaviour, RealTimeMultiplayerListener
{
    public Transform prefab = null;
    public bool authenticate = false;
    public Transform player = null;
    
    float timer = 0f;

    void Update()
    {
        if (!authenticate)
        {
            //player = Instantiate(prefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            Authenticate();
        }

        else
        {
            if (player != null)
            {

                timer = 0.035f;
                bool reliability = true;
                string data = "Position:" + player.position.x + ":" + player.position.y + ":" + player.position.z;
                byte[] bytedata = System.Text.ASCIIEncoding.Default.GetBytes(data);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, bytedata);

            }
        }
    }

    public void updatePosition(string playerName)
    {
        //Todo: updatePosition

        //bool reliability = true;
        //string data = "Position:" + player.position.x + ":" + player.position.y + ":" + player.position.z;
        //byte[] bytedata = System.Text.ASCIIEncoding.Default.GetBytes(data);
        //PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, bytedata);
    }

    void Authenticate()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Authentication succeeded");
                CreateQuickGame();
            }

            else
            {
                Debug.Log("Authentication failed");
            }
        });
    }

    void CreateQuickGame()
    {
        const int MinOpponents = 1, MaxOpponents = 1;
        const int GameVariant = 0;
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
            GameVariant, this);
    }

    #region RealTimeMultiplayerListener implementation

    private bool isRoomSetuped = false;
    public void OnRoomSetupProgress(float percent)
    {
        if (percent >= 20f)
        {
            isRoomSetuped = true;
            Debug.Log("Currently finding a macth...");
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }
    }

    private bool connected = false;
    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            //Instantiate your player on the network.
            connected = true;

            player = Instantiate(prefab, new Vector3(0f, 4f, 0f), Quaternion.identity);
            player.name = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;

            bool reliability = true;
            string data = "Instantiate:0:1:2";
            byte[] bytedata = System.Text.ASCIIEncoding.Default.GetBytes(data);
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, bytedata);
        }

        else
        {
            connected = false;
            CreateQuickGame();
        }
    }

    public void OnLeftRoom()
    {
        connected = false;
        authenticate = false;
    }

    public void OnParticipantLeft(Participant participant)
    {
        connected = false;
        authenticate = false;
    }

    public void OnPeersConnected(string[] participantIds)
    {

    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        connected = false;
        authenticate = false;
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        if (!PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId.Equals(senderId))
        {
            string rawdata = System.Text.ASCIIEncoding.Default.GetString(data);
            string[] sliced = rawdata.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries);

            if (sliced[0].Contains("Instantiate"))
            {
                Transform naming = Instantiate(prefab, new Vector3(0f, 4f, 0f), Quaternion.identity);
                naming.name = senderId;
                naming.GetChild(0).gameObject.SetActive(false);
            }

            else if (sliced[0].Contains("Position"))
            {
                Transform target = GameObject.Find(senderId).transform;

                if (target == null)
                {
                    return;
                }

                Vector3 newpos = new Vector3
                (
                    System.Convert.ToSingle(sliced[1]),
                    System.Convert.ToSingle(sliced[2]),
                    System.Convert.ToSingle(sliced[3])
                );

                target.position = newpos;
            }
        }
    }

    #endregion
}