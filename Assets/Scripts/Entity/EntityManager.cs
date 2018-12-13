using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {
    public Transform BuildingGroup;
    public Transform UnitGroup;
    public Transform NaturalGroup;
    public TriGrid grid;
    Dictionary<int, Building> buildings;
    Dictionary<int, Unit> units;
    Dictionary<int, Natural> naturals;

    private void Start() {
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

    public void Save(string path) {
        int k = 0;
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "natural.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(naturals.Count);
            k = 0;
            foreach (KeyValuePair<int, Natural> b in naturals) {
                writer.Write(k++);
                switch (b.Value.type) {
                    case NaturalType.TREE:
                        ((Tree)b.Value).Save(writer);
                        break;
                }

            }
        }
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "building.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(buildings.Count);
            k = 0;
            foreach (KeyValuePair<int, Building> b in buildings) {
                writer.Write(k++);
                switch (b.Value.type) {
                    case BuildingType.INN:
                        switch (((Inn)b.Value).subType) {
                            case InnType.TENT:
                                ((Tent)b.Value).Save(writer);
                                break;
                        }
                        break;
                }
            }
        }
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "unit.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(units.Count);
            k = 0;
            foreach (KeyValuePair<int, Unit> b in units) {
                writer.Write(k++);
                switch (b.Value.type) {
                    case UnitType.PERSON:
                        ((Person)b.Value).Save(writer);
                        break;
                }

            }
        }
    }

    public void ClearEntities() {
        foreach (KeyValuePair<int, Building> b in buildings) Destroy(b.Value.gameObject);
        buildings.Clear();
        foreach (KeyValuePair<int, Unit> b in units) Destroy(b.Value.gameObject);
        units.Clear();
        foreach (KeyValuePair<int, Natural> b in naturals) Destroy(b.Value.gameObject);
        naturals.Clear();
    }

    public void Load(string path){
        ClearEntities();
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "natural.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++) {
                    int id = reader.ReadInt32();
                    TriCoordinates coord = TriCoordinates.Load(reader);
                    Natural loaded = Natural.Load(reader);
                    if (loaded) {
                        loaded.ID = id;
                        loaded.Location = grid.GetCell(coord);
                        loaded.transform.parent = NaturalGroup;
                        AddNatural(loaded);
                        loaded.validateRotation();
                    }
                }
            }
            else {
                Debug.LogWarning("Unknown building format " + header);
            }
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "building.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++) {
                    int id = reader.ReadInt32();
                    TriCoordinates coord = TriCoordinates.Load(reader);
                    Building loaded = Building.Load(reader);
                    if (loaded) {
                        loaded.ID = id;
                        loaded.Location = grid.GetCell(coord);
                        loaded.Location.Entity = loaded;
                        loaded.transform.parent = BuildingGroup;
                        AddBuilding(loaded);
                    }
                }
            }
            else {
                Debug.LogWarning("Unknown building format " + header);
            }
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "unit.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++) {
                    int id = reader.ReadInt32();
                    TriCoordinates coord = TriCoordinates.Load(reader);
                    Unit loaded = Unit.Load(reader);
                    if (loaded) {
                        loaded.ID = id;
                        loaded.Location = grid.GetCell(coord);
                        if (grid.GetCell(coord).Entity) {
                            loaded.GetIn((Building)grid.GetCell(coord).Entity);
                        }
                        loaded.transform.parent = UnitGroup;
                        AddUnit(loaded);
                    }
                }
            }
            else {
                Debug.LogWarning("Unknown unit format " + header);
            }
        }
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
        unit.liverList = GameUI.Instance.personList;
    }

    public void AddNatural(Natural unit) {
        unit.ID = naturals.Count;
        naturals.Add(unit.ID, unit);
        unit.transform.SetParent(NaturalGroup, false);
    }

}
