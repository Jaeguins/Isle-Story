using UnityEngine;

public class TriMetrics {
    public const float root = 1.732050807568877f;
    public const float outerRadius = 2f*root*innerRadius/3f;
    public const float innerRadius = 10f;
    public static Vector3[] corners = {
        new Vector3(0f,0f,outerRadius),
        new Vector3(innerRadius,0f,-0.5f*outerRadius),
        new Vector3(-innerRadius,0f,-0.5f*outerRadius),
    };
    public static Vector3[] inverseCorners = {
        new Vector3(0f,0f,-outerRadius),
        new Vector3(innerRadius,0f,0.5f*outerRadius),
        new Vector3(-innerRadius,0f,0.5f*outerRadius),
    };
}
