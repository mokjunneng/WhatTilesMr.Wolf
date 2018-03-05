using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEyeScript : MonoBehaviour {
    private float countTimer;

    //rotation-related variables  
    public float rotationAmount = 180;
    //booleans control
    private bool facingPlayers;
    private bool handlingPenalty = false;

    private Renderer[] childRenderers;
    private StepTriggerParent stepTriggerParent;

    private GameObject player;

    private Color red = Color.red;// new Color(1.0f, 0.0f, 68f / 255f);
    private Color blue = new Color(41f / 255f, 181f / 255f, 193f / 255f);


    // Use this for initialization
    void Start () {
        countTimer = Random.Range(3f, 6f);
        facingPlayers = false;
        player = GameObject.FindGameObjectWithTag("Player Red");

        childRenderers = GameObject.FindGameObjectWithTag("Tile Master").GetComponentsInChildren<Renderer>();
        stepTriggerParent = GameObject.FindGameObjectWithTag("Tile Master").GetComponent<StepTriggerParent>();
    }
	
	// Update is called once per frame
	void Update () {
        countTimer -= Time.deltaTime;

        //rotate the wolf when the randomized timer is up
        if (countTimer <= 0f)
        {
            StartCoroutine(rotate(rotationAmount));

            //to ignore the countdown timer
            countTimer = Mathf.Infinity;
        }

        //if player moving, give penalty
        if (player.GetComponent<CharacterControlScript>().isMoving && facingPlayers)
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
        //Debug.Log("giving penalty");
        Renderer lostTile;
       

        //Player playerData = player.GetComponent<Player>();
        //List<GameObject> tiles = playerData.tiles;

        
        while (player.GetComponent<CharacterControlScript>().isMoving && facingPlayers)
        {

            int tile = Mathf.RoundToInt(Random.Range(0f, childRenderers.Length - 2));
            Renderer r = childRenderers[tile];
     

            if (r.material.color == red)
            {
                r.material.color = blue;
                stepTriggerParent.increaseRed(-1);

            }

            yield return new WaitForSeconds(0.1f);
      
        }

        yield return new WaitForSeconds(1.5f);
        handlingPenalty = false;
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
