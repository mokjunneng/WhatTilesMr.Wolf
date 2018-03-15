using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex{

    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    // Q + R + S = 0
    // S = -(Q + R)

    public readonly int Q;  //column
    public readonly int R;  //row
    public readonly int S;

    readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    // returns world-space position of this hex
    public Vector3 Position()
    {
        
        float radius = 1f;
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        //horizontal offset
        float horizontal = width;
        //vertical offset
        float vertical = height * 0.75f;

        float xPos = horizontal * this.Q;

        if (this.R % 2 == 1)
        {
            xPos += horizontal / 2f;
        }

        return new Vector3(xPos, 0, vertical * this.R);
    }


}
