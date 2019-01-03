using UnityEngine;
using System.IO;
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
    public int DistanceTo(TriCoordinates other) {
        return 2*((x<other.x)?(other.x-x):(x-other.x))+
               ((z<other.z)?(other.z-z):(z-other.z));
    }
    public void Save(BinaryWriter writer) {
        writer.Write(x);
        writer.Write(z);
    }
    public static TriCoordinates Load(BinaryReader reader) {
        TriCoordinates c;
        c.x = reader.ReadInt32();
        c.z = reader.ReadInt32();
        return c;
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
    public static TriCoordinates operator +(TriCoordinates origin, Vector2Int adder) {
        origin.x += adder.x;
        origin.z += adder.y;
        return origin;
    }

    public static bool operator==(TriCoordinates A,TriCoordinates B) {
        if (A.X == B.X && A.Z == B.Z) return true;
        else return false;
    }
    public static bool operator !=(TriCoordinates A, TriCoordinates B) {
        return !(A == B);
    }
    public override bool Equals(object t) {
        return base.Equals(t);
    }
    public override int GetHashCode() {
        return base.GetHashCode();
    }
}