using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour {

    public GameObject effect;

    public AudioClip powerupSE;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GameObject.Find("Directional Light").GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print("get power up");
        if (other.gameObject.tag == "Player")
        {
            //SE
            audioSource.PlayOneShot(powerupSE);

            print("trigger fade animation && do something");
            if (effect)
            {
                //to add animation
                Instantiate(effect, transform.position, transform.rotation);
            }

            //display aura
            //other.GetComponent<ParticleSystem>().Play();

            other.GetComponent<playerMovement>().setHighSpeed();
            DestroyObject(this.gameObject);


        }
    }
}
