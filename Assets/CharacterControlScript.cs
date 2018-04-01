using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterControlScript : NetworkBehaviour
{
    private Transform myTransform;              // This transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition

    private float moveSpeed;
    private float moveSpeedLow = 1.5f;
    private float moveSpeedHigh = 2.5f;
    public bool isMoving;
    private bool highSpeed;
    private float highSpeedTimer;
    private float highSpeedThreshold = 5f;

    private Animator anim;
    private Slider hpSlider;
    private float hp = 5f;
    private float hpOffset = 0.8f;
    private bool isDead;

    private Ray ray;
    Plane playerPlane;

    // Use this for initialization
    void Start () {
        myTransform = transform;                            // Sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;         // Prevents myTransform reset

        anim = GetComponent<Animator>();
        hpSlider = GameObject.FindGameObjectWithTag("HpSlider").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
            return;

        //print(destinationDistance + "update");


        if (hp > 5)
        {
            hp = 5;
        }else if (hp <= 0)
        {
            hp = 0;
            print("dead");
            isDead = true;
        }
        else hpSlider.value = hp;

        if (highSpeed)
        {
         
            highSpeedTimer += Time.deltaTime;
            if (highSpeedTimer > highSpeedThreshold)
            {
                highSpeed = false;
                highSpeedTimer = 0f;
            }
        }

        if (!isMoving)
        {
            hp -= Time.deltaTime * hpOffset;
        }
        else
        {
            hp += Time.deltaTime;
        }

        if (isDead)
        {
            isMoving = false;
            moveSpeed = 0f;
        }

        //Touch touch = Input.GetTouch(0);

        // Keep track of the distance between this gameObject and destinationPosition
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < .5f)
        {       
            // Prevent shakin behavior when near destination
            moveSpeed = 0f;
            isMoving = false;
        }
        else if (destinationDistance > .5f)
        {
            // Reset speed to default
            if (highSpeed)
            {
                moveSpeed = moveSpeedHigh;
            }
            else
            {
                moveSpeed = moveSpeedLow;
            }
            
            
            isMoving = true;
            
        }

        anim.SetFloat("Speed", moveSpeed);

        // Moves player by click... can be used in mobile devices also
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            playerPlane = new Plane(Vector3.up, myTransform.position);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                //Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 1000 * Time.deltaTime);
                //myTransform.rotation = targetRotation;
            }
        }

        // Moves player by touch
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        //{

        //    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
        //    Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //    float hitdist = 0.0f;

        //    if (playerPlane.Raycast(ray, out hitdist))
        //    {
        //        Vector3 targetPoint = ray.GetPoint(hitdist);
        //        destinationPosition = ray.GetPoint(hitdist);
        //        //Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        //        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 1000 * Time.deltaTime);
        //        //myTransform.rotation = targetRotation;
        //    }
        //}

        // Prevent code from running if not needed
        if (destinationDistance > .5f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
            print(destinationDistance + "> .5f");
        }

    }

    public void setHighSpeed()
    {
        highSpeed = true;
    }

    public bool getDead()
    {
        return isDead;
    }

    public void setDead(bool status)
    {
        isDead = status;
        if (status)
        {
            hp = 0;


        }

        else
        {

            hp = 5f;

        }
    }

    void OnEnable()
    {
        // restarts script;
        Start();
    }

}
