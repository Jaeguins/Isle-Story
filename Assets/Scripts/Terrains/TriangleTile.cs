using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TriangleTile {
    public const float radSize = 10f;//높이
    public const float sideSize = radSize * 1.73205080756887f;//변의 길이의 반
    public static Vector3[] corners = {
        new Vector3(0,0,2*radSize),
        new Vector3(-sideSize,0,-radSize),
        new Vector3(sideSize,0,-radSize)
    };
    
    public float height = 1f;
}

