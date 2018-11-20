using UnityEngine;
[System.Serializable]
public struct TriCoordinates {
    [SerializeField]
    private int x, z;
    public int X {
        get {
            return x;
        }
    }
    public int Z {
        get {
            return z;
        }
    }

    public TriCoordinates(int x, int z) {
        this.x = x;
        this.z = z;
    }
    public static TriCoordinates FromOffsetCoordinates(int x, int z) {
        return new TriCoordinates(x, z);
    }
    public override string ToString() {
        return "(" + X.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines() {
        return X.ToString() + "\n" + Z.ToString();
    }
    public static TriCoordinates FromPosition (Vector3 position) {
        int iZ=Mathf.FloorToInt((position.z+TriMetrics.outerRadius)/(1.5f*TriMetrics.outerRadius));
        int iX = Mathf.FloorToInt((position.x+0.5f*TriMetrics.innerRadius) / (TriMetrics.innerRadius));
        return new TriCoordinates(iX,iZ);
    }
}