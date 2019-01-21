﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
public class TriIsleland : MonoBehaviour {
    public TopViewCam topCam;
    public List<PrefabManager> buildings;
    public PrefabManager units;
    public PrefabManager naturals;
    public EntityManager entities;
    public static TriIsleland Instance;
    public static Building GetCamp() {
        return Instance.entities.GetCamp();
    }
    string isleName = "test";
    int version = 0;
    public TriGrid grid;
    void Awake() {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/" + isleName);
        if (di.Exists == false) {
            di.Create();

        }
        Instance = this;

    }
    public void Save() {//TODO Saving function
        string path = Path.Combine(Application.persistentDataPath, isleName);
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "world.dat"), FileMode.Create))) {
            writer.Write(0);
            writer.Write(Clock.GetTime());
        }
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "map.dat"), FileMode.Create))) {
            writer.Write(2);
            grid.Save(writer);
        }
        entities.Save(path);
    }

    public void Load() {//TODO Load function
        string path = Path.Combine(Application.persistentDataPath, isleName);
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "world.dat")))) {
            int header=reader.ReadInt32();
            if (header <= 0) {
                Clock.Instance.currentTimeOfDay = reader.ReadSingle();
            }
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "map.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 2) {
                grid.Load(reader, header);
                topCam.ValidatePosition();
            }
            else {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
        entities.Load(path);
    }
    public static Entity GetBuildingPrefabs(int mainType,int subType,int index) {
        return Instance.buildings[mainType][subType][index];
    }
    public static Entity GetUnitPrefabs(int mainType, int subType) {
        return Instance.units[mainType][subType];
    }
    public static Entity GetNaturalPrefabs(int mainType, int subType) {
        return Instance.naturals[mainType][subType];
    }
}
