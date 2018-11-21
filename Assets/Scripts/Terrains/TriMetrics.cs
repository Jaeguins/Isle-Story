using UnityEngine;

public class TriMetrics {
    public const float root = 1.732050807568877f;
    public const float outerRadius = 2f*root*innerRadius/3f;
    public const float innerRadius = 10f;
    static Vector3[] corners = {
        new Vector3(0f,0f,outerRadius),
        new Vector3(innerRadius,0f,-0.5f*outerRadius),
        new Vector3(-innerRadius,0f,-0.5f*outerRadius),
        new Vector3(0f,0f,outerRadius)
    };
    public static Vector3 GetFirstCorner(TriDirection direction,bool inverted) {
        if (direction == 0) return corners[1];
        if (inverted) {
            return corners[(int)direction + 1];
        }
        else {
            return corners[(int)direction < 2 ? 0 : 2];
        }
    }

    public static Vector3 GetSecondCorner(TriDirection direction,bool inverted) {
        if (direction == 0) return corners[2];
        if (inverted) {
            return corners[(int)direction - 1];
        }
        else {
            return corners[(int)direction < 2 ? 1 : 0];
        }
    }
}
