using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPrevent : MonoBehaviour
{

    // Attach this script as a component to the character
    private float Xorigin;
    private float Zorigin;
    private float Height;

    private GameObject player;
    private CharacterControlScript characterControlScript;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player Red");
        characterControlScript = GameObject.FindGameObjectWithTag("Player Red").GetComponent<CharacterControlScript>();

        // Setting Startpoint
        Xorigin = player.GetComponent<Transform>().position.x;
        Zorigin = player.GetComponent<Transform>().position.z;
        Height = player.GetComponent<Transform>().position.y;
        // Asking Height
        //if (Height < 0)
        //{
        //    print("FALLPREVENT: Height is bellow zero! Setting secure height.");
        //    Height += 3;
        //}
    }
    void Update()
    {
        // Character respawns to the starting point when falling.
        //if (transform.position.y < 0)
        //{
        //    transform.position = new Vector3(Xorigin, Height, Zorigin);
        //}

        if (characterControlScript.getDead())
        {
            characterControlScript.setDead(false);
            print("...");
            StartCoroutine(ResPawn());
        }

    }

    private IEnumerator ResPawn()
    {
        player.GetComponent<Transform>().position = new Vector3(Xorigin, Height, Zorigin);
        player.SetActive(false);
        yield return new WaitForSeconds(5f);
        print("revived!");         
        player.SetActive(true);
        
        yield return null;

    }
}

