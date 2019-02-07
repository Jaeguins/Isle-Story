using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
public class TriIsland : MonoBehaviour {
    public TopViewCam topCam;
    public List<PrefabManager> buildings;
    public PrefabManager units;
    public PrefabManager naturals;
    public EntityManager entities;
    public TriMapEditor editor;
    public TriMapGenerator mapGenerator;
    public static TriIsland Instance;
    public int sizeX=120,sizeZ=60;
    public int isleX=0, isleZ=0;
    public static bool Loaded {
        get {
            return Instance.loaded;
        }
        set {
            Instance.loaded = value;
        }
    }
    public bool loaded = false;
    public static Building GetCamp() {
        return Instance.entities.GetCamp();
    }
    public string SaveName = "save1";
    public string IsleName = "test1";
    public string IslePath;
    int version = 0;
    public TriGrid grid;

    public void NewMap() {
        StartCoroutine(NewMapInternal());
    }

    void Awake() {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/save/" + SaveName + "/" + IsleName);
        IslePath = di.FullName;
        if (di.Exists == false) {
            di.Create();
        }
        Instance = this;

    }
    public void Save() {//TODO Saving function
        string path = IslePath;//Path.Combine(Application.persistentDataPath, isleName);
        using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(path, "thumbnail.dat"), FileMode.Create))) {
            writer.Write(isleX);
            writer.Write(isleZ);
        }
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
    public void Load() {
        StartCoroutine(LoadInternal());
    }

    public IEnumerator<Coroutine> NewMapInternal() {
        Time.timeScale = 0;
        Loaded = false;
        entities.ClearEntities();
        yield return StartCoroutine(grid.CreateMap(sizeX, sizeZ));
        yield return StartCoroutine(mapGenerator.GenerateMap(sizeX, sizeZ));
        Loaded = true;
        topCam.ValidatePosition();
        Selector.Instance.RequestLocation(null, SizeType.HEX, new BuildCommand(null));
        Time.timeScale = 1;
    }

    public IEnumerator<Coroutine> LoadInternal() {//TODO Load function
        Time.timeScale = 0;
        string path = IslePath;//Path.Combine(Application.persistentDataPath, isleName);
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "thumbnail.dat")))) {
            isleX = reader.ReadInt32();
            isleZ = reader.ReadInt32();
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "world.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 0) {
                Clock.Instance.currentTimeOfDay = reader.ReadSingle();
            }
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(path, "map.dat")))) {
            int header = reader.ReadInt32();
            if (header <= 2) {
                yield return StartCoroutine(grid.Load(reader, header));
                topCam.ValidatePosition();
            }
            else {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
        yield return StartCoroutine(entities.Load(path));
        Time.timeScale = 1;
    }
    public static Entity GetBuildingPrefabs(int mainType, int subType, int index) {
        return Instance.buildings[mainType][subType][index];
    }
    public static Entity GetUnitPrefabs(int mainType, int subType) {
        return Instance.units[mainType][subType];
    }
    public static Entity GetNaturalPrefabs(int mainType, int subType) {
        return Instance.naturals[mainType][subType];
    }
}
