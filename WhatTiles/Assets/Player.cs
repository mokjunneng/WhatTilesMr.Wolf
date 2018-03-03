using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
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

    //Store tiles that belongs to player
    public List<GameObject> tiles;

	// Use this for initialization
	void Start () {
        
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
    void Update () {
        
        //if( currentPath != null)
        //{
        //    int currNode = 0;

        //    while( currNode < currentPath.Count - 1)
        //    {
        //        Vector3 start = currentPath[currNode].position;
        //        Vector3 end = currentPath[currNode + 1].position;

        //        Debug.DrawLine(start, end);

        //        currNode++;
        //    }
        //}

        //if (!moving)
        //{
        //    movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //    if (!allowDiagonals)
        //    {
        //        if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y))
        //        {
        //            movementInput.y = 0;
        //        }
        //        else
        //        {
        //            movementInput.x = 0;
        //        }
        //    }

        //    if (movementInput != Vector2.zero)
        //    {
        //        StartCoroutine(move(transform));
        //    }
        //}
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

        while ( t < 1f)
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

    //public void moveOneTile(int col, int row) 
    //{
        
    //    float distance = (Mathf.Abs(this.tileCol - col) + Mathf.Abs(this.tileCol + this.tileRow - col - row) + Mathf.Abs(this.tileRow - row)) / 2f;

    //    if (distance <= 2)
    //    {
    //        this.StartCoroutine(move2(this.transform, col, row));
    //    }
    //}

    //private IEnumerator move2(Transform transform, int col, int row)
    //{
    //    moving = true;
    //    startPosition = transform.position;
    //    t = 0;

    //    Hex h = new Hex(col, row);

    //    endPosition = h.Position();

    //    factor = 1f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime * (movementSpeed / Vector3.Distance(startPosition, endPosition)) * factor;
    //        transform.position = Vector3.Lerp(startPosition, endPosition, t);
    //        yield return null;
    //    }

    //    moving = false;
    //    yield return 0;
    //}

    
}
