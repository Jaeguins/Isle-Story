using UnityEngine;
using System.Collections.Generic;

public class TriMapGenerator : MonoBehaviour {
    int cellCount;
    public TriGrid grid;
    Queue<TriCell> searchFrontier;
    HashSet<TriCoordinates> checker;
    [Range(0f, 1f)]
    public float jitterProbability = 0.25f;
    [Range(20, 200)]
    public int chunkSizeMin = 30;
    [Range(20, 200)]
    public int chunkSizeMax = 100;
    [Range(5, 95)]
    public int landPercentage = 50;
    [Range(0, 10)]
    public int mapBorderX = 5;
    [Range(0, 10)]
    public int mapBorderZ = 5;

    int xMin, xMax, zMin, zMax;

    int searchFrontierPhase;
    public void GenerateMap(int x, int z) {
        cellCount = x * z;
        grid.CreateMap(x, z);
        if (searchFrontier == null) {
            searchFrontier = new Queue<TriCell>();
            checker = new HashSet<TriCoordinates>();
        }
        xMin = mapBorderX;
        xMax = x - mapBorderX;
        zMin = mapBorderZ;
        zMax = z - mapBorderZ;
        CreateLand();
        SetTerrainType();
    }

    void CreateLand() {
        int landBudget = Mathf.RoundToInt(cellCount * landPercentage * 0.01f);
        bool initiated = false;
        int overflowCatcher = 0;
        while (landBudget > 0) {
            overflowCatcher++;
            if (overflowCatcher > 10000) {
                Debug.LogError("drawcall overflowed");
                return;
            }
            landBudget = RaiseTerrain(
                Random.Range(chunkSizeMin, chunkSizeMax + 1), landBudget, initiated
            );
            initiated = true;
        }
    }

    int RaiseTerrain(int chunkSize, int budget, bool initiated) {
        searchFrontierPhase += 1;
        TriCoordinates center = grid.GetCell(grid.cellCountX / 2, grid.cellCountZ / 2).coordinates;
        center = TriMetrics.TriToHex(center);
        TriCell firstCell = grid.GetCell(center.X, center.Z);
        if (initiated) {
            do {
                firstCell = GetRandomCell();
            } while (firstCell.Elevation < 1);
        }
        else {
            Debug.Log(center);
        }
        searchFrontier.Enqueue(firstCell);
        center = firstCell.coordinates;
        checker.Add(center);
        int size = 0;
        int overflowCatcher = 0;
        while (size < chunkSize && searchFrontier.Count > 0) {
            overflowCatcher++;
            if (overflowCatcher > 10000) {
                Debug.LogError("queue overflowed");
                break;
            }
            TriCell current = searchFrontier.Dequeue();
            if (current.Elevation < 16) {
                TriDirection d = TriDirection.VERT;
                TriCell k = current;
                if (k.Elevation < 16) {
                    if (k.Elevation == 0) budget -= 6;
                    for (int i = 0; i < 6; i++) {
                        if (!k) break;
                        k.Elevation += 1;
                        k = k.GetNeighbor(d);
                        if (current.inverted)
                            d = d.Next();
                        else
                            d = d.Previous();
                    }
                }
            }

            if (current.Elevation == 1 && budget == 0) {
                break;
            }
            size += 6;
            TriCoordinates coord = current.coordinates;
            for (int i = 0; i < 4; i++) {
                TriCell neighbor = grid.GetCell(coord.X + TriMetrics.hexDir[i, 0], coord.Z + TriMetrics.hexDir[i, 1]);
                if (neighbor && checkbounds(neighbor.coordinates) && !checker.Contains(neighbor.coordinates) && (i == 3 || Random.value < jitterProbability ? true : false)) {
                    searchFrontier.Enqueue(neighbor);
                    checker.Add(neighbor.coordinates);
                }
            }
        }
        searchFrontier.Clear();
        checker.Clear();
        return budget;
    }
    bool checkbounds(TriCoordinates coord) {
        if (coord.X > xMin && coord.X < xMax && coord.Z > zMin && coord.Z < zMax) return true;
        return false;
    }
    void SetTerrainType() {
        for (int i = 0; i < cellCount; i++) {
            TriCell cell = grid.GetCell(i);
            cell.TerrainTypeIndex = (int)Mathf.Pow(cell.Elevation, 0.5f);
        }
    }
    TriCell GetRandomCell() {
        TriCoordinates t = TriMetrics.TriToHex(new TriCoordinates(Random.Range(xMin, xMax), Random.Range(zMin, zMax)));
        return grid.GetCell(t.X, t.Z);
    }

}
