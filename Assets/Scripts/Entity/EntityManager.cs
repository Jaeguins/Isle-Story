using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {
    public Transform BuildingGroup;
    public Transform UnitGroup;
    public Transform NaturalGroup;
    public TriGrid grid;
    public Hall camp;
    public Dictionary<int, Building> buildings;
    public Dictionary<int, Unit> units;
    public Dictionary<int, Natural> naturals;
    public Building GetCamp() {
        return camp;
    }
    private void Awake() {
        buildings = new Dictionary<int, Building>();
        units = new Dictionary<int, Unit>();
        naturals = new Dictionary<int, Natural>();
    }

    public int UnitCount {
        get {
            return units.Count;
        }
    }

    public int BuildingCount {
        get {
            return buildings.Count;
        }
    }

    public int NaturalCount {
        get {
            return naturals.Count;
        }
    }

    public void SaveNatural(string path) {
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "natural.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(naturals.Count);
            int k = 0;
            foreach (KeyValuePair<int, Natural> b in naturals) {
                switch (b.Value.type) {
                    case NaturalType.TREE:
                        ((Tree)b.Value).Save(writer);
                        break;
                }

            }
        }
    }

    public void SaveBuilding(string path) {
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "building.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(buildings.Count);
            foreach (KeyValuePair<int, Building> b in buildings) {
                if (b.Value == null) continue;
                switch (b.Value.type) {
                    case BuildingType.HALL:
                        (b.Value as Hall).Save(writer);
                        break;
                    case BuildingType.INN:
                        switch (((Inn)b.Value).subType) {
                            case InnType.TENT:
                                ((Tent)b.Value).Save(writer);
                                break;
                        }
                        break;
                    case BuildingType.COMPANY:
                        switch (((Company)b.Value).subType) {
                            case CompType.WAREHOUSE:
                                ((WareHouse)b.Value).Save(writer);
                                break;
                        }
                        break;
                    case BuildingType.WORKSITE:
                        switch (((Worksite)b.Value).subType) {
                            case WorkType.FARMLAND:
                                ((Farmland)b.Value).Save(writer);
                                break;
                        }
                        break;
                }
            }
        }
    }

    public void SaveUnit(string path) {
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "unit.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(units.Count);
            foreach (KeyValuePair<int, Unit> b in units) {
                if (b.Value == null) continue;
                switch (b.Value.type) {
                    case UnitType.PERSON:
                        ((Human)b.Value).Save(writer);
                        break;
                }

            }
        }
    }

    public void Save(string path) {
        SaveNatural(path);
        SaveBuilding(path);
        SaveUnit(path);
    }
    public void ClearUnits() {
        foreach (KeyValuePair<int, Unit> b in units) Destroy(b.Value.gameObject,1.0f);
        units.Clear();
    }
    public void ClearBuildings() {
        foreach (KeyValuePair<int, Building> b in buildings) Destroy(b.Value.gameObject, 1.0f);
        buildings.Clear();
    }
    public void ClearNaturals() {
        foreach (KeyValuePair<int, Natural> b in naturals) Destroy(b.Value.gameObject, 1.0f);
        naturals.Clear();
    }

    public void ClearEntities() {
        ClearBuildings();
        ClearUnits();
        ClearNaturals();
    }

    public IEnumerator<Coroutine> LoadNatural(string path) {
        ClearNaturals();
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "natural.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++) {
                    TriCoordinates coord = TriCoordinates.Load(reader);
                    Natural loaded = Natural.Load(reader);
                    if (loaded) {
                        loaded.ID = i;
                        loaded.Location = grid.GetCell(coord);
                        loaded.transform.SetParent(NaturalGroup);
                        AddNatural(loaded);
                        loaded.validateRotation();
                    }
//                    if (i % Strings.refreshLimit == 0) yield return null;
                }
            }
            else {
                Debug.LogWarning("Unknown naturals format " + header);
            }
        }
        yield return null;
    }

    public IEnumerator<Coroutine> LoadBuilding(string path) {
        ClearBuildings();
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "building.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++) {
                    TriCoordinates coord = TriCoordinates.Load(reader);
                    Building loaded = Building.Load(reader);
                    if (loaded.type == BuildingType.HALL) camp = loaded as Hall;
                    if (loaded) {
                        loaded.ID = i;
                        loaded.Location = grid.GetCell(coord);
                        loaded.EntranceDirection = loaded.EntranceDirection;
                        loaded.Location.Statics = loaded;
                        loaded.transform.SetParent(BuildingGroup);
                        AddBuilding(loaded);
                    }
//                    if (i % Strings.refreshLimit == 0) yield return null;
                }
            }
            else {
                Debug.LogWarning("Unknown building format " + header);
            }
        }
        yield return null;
    }

    public IEnumerator<Coroutine> LoadUnit(string path) {
        ClearUnits();
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "unit.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++) {
                    TriCoordinates coord = TriCoordinates.Load(reader);
                    Unit loaded = Unit.Load(reader);
                    if (loaded) {
                        loaded.ID = i;
                        loaded.Location = grid.GetCell(coord);
                        if (grid.GetCell(coord).Statics) {
                            loaded.GetIn((Building)grid.GetCell(coord).Statics);
                        }
                        loaded.transform.SetParent(UnitGroup);
                        AddUnit(loaded);
                    }
//                    if (i % Strings.refreshLimit == 0) yield return null;
                }
            }
            else {
                Debug.LogWarning("Unknown unit format " + header);
            }
        }
        yield return null;
    }

    public IEnumerator Load(string path){
        ClearEntities();
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(LoadNatural(path));
        Debug.Log(naturals.Count + " natural(s) loaded.");
        yield return StartCoroutine(LoadBuilding(path));
        Debug.Log(buildings.Count + " building(s) loaded.");
        yield return StartCoroutine(LoadUnit(path));
        Debug.Log(units.Count + " unit(s) loaded.");
        TriIsland.Loaded = true;
    }

    public void AddUnit(Unit unit) {
        units.Add(unit.ID, unit);
        unit.transform.SetParent(UnitGroup, false);
    }

    public void RemoveUnit(int id) {
        Unit unit = units[id];
        Destroy(unit.gameObject);
        units.Remove(id);
    }

    public void RemoveBuilding(int id) {
        Building unit = buildings[id];
        Destroy(unit.gameObject);
        buildings.Remove(id);
    }

    public void AddBuilding(Building unit) {
        unit.ID = buildings.Count;
        buildings.Add(unit.ID, unit);
        unit.transform.SetParent(BuildingGroup, false);
    }

    public void AddNatural(Natural unit) {
        unit.ID = naturals.Count;
        naturals.Add(unit.ID, unit);
        unit.transform.SetParent(NaturalGroup, false);
    }
}
