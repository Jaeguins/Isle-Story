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
    public int sizeX = 120, sizeZ = 60;
    public int isleX = 0, isleZ = 0;
    public GameObject IslandView;
    public Texture2D GenerateThumb() {
        Texture2D ret = new Texture2D(120, 120);
        foreach (TriCell cell in TriGrid.Instance.cells) {
            ret.SetPixel(cell.coordinates.X, 2 * cell.coordinates.Z, cell.GetColor());
            ret.SetPixel(cell.coordinates.X, 2 * cell.coordinates.Z + 1, cell.GetColor());
        }
        return ret;
    }
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

    public string SaveName = Strings.NaN;
    public string IsleName = Strings.NaN;
    public string IslePath;
    public TriGrid grid;
    public void NewMap() {
        NewMap(isleX, isleZ);
    }
    public void NewMap(int x,int z) {
        isleX = x;
        isleZ = z;
        Awake();
        StartCoroutine(NewMapInternal());
    }
    void Awake() {
        SaveName = Intent.GetData<string>(Strings.SaveInd);
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/save/" + SaveName + "/" + IsleName);
        if (IsleName != Strings.NaN) {
            IslePath = di.FullName;
            if (di.Exists == false) {
                di.Create();
            }
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
        File.WriteAllBytes(Path.Combine(path, "thumbnailImg.png"), GenerateThumb().EncodeToPNG());
    }
    public void Load() {
        Awake();
        StartCoroutine(LoadInternal());
    }
    public void Load(string iPath) {
        IslePath = iPath;
        Load();
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
        Time.timeScale = 1f;
        IslandView.SetActive(false);
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
        IslandView.SetActive(false);
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
