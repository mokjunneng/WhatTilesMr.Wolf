using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverManager : NetworkBehaviour {

    private Animator anim;

    private HexMap map;
    
    [SyncVar]
    private float countdownTimer = 10f;
    
    public Text stringTimer;
    public Text redCount;
    public Text blueCount;
    public Text result;

    [SyncVar]
    private bool startTimer = false;
    [SyncVar]
    private bool restart = false;

    [SyncVar]
    private float restartDelay = 10f;
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
            stringTimer.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(countdownTimer/60), countdownTimer % 60);
                
            if (countdownTimer <= 0)
            {
                anim.SetTrigger("GameOver");
                
                if (isServer)
                {
                    map = GameObject.FindGameObjectWithTag("TileMap").GetComponent<HexMap>();
                    int redTileCount = map.redTiles.Count;
                    int blueTileCount = map.blueTiles.Count;
                    
                    RpcSetResult(redTileCount, blueTileCount);
                }
                
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

    [ClientRpc]
    private void RpcSetResult(int redTileCount, int blueTileCount)
    {
        StartCoroutine(showResults(redTileCount, blueTileCount));
    }

    private IEnumerator showResults(int redTileCount, int blueTileCount)
    {
        yield return new WaitForSeconds(1);
        Debug.Log("setting text");
        redCount.text = "Red: " + redTileCount;
        blueCount.text = "Blue: " + blueTileCount;
        if (redTileCount > blueTileCount)
        {
            result.text = "Player 1 Wins!";
        }
        else if (redTileCount < blueTileCount)
        {
            result.text = "Player 2 Wins!";
        }
        else
        {
            result.text = "Game Draws";
        }

    }
}
