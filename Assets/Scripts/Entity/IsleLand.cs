using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class IsleLand : MonoBehaviour {
    public List<Inn> innPrefabs;
    public static IsleLand Instance;
    string isleName = "test";
    int version = 0;
    public TriGrid grid;
    Dictionary<int, Building> buildings;
    Dictionary<int, Unit> units;
    void Awake() {
        Instance = this;
        buildings = new Dictionary<int, Building>();
        units = new Dictionary<int, Unit>();
    }
    public void Save() {
        int k = 0;
        string path = Path.Combine(Application.persistentDataPath,isleName);
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path,"building.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(buildings.Count);
            k = 0;
            foreach (Building b in buildings.Values) {
                b.Save(writer,k++);
            }
            
        }
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "unit.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(units.Count);
            k = 0;
            foreach (Unit b in units.Values) {
                b.Save(writer,k++);
            }
        }
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "map.dat"), FileMode.Create))) {
            writer.Write(2);
            grid.Save(writer);
        }
    }

    public void Load(){
        string path = Path.Combine(Application.persistentDataPath, isleName);
        ClearIsle();
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path,"map.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 2) {
                grid.Load(reader, header);
                TriMapCamera.ValidatePosition();
            }
            else {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "building.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                int counter = reader.ReadInt32();
                for(int i = 0; i < counter; i++) {
                    int id = reader.ReadInt32();
                    TriCoordinates coordinates = TriCoordinates.Load(reader);
                    Building loaded = Building.Load(reader);
                    loaded.ID = id;
                    loaded.transform.parent = transform;
                    AddBuilding(loaded);
                }
            }
            else {
                Debug.LogWarning("Unknown building format " + header);
            }
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "unit.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 2) {
                grid.Load(reader, header);
                TriMapCamera.ValidatePosition();
            }
            else {
                Debug.LogWarning("Unknown unit format " + header);
            }
        }
    }
    public void ClearIsle() {
        foreach (Building b in buildings.Values) Destroy(b);
        buildings.Clear();
        foreach (Unit b in units.Values) Destroy(b);
        units.Clear();
    }
    public void AddUnit(Unit unit) {
        units.Add(unit.ID,unit);
        unit.transform.SetParent(transform, false);
    }
    public void AddBuilding(Building unit) {
        buildings.Add(unit.ID,unit);
        unit.transform.SetParent(transform, false);
    }
    /*void CreateUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && !cell.Entity) {
            triGrid.AddUnit(
                Instantiate((Unit)Entity.unitPrefab), cell, Random.Range(0f, 360f));
        }
    }*/
}
