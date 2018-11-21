using UnityEngine;

public class TriMetrics {
    public const float root = 1.732050807568877f;
    public const float outerRadius = 2f * root * innerRadius / 3f;
    public const float innerRadius = 10f;

    public const float solidFactor = 0.8f;
    public const float blendFactor = 1f - solidFactor;

    public const float elevationStep = 5f;

    static Vector3[] corners = {
        new Vector3(0f,0f,outerRadius),
        new Vector3(innerRadius,0f,-0.5f*outerRadius),
        new Vector3(-innerRadius,0f,-0.5f*outerRadius),
        new Vector3(0f,0f,outerRadius)
    };
    public static Vector3 GetFirstCorner(TriDirection direction) {
        return corners[((int)direction+1)];
    }

    public static Vector3 GetSecondCorner(TriDirection direction) {
        return corners[((int) direction+2)%3];
    }

    public static Vector3 GetFirstSolidCorner(TriDirection direction) {
        return GetFirstCorner(direction) * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(TriDirection direction) {
        return GetSecondCorner(direction) * solidFactor;
    }
    public static Vector3 GetBridge(TriDirection direction) {
        return (GetFirstCorner(direction) + GetSecondCorner(direction)) *0.5f * blendFactor;
    }
}
