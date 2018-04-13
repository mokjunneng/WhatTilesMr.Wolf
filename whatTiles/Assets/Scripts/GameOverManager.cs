using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverManager : NetworkBehaviour
{

    private Animator anim;

    private HexMap map;

    [SyncVar]
    private float countdownTimer = 20f;

    public Text stringTimer;
    public Text redCount;
    public Text blueCount;
    public Text result;
    public Text loadingText;

    [SyncVar]
    private bool startTimer = false;
    [SyncVar]
    private bool restart = false;

    [SyncVar]
    private float restartDelay = 5f;
    [SyncVar]
    private float restartTimer;

    [SyncVar]
    private int playersConnected = 0;
    [SyncVar]
    private float preTimer = 3f;

    public GameObject loadingMask;

    public Image playerImg1;
    public Image playerImg2;

    // Use this for initialization
    void Awake()
    {

        anim = GetComponent<Animator>();
    }

    void Start()
    {
        playerImg1.enabled = false;
        playerImg2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!startTimer && !restart)
        {
            if (isClient && hasAuthority)
            {
                CmdGetPlayers();
            }

            if (playersConnected < 2)
            {
                loadingMask.SetActive(true);
                loadingText.text = "Gathering additional player... " + playersConnected + "/2";

                if (playersConnected == 1)
                {
                    playerImg1.enabled = true;
                }
            }
            else
            {
                preTimer -= Time.deltaTime;

                if (preTimer <= -2)
                {
                    loadingMask.SetActive(false);
                    loadingText.text = "";

                    startTimer = true;
                }

                else if (preTimer <= 0)
                {
                    loadingText.text = "Start!";

                    playerImg1.enabled = false;
                    playerImg2.enabled = false;
                }
                else
                {
                    loadingText.text = string.Format("Starting in {0} ...", Mathf.CeilToInt(preTimer));
                    playerImg2.enabled = true;
                    playerImg1.enabled = true;
                }
            }
        }

        else if (startTimer && !restart)
        {

            countdownTimer -= Time.deltaTime;
            stringTimer.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(countdownTimer / 60), countdownTimer % 60);

            if (countdownTimer <= 0)
            {
                anim.SetTrigger("GameOver");
                stringTimer.enabled = false;

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
                Debug.Log("Go to menu scene");
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

    [Command]
    private void CmdGetPlayers()
    {
        playersConnected = NetworkServer.connections.Count;
    }

    void OnDisable()
    {
        Debug.Log("GameOver: script was disabled");
    }

    void OnEnable()
    {
        Debug.Log("GameOver: script was enabled");
    }

}

