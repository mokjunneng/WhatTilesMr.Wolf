using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isLocalPlayer = false;

    #region tile initialisation
    private int tileCol = 0;
    private int tileRow = 0;
    //Store tiles that belongs to player
    public List<GameObject> tiles;
    #endregion


    #region getter and setter
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
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        

    }
}
