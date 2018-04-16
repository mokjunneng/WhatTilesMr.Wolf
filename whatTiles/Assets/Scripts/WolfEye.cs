
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WolfEye :NetworkBehaviour {

    private float countTimer;
    public float countTimerCopy; // Copy of countTimer for Testing
    //rotation-related variables  
    public float rotationAmount = 180;

    //vibration-related variables
    public float shakeIntensity = 0.3f;

    //SE
    AudioSource audioSource;

    //booleans control
    public bool handlingPenalty = false;
    [SyncVar]
    public bool facingPlayers = false; 
    private bool ordered = false;
    //private bool vibrating = true;

    //store the renderers of tiles generated in game
    private List<GameObject> tiles;
    private List<GameObject> tilesGettingPenalty;
    //private Renderer[] tilesRenderers;

    private GameObject player;
    private GameObject map;
    private Color red = new Color(1F, 0.1911765F, 0.1911765F);
    private Color blue = new Color(0.3317474F, 0.6237204F, 0.8676471F);

    private bool init = true;
    public GameObject[] players;
    List<GameObject> playerOrderedList = new List<GameObject>();

    private uint clientId;

    public SyncListUInt playerList = new SyncListUInt();
    private int index = 0;
    public GameObject[] items;


    void Start()
    {
        countTimer = Random.Range(3f, 6f);
        playerList.Callback = SyncListUIntChanged;
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnStartClient()
    {
        Debug.Log("Wolf says hi");
        Debug.Log("facingPlayer bool: " + facingPlayers);
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (init && GameObject.FindGameObjectWithTag("Player") != null)
        {
            init = false;
            map = GameObject.FindGameObjectWithTag("TileMap");
        }
        else if (!init && GameObject.FindGameObjectWithTag("Player") != null)
        {
            //get reference to players in game from server
            players = GameObject.FindGameObjectsWithTag("Player");

            if (GameObject.Find("Canvas").GetComponent<GameOverManager>().startTimer)
                countTimer -= Time.deltaTime;
             
            //get ready to vibrate alert 
            if (countTimer <= 0.6f && countTimer > 0f && facingPlayers == false)
            {
                Vector3 originpos = transform.position;
                StartCoroutine(vibrate(transform.position));
                transform.position = originpos;
            }

            //rotate wolf when timer is up 
            if (countTimer <= 0f)
            {
                StartCoroutine(rotate(rotationAmount));

                //to ignore countdown timer 
                countTimer = Mathf.Infinity;
            }

            //order players
            if (!ordered)
            {
                foreach (GameObject p in players)
                {
                    if (p.GetComponent<playerMovement>().playerIndex == 1)
                    {
                        playerOrderedList.Insert(0, p);
                    }
                    else playerOrderedList.Add(p);
                }

                ordered = true;
            }
        }
        
    }

    
    
    private IEnumerator rotate(float angle, float duration = 0.3f)
    {
        
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(Vector3.up * angle);

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        facingPlayers ^= true;
        //RpcSetFacingPlayerBool();

        //foreach (GameObject p in players)
        //    p.GetComponent<playerMovement>().wolfSpotting ^= true;

        transform.rotation = to;
        resetTimer();
        //Todo reset wolf
     
    }

    [ClientRpc]
    private void RpcSetFacingPlayerBool()
    {
        facingPlayers ^= true;
    }

    private IEnumerator vibrate(Vector3 originPost)
    {
        float elapsed = 0f;
        if (!audioSource.isPlaying)
            audioSource.Play();

        while (elapsed < 0.6f)
        {
            transform.position = originPost + Random.insideUnitSphere * shakeIntensity;
            elapsed += Time.deltaTime;
            yield return null;
        }
        //vibrating = false;
        audioSource.Stop();
    }

    void resetTimer()
    {
        countTimer = Random.Range(3f, 6f);
        countTimerCopy = countTimer;
    }

    // Functions for Testing
    public float getCountTimer()
    {
        return countTimerCopy;
    }

    public void startRotation()
    {
        StartCoroutine(rotate(rotationAmount));
    }

    public void SyncListUIntChanged(SyncListUInt.Operation op, int index)
    {
        Debug.Log("ADDING PLAYER " + (index) + " to Id " + playerList[index]);
        items = GameObject.FindGameObjectsWithTag("Player");
        items[index].GetComponent<playerMovement>().indexChanged(index);
    }
}

