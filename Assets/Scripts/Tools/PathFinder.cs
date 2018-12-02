using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class PathFinder{
    static float cellHeight = TriMetrics.outerRadius / 2f;
    static float cellWidth = TriMetrics.innerRadius;
    static Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
    TriGrid grid;
    public static Vector2Int getCellCoordByOffset(float x,float z) {
        Vector2Int ret = new Vector2Int((int)(x/cellWidth), (int)(z / cellWidth));
        return ret;
    }
    static bool possibilityCheck(Vector2Int coord) {
        return true;
    }
    public static Stack<Vector2Int> FindPath(Vector2Int start,Vector2Int target) {
        Dictionary<Vector2Int, PathNode> nodes = DictPool<Vector2Int, PathNode>.Get();
        int count = 0;
        nodes.Add(start, new PathNode(start, 0, (target-start).sqrMagnitude));
        while (true) {
            if (count++ > 100000) return new Stack<Vector2Int>();
            KeyValuePair<Vector2Int, PathNode>[] arrField = nodes.ToArray<KeyValuePair<Vector2Int, PathNode>>();
            PathNode tNode= arrField[0].Value;
            Vector2Int tKey= arrField[0].Key;
            int minimumF = int.MaxValue;
            for (int i = 0; i < nodes.Count; i++) {
                if (!arrField[i].Value.isClosed && arrField[i].Value.GetF() < minimumF) {
                    tNode = arrField[i].Value;
                    tKey= arrField[i].Key;
                    minimumF = arrField[i].Value.GetF();
                }
            }
            if (tKey == target) break;
            tNode.isClosed = true;

            foreach (Vector2Int dir in directions) {
                if (!possibilityCheck(tKey+dir)) continue;
                int accG = 10;
                accG += tNode.g;
                Vector2Int tLoc = tKey + dir;
                if (nodes.ContainsKey(tLoc)) {
                    if (nodes[tLoc].g > accG) {
                        nodes[tLoc] = new PathNode(tKey, accG, nodes[tLoc].h);
                    }
                }
                else {
                    nodes.Add(tLoc, new PathNode(tKey, accG, 10 * (int)(target.x + target.y - tLoc.x - tLoc.y)));
                }
            }
        }
        Stack<Vector2Int> stack = StackPool<Vector2Int>.Get();
        PathNode pointer = nodes[target];
        while (pointer.parent != start) {
            stack.Push(pointer.parent);
            pointer = nodes[pointer.parent];
        }
        DictPool<Vector2Int, PathNode>.Add(nodes);
        return stack;
    }
}
public struct PathNode {
    public Vector2Int parent;
    public bool isClosed;
    public int g, h;
    public PathNode(Vector2Int parent,int g,int h) {
        isClosed = false;
        this.parent = parent;
        this.g = g;
        this.h = h;
    }
    public int GetF() {
        return g + h;
    }
    public void setParent(Vector2Int parent) {
        this.parent = parent;
    }
}