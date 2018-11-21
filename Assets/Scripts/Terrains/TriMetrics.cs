using UnityEngine;

public class TriMetrics {
    public const float root = 1.732050807568877f;
    public const float outerRadius = 2f * root * innerRadius / 3f;
    public const float innerRadius = 10f;

    public const float solidFactor = 0.3f;

    public const float blendFactor = 1f - solidFactor;

    static Vector3[] corners = {
        new Vector3(0f,0f,outerRadius),
        new Vector3(innerRadius,0f,-0.5f*outerRadius),
        new Vector3(-innerRadius,0f,-0.5f*outerRadius),
        new Vector3(0f,0f,outerRadius)
    };
    public static Vector3 GetFirstCorner(TriDirection direction, bool inverted) {
        return corners[((int)direction+1)];
    }

    public static Vector3 GetSecondCorner(TriDirection direction, bool inverted) {
        return corners[((int) direction+2)%3];
    }

    public static Vector3 GetFirstSolidCorner(TriDirection direction, bool inverted) {
        return GetFirstCorner(direction, inverted) * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(TriDirection direction, bool inverted) {
        return GetSecondCorner(direction, inverted) * solidFactor;
    }
}
