using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepTriggerParent : MonoBehaviour
{

    private Component[] triggers;
    //private Slider slider;

    protected float redTile = 0;

    // Use this for initialization
    void Start()
    {
        triggers = GetComponentsInChildren<StepTrigger>();
        //slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //slider.value = redTile;

    }

    public void increaseRed(int sign)
    {
        float k = 0f;
        if (sign < 0) redTile --;
        else redTile ++;
    }

    void UpdateTiles(Collider other)
    {
        //foreach (Collider childCollider in colliders)
        //    OnTriggerEnter(childCollider);

        //Debug.Log("Triggered on: " + other.gameObject.tag);


        //if (other.gameObject.tag == "Player Red")
        //    gameObject.GetComponent<Renderer>().material.color = Color.red;

        //else if (other.gameObject.tag == "Player Blue")
        //    gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

}
