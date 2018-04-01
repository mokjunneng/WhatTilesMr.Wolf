using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestCubeScript: NetworkBehaviour
{

    [SyncVar]
    public bool facingPlayers;

    [SyncVar]
    private float countTimer;

    //booleans control
    [SyncVar]
    private bool handlingPenalty = false;

    //rotation-related variables
    [SyncVar]
    public float rotationAmount = 180;

    //store the renderers of tiles generated in game
    private List<GameObject> tiles;

    public GameObject tileMaster;

    // Use this for initialization
    void Start()
    {
        tiles = tileMaster.GetComponent<TileStorage>().tiles;

        countTimer = Random.Range(3f, 6f);
        facingPlayers = false;
    }

    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            return;

        player = GameObject.FindGameObjectWithTag("Player");

        countTimer -= Time.deltaTime;

        if (countTimer <= 0f)
        {
            StartCoroutine(rotate(rotationAmount));

            //to ignore the countdown timer
            countTimer = Mathf.Infinity;
        }

        //if player moving, give penalty
        if (player.GetComponent<PlayerMove>().isMoving && facingPlayers)
        {
            if (!handlingPenalty)
            {
                StartCoroutine(givePenaltyToMovingPlayer());
            }
        }
    }

    private IEnumerator givePenaltyToMovingPlayer()
    {
        handlingPenalty = true;

        while (player.GetComponent<PlayerMove>().isMoving && facingPlayers)
        {

            //int tile = Mathf.RoundToInt(Random.Range(0f, childRenderers.Length - 2));

            GameObject tile = tiles[Random.Range(0, tiles.Count - 1)];


            Renderer r = tile.GetComponent<Renderer>();


            if (player.GetComponent<playerMovement>().id % 2 == 1)
            {
                if (r.material.color != Color.blue)
                {
                    RpcPenalty(tile, Color.blue);
                    //r.material.color = blue;
                    //stepTriggerParent.increaseRed(-1);
                }
            }
            else
            {
                if (r.material.color != Color.red)
                {
                    RpcPenalty(tile, Color.red);
                    //r.material.color = red;
                    //stepTriggerParent.increaseRed(-1);
                }
            }
            yield return new WaitForSeconds(0.1f);

        }

        print("moving " + player.name);
        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;

    }

    [ClientRpc]
    void RpcPenalty(GameObject tileObject, Color col)
    {
        tileObject.GetComponent<Renderer>().material.color = col;
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



        transform.rotation = to;
        resetTimer();

    }

    void resetTimer()
    {
        countTimer = Random.Range(3f, 6f);
    }
}
