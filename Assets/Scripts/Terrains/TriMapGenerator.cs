using UnityEngine;
using System.Collections.Generic;

public class TriMapGenerator : MonoBehaviour {
    int cellCount;
    public TriGrid grid;
    Queue<TriCell> searchFrontier;
    HashSet<TriCoordinates> checker;
    List<ClimateData> climate = new List<ClimateData>();
    [Range(0f, 1f)]
    public float evaporationFactor = 0.5f;
    [Range(0f, 1f)]
    public float precipitationFactor = 0.25f;
    [Range(0f, 1f)]
    public float runoffFactor = 0.25f;
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
    void EvolveClimate(int cellIndex) {
        TriCell cell = grid.GetCell(cellIndex);
        ClimateData cellClimate = climate[cellIndex];

        if (cell.IsUnderwater) {
            cellClimate.moisture = 1f;
            cellClimate.clouds += evaporationFactor;
        }
        else {
            float evaporation = cellClimate.moisture * evaporationFactor;
            cellClimate.moisture -= evaporation;
            cellClimate.clouds += evaporation;
        }
        float precipitation = cellClimate.clouds * precipitationFactor;
        cellClimate.clouds -= precipitation;
        cellClimate.moisture += precipitation;
        float cloudDispersal = cellClimate.clouds * (1f / 6f);
        float runoff = cellClimate.moisture * runoffFactor * (1f / 6f);
        for (int i = 0; i < 4; i++) {
            TriCell neighbor = grid.GetCell(TriMetrics.TriToHex(cell.coordinates) + new Vector2Int(TriMetrics.hexDir[i, 0], TriMetrics.hexDir[i, 1]));
            if (!neighbor) {
                continue;
            }
            ClimateData neighborClimate = climate[neighbor.Index];
            neighborClimate.clouds += cloudDispersal;
            int elevationDelta = neighbor.Elevation - cell.Elevation;
            if (elevationDelta < 0) {
                cellClimate.moisture -= runoff;
                neighborClimate.moisture += runoff;
            }
            climate[neighbor.Index] = neighborClimate;
        }
        cellClimate.clouds = 0f;

        climate[cellIndex] = cellClimate;
    }
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
        CreateClimate();
        SetTerrainType();
    }

    void CreateClimate() {
        climate.Clear();
        ClimateData initialData = new ClimateData();
        for (int i = 0; i < cellCount; i++) {
            climate.Add(initialData);
        }
        for (int cycle = 0; cycle < 40; cycle++) {
            for (int i = 0; i < cellCount; i++) {
                TriCoordinates t = grid.GetCell(i).coordinates;
                if (t == TriMetrics.TriToHex(t))
                    EvolveClimate(i);
            }
        }
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
            TriCell cell = grid.GetCell(i), hexCell = grid.GetCell(TriMetrics.TriToHex(cell.coordinates));
            if (cell.coordinates == hexCell.coordinates) {
                float moisture = climate[hexCell.Index].moisture;
                TriDirection d = TriDirection.VERT;
                for (int j = 0; j < 6; j++) {
                    if (!cell) break;
                    if (!cell.IsUnderwater) {
                        if (cell.Elevation > 10) {
                            cell.TerrainTypeIndex = 4;
                        }
                        else if (moisture < 0.02f) {
                            cell.TerrainTypeIndex = 3;
                        }
                        else if (moisture < 0.12f) {
                            cell.TerrainTypeIndex = 2;
                        }
                        else if (moisture < 0.28f) {
                            cell.TerrainTypeIndex = 1;
                        }
                        else if (moisture < 0.85f) {
                            cell.TerrainTypeIndex = 0;
                        }
                        else {
                            cell.TerrainTypeIndex = 0;
                        }
                    }
                    else {
                        cell.TerrainTypeIndex = 1;
                    }
                    cell = cell.GetNeighbor(d);
                    if (hexCell.inverted)
                        d = d.Next();
                    else
                        d = d.Previous();
                }
            }
        }


    }
    TriCell GetRandomCell() {
        TriCoordinates t = TriMetrics.TriToHex(new TriCoordinates(Random.Range(xMin, xMax), Random.Range(zMin, zMax)));
        return grid.GetCell(t.X, t.Z);
    }

}
