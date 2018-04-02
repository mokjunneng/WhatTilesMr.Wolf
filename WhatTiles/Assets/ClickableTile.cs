using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {
    
    //public HexMap map;
    public Vector3 tilePos;
    public int tileCol;
    public int tileRow;
    //public Player player;
    private Hex h;

    

    private void Start()
    {
        //h = new Hex(tileCol, tileRow);
    }
    private void OnMouseUp()
    {
        Debug.Log("Clicked Tile: " + tileCol + ","  + tileRow);
        //map.GenerateShortestPathTo(tilePos, tileCol, tileRow);
       
        //player.clickMove(h.Position());
    }

    
}
