using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Player : MonoBehaviour
{
    private float movementSpeed = 3f;

    private float tileSize = 1.732f;

    private enum Orientation { Horizontal, Vertical };

    private Orientation gridOrientation = Orientation.Horizontal;

    private bool moving = false;

    private bool allowDiagonals = false;

    private Vector2 movementInput;

    private Vector3 startPosition;

    private Vector3 endPosition;

    private float t;

    private float factor;

    private int tileCol = 0;
    private int tileRow = 0;

    public List<HexMap.Node> currentPath = null;

    public ControlScript controlScript;

    //Store tiles that belongs to player
    public List<GameObject> tiles;
    private Vector3 destinationPos;
    private float destinationDist;
    private Transform myTransform;

    private float moveSpeed;
    public bool isMoving;

    private Transform playerTransform = null;


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        myTransform = playerTransform;
        destinationPos = myTransform.position;
    }

    public int getTileCol()
    {
        return this.tileCol;
    }

    public void setTileCol(int col)
    {
        this.tileCol = col;
    }

    public int getTileRow()
    {
        return this.tileRow;
    }

    public void setTileRow(int row)
    {
        this.tileRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        destinationDist = Vector3.Distance(destinationPos, myTransform.position);

        if (destinationDist < .5f)  //prevent shaking behvaior when approaching destination
        {
            moveSpeed = 0;
            isMoving = false;
        }
        else
        {
            moveSpeed = 3;
            isMoving = true;
        }

        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPoint = ray.GetPoint(hit.distance);
                destinationPos = ray.GetPoint(hit.distance);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                myTransform.rotation = targetRotation;
            }
        }

        if (destinationDist > .5f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPos, moveSpeed * Time.deltaTime);
        }
        controlScript.updatePosition(myTransform.name);
    }


    private IEnumerator move(Transform transform)
    {
        moving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            if (movementInput.x != 0)
            {
                endPosition = new Vector3(
                startPosition.x + System.Math.Sign(movementInput.x) * tileSize,
                startPosition.y,
                startPosition.z);
            }
            if (movementInput.y != 0)
            {
                endPosition = new Vector3(
                startPosition.x + (System.Math.Sign(movementInput.x) * tileSize / 2.0f),
                startPosition.y,
                startPosition.z + System.Math.Sign(movementInput.y) * tileSize);
            }

        }
        else
        {
            endPosition = new Vector3(
                startPosition.x + System.Math.Sign(movementInput.x) * tileSize,
                startPosition.y + System.Math.Sign(movementInput.y) * 2f,
                startPosition.z);
        }

        factor = 1f;

        while (t < 1f)
        {
            t += Time.deltaTime * (movementSpeed / tileSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }


        moving = false;
        yield return 0;


    }

    public void clickMove(Vector3 finalPosition)
    {
        startPosition = transform.position;
        transform.Translate(endPosition * Time.deltaTime);

    }


}
