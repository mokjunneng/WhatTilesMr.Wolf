using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverManager : NetworkBehaviour {

    private Animator anim;
    [SyncVar]
    private float countdownTimer = 20f;
    public Text stringTimer;
    [SyncVar]
    private bool startTimer = false;
    [SyncVar]
    private bool restart = false;

    [SyncVar]
    private float restartDelay = 5f;
    [SyncVar]
    private float restartTimer;

	// Use this for initialization
	void Awake () {
        
        anim = GetComponent<Animator>();
	}

    public override void OnStartServer()
    {
        startTimer = true;
    }

    // Update is called once per frame
    void Update () {
		if (startTimer)
        {
            countdownTimer -= Time.deltaTime;
            stringTimer.text = countdownTimer.ToString("f2");
            if (countdownTimer <= 0)
            {
                anim.SetTrigger("GameOver");
                startTimer = false;
                restart = true;
            }
        }

        if (restart)
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(0);
            }
        }
	}
}
