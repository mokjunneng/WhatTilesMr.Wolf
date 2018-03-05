using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour {    

    private Vector3 dir = Vector3.up;
    private float timer,t;
    private float rotationSpeed;
    private float distance = 50f;
    private float eyeInterval = 2f;

    private Renderer[] childRenderers;
    private StepTriggerParent stepTriggerParent;

    private Color red = Color.red;// new Color(1.0f, 0.0f, 68f / 255f);
    private Color blue = new Color(41f / 255f, 181f / 255f, 193f / 255f);

    // Use this for initialization
    void Start () {
        //dir = new Vector3();
        childRenderers = GameObject.FindGameObjectWithTag("Tile Master").GetComponentsInChildren<Renderer>();
        timer = Random.Range(1.0f, 3.0f);
        t = timer;

        stepTriggerParent = GameObject.FindGameObjectWithTag("Tile Master").GetComponent<StepTriggerParent>();
    }
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        transform.Rotate(dir * Time.deltaTime * rotationSpeed);

        if (Mathf.RoundToInt(timer) > 0f)
        {
            rotationSpeed = 200f;
        }
        

        else if (Mathf.RoundToInt(timer) <= 0 && Mathf.RoundToInt(timer) > -2f)
        {
            Debug.DrawRay(transform.position, transform.forward * distance, Color.red, 0.5f);
            

            rotationSpeed = 0;

            RaycastHit hit;
            Ray eyeRay = new Ray(transform.position, transform.forward);

            Quaternion spreadAngle = Quaternion.AngleAxis(-15, new Vector3(0, 1, 0));
            Vector3 newVector = spreadAngle * transform.forward;

            Ray eyeRay2 = new Ray(transform.position, newVector);
            Debug.DrawRay(transform.position, newVector * distance, Color.red, 0.5f);



            /*
             * if (hit player && isMoving) {
             *   for (all tiles == 'tag') {
             *      some number of tile.color == 'tag' -> other color
             */

            if (Physics.Raycast(eyeRay, out hit, distance))
            {
                if (hit.collider.gameObject.GetComponent<CharacterControlScript>().isMoving)
                {
                    Debug.Log(hit.collider.tag + " spoted moving at " + timer);
                    foreach (Renderer r in childRenderers)
                    {
             
                        if (hit.collider.tag == "Player Red" && r.material.color == red)
                        {
                            r.material.color = blue;
                            stepTriggerParent.increaseRed(-1);

                    
                        }

                        else if (hit.collider.tag == "Player Blue" && r.material.color == blue)
                        {
                            r.material.color = red;
                        }
                    }

                }

            }



        }
        else if (Mathf.RoundToInt(timer) <= -2f)
        {
            timer = Random.Range(1.0f, 5.0f);
            if (dir == Vector3.down) dir = Vector3.up;
            else dir = Vector3.down;
        }

    }
}
