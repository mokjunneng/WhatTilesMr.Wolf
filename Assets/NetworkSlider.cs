using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkSlider : NetworkBehaviour
{


    private Combat controller;


    public void SetPlayer(Combat controller)
    {
        this.controller = controller;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
            SetHealthAmount(controller.health);
    }

    void SetHealthAmount(float _amount)
    {
        GetComponent<Slider>().value = _amount;
    }


}
