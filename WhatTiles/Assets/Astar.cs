using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar {
    Player player;
    int col, row;

    Queue<HexMap.Node> path;

    public Astar (Player player, int col, int row)
    {
        this.player = player;
        this.col = col;
        this.row = row;
    }

    public void computePath()
    {
        path = new Queue<HexMap.Node>();
    }
}
