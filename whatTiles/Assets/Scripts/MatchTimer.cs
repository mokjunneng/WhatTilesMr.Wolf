using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MatchTimer : NetworkBehaviour {

    public string sceneToLoad;
    [SyncVar]
    private float timer = 60f;
    private Text timerSeconds;
    [SyncVar]
    private bool startTimer = false;

	void Start () {
        timerSeconds = GetComponent<Text>();
	}

    public override void OnStartServer()
    {
        startTimer = true;
    }

    void Update () {
        if (startTimer)
        {
            timer -= Time.deltaTime;
            timerSeconds.text = timer.ToString("f2");
            if (timer <= 0)
            {
                SceneManager.LoadScene(1);
            }
        }
       
	}
    
}
