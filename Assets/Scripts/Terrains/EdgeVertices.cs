using UnityEngine;

public struct EdgeVertices {
    public Vector3 v1, v2,v3, v4, v5;

    public EdgeVertices(Vector3 corner1, Vector3 corner2) {
        v1 = corner1;
        v2 = Vector3.Lerp(corner1, corner2, 1/3f);
        v3 = Vector3.Lerp(corner1, corner2, 0.5f);
        v4 = Vector3.Lerp(corner1, corner2, 2/3f);
        v5 = corner2;
    }
    public static EdgeVertices operator+(EdgeVertices origin ,Vector3 vector) {
        origin.v1 += vector;
        origin.v2 += vector;
        origin.v3 += vector;
        origin.v4 += vector;
        origin.v5 += vector;
        return origin;
    }
}