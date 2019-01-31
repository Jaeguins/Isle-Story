using UnityEngine;
using System.Collections.Generic;

public class TriMapGenerator : MonoBehaviour {
    int cellCount, landCells;
    public TriGrid grid;
    public EntityManager entities;
    Queue<TriCell> searchFrontier;
    HashSet<TriCoordinates> checker;
    List<TriDirection> flowDirections = new List<TriDirection>();
    public List<ClimateData> climate = new List<ClimateData>();
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
    [Range(0, 40)]
    public int mapBorderX = 30;
    [Range(0, 40)]
    public int mapBorderZ = 30;
    [Range(0, 100)]
    public int elevationMaximum = 15;
    [Range(0, 20)]
    public int riverPercentage = 10;
    int xMin, xMax, zMin, zMax;
    int searchFrontierPhase;

    void SetClimateData(int cellIndex,float moisture,float clouds) {
        TriDirection d = TriDirection.VERT;
        TriCell current, k;
        ClimateData t;
        current = k = grid.GetCell(cellIndex);
        for (int i = 0; i < 6; i++) {
            if (!k) break;

            t= new ClimateData();
            t.moisture = moisture;
            t.clouds = clouds;
            climate[k.Index] = t;

            k = k.GetNeighbor(d);
            if (current.inverted)
                d = d.Next();
            else
                d = d.Previous();
        }
    }

    void EvolveClimate(int cellIndex) {
        TriCell cell = grid.GetCell(cellIndex);
        ClimateData cellClimate = climate[cellIndex];
        float tMoisture = cellClimate.moisture, tClouds = cellClimate.clouds;
        if (cell.IsUnderwater) {
            tMoisture = 1f;
            tClouds += evaporationFactor;
        }
        else {
            float evaporation = tMoisture * evaporationFactor;
            tMoisture -= evaporation;
            tClouds += evaporation;
        }
        float precipitation = tClouds * precipitationFactor;
        tClouds -= precipitation;
        tMoisture += precipitation;
        //SetClimateData(cellIndex, tMoisture, tClouds);
        float cloudDispersal = tClouds * (1f / 6f);
        float runoff = tMoisture * runoffFactor * (1f / 6f);
        for (int i = 0; i < 4; i++) {
            TriCell neighbor = grid.GetCell(TriMetrics.TriToHex(cell.coordinates) + new Vector2Int(TriMetrics.hexDir[i, 0], TriMetrics.hexDir[i, 1]));
            if (!neighbor) {
                continue;
            }
            ClimateData neighborClimate = climate[neighbor.Index];
            float tNMoisture = neighborClimate.moisture, tNClouds = neighborClimate.clouds;
            tNClouds += cloudDispersal;
            int elevationDelta = neighbor.Elevation - cell.Elevation;
            if (elevationDelta < 0) {
                tMoisture -= runoff;
                tNMoisture += runoff;
            }
            SetClimateData(neighbor.Index, tNMoisture, tNClouds);
            //climate[neighbor.Index] = neighborClimate;
        }
        tClouds = 0f;
        SetClimateData(cellIndex, tMoisture, tClouds);
        //climate[cellIndex] = cellClimate;
    }
    
    public IEnumerator<Coroutine> GenerateMap(int x, int z) {
        Selector.Instance.CancelCommand();
        cellCount = x * z;
        if (searchFrontier == null) {
            searchFrontier = new Queue<TriCell>();
            checker = new HashSet<TriCoordinates>();
        }
        xMin = mapBorderX;
        xMax = x - mapBorderX;
        zMin = mapBorderZ;
        zMax = z - mapBorderZ;
        yield return StartCoroutine(CreateLand());
        yield return StartCoroutine(CreateClimate());
        yield return StartCoroutine(CreateRivers());
        yield return StartCoroutine(SetTerrainType());
    }

    IEnumerator<WaitForEndOfFrame> CreateRivers() {
        List<TriCell> riverOrigins = ListPool<TriCell>.Get();
        for (int i = 0; i < cellCount; i++) {
            TriCell cell = grid.GetCell(i);
            if (cell.IsUnderwater) {
                continue;
            }
            ClimateData data = climate[i];
            
            float weight =
                (float)(cell.Elevation) /
                (float)(elevationMaximum);
            //grid.labels[i].text = weight.ToString("F");
            if (weight > 0.75f) {
                riverOrigins.Add(cell);
                riverOrigins.Add(cell);
            }
            if (weight > 0.5f) {
                riverOrigins.Add(cell);
            }
            if (weight > 0.25f) {
                riverOrigins.Add(cell);
            }
        }
        int riverBudget = Mathf.RoundToInt(landCells * riverPercentage * 0.01f);
        int overflowChecker = 0;
        while (riverBudget > 0 && riverOrigins.Count > 0) {
            if (overflowChecker++ > 1000) {
                Debug.Log("checking river stack overflowed");
                break;
            }
            int index = Random.Range(0, riverOrigins.Count);
            int lastIndex = riverOrigins.Count - 1;
            TriCell origin = riverOrigins[index];
            riverOrigins[index] = riverOrigins[lastIndex];
            riverOrigins.RemoveAt(lastIndex);
            if (!origin.HasRiver) {
                riverBudget -= CreateRiver(origin);
            }
            yield return null;
        }

        if (riverBudget > 0) {
            Debug.LogWarning("Failed to use up river budget.");
        }
        ListPool<TriCell>.Add(riverOrigins);
    }

    int CreateRiver(TriCell origin) {
        int length = 0;
        TriCell cell = origin;
        TriDirection direction = TriDirection.VERT;
        while (!cell.IsUnderwater) {
            flowDirections.Clear();
            for (TriDirection d = TriDirection.VERT; d <= TriDirection.RIGHT; d++) {
                TriCell neighbor = cell.GetNeighbor(d);
                if (!neighbor || neighbor.HasRiver) {
                    continue;
                }
                int delta = neighbor.Elevation - cell.Elevation;
                if (delta > 0) {
                    continue;
                }
                if (delta < 0) {
                    flowDirections.Add(d);
                    flowDirections.Add(d);
                    flowDirections.Add(d);
                }
                if (
                    length == 1 ||
                    (d != direction.Next2() && d != direction.Previous2())
                ) {
                    flowDirections.Add(d);
                }
                flowDirections.Add(d);
            }
            if (flowDirections.Count == 0) {
                return length > 1 ? length : 0;
            }
            direction= flowDirections[Random.Range(0, flowDirections.Count)];
            cell.SetRiver(direction);
            cell.GetNeighbor(direction).SetRiver(direction);
            length += 1;
            cell = cell.GetNeighbor(direction);
        }
        return length;
    }

    IEnumerator<WaitForEndOfFrame> CreateClimate() {
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
        yield return null;
    }

    IEnumerator<WaitForEndOfFrame> CreateLand() {
        int landBudget = Mathf.RoundToInt(cellCount * landPercentage * 0.01f);
        landCells = landBudget;
        bool initiated = false;
        while (landBudget > 0) {
            landBudget = RaiseTerrain(
                Random.Range(chunkSizeMin, chunkSizeMax + 1), landBudget, initiated
            );
            initiated = true;
            yield return null;
        }
        yield return null;
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
        searchFrontier.Enqueue(firstCell);
        center = firstCell.coordinates;
        checker.Add(center);
        int size = 0;
        while (size < chunkSize && searchFrontier.Count > 0) {
            TriCell current = searchFrontier.Dequeue();
            if (current.Elevation < elevationMaximum) {
                TriDirection d = TriDirection.VERT;
                TriCell k = current;
                if (k.Elevation < elevationMaximum) {
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

    private bool checkbounds(TriCoordinates coord) {
        if (coord.X > xMin && coord.X < xMax && coord.Z > zMin && coord.Z < zMax) return true;
        return false;
    }

    IEnumerator<WaitForEndOfFrame> SetTerrainType() {
        for (int i = 0; i < cellCount; i++) {
            TriCell cell = grid.GetCell(i), hexCell = grid.GetCell(TriMetrics.TriToHex(cell.coordinates));
            if (cell.coordinates == hexCell.coordinates) {
                float moisture = climate[hexCell.Index].moisture;
                TriDirection d = TriDirection.VERT;
                TriIsland isle = TriIsland.Instance;
                for (int j = 0; j < 6; j++) {
                    if (!cell) break;
                    if (!cell.IsUnderwater) {
                        if (cell.Elevation > 10) {
                            cell.TerrainTypeIndex = 4;
                        }
                        else if (moisture < 0.02f) {
                            cell.TerrainTypeIndex = 3;
                            if (Random.value < 0.5f&&!cell.HasRiver) {
                                Tree t=(Tree)Instantiate(TriIsland.GetNaturalPrefabs((int)NaturalType.TREE,0),isle.transform);
                                t.Location = cell;
                                t.EntranceDirection = (TriDirection)((int)(Random.value * 3f));
                                entities.AddNatural(t);
                            }
                        }
                        else if (moisture < 0.12f) {
                            cell.TerrainTypeIndex = 2;
                            if (Random.value < 0.2f&&!cell.HasRiver) {
                                Tree t = (Tree)Instantiate(TriIsland.GetNaturalPrefabs((int)NaturalType.TREE, 0), isle.transform);
                                t.Location = cell;
                                t.EntranceDirection = (TriDirection)((int)(Random.value * 3f));
                                entities.AddNatural(t);
                            }
                        }
                        else if (moisture < 0.20f) {
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
                if (i % Strings.refreshLimit == 0) {
                    yield return null;
                }
                    
            }
        }


    }

    TriCell GetRandomCell() {
        TriCoordinates t = TriMetrics.TriToHex(new TriCoordinates(Random.Range(xMin, xMax), Random.Range(zMin, zMax)));
        return grid.GetCell(t.X, t.Z);
    }

}
