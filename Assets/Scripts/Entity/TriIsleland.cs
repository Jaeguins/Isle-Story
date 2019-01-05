using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
public class TriIsleland : MonoBehaviour {
    public TopViewCam topCam;
    public List<Inn> innPrefabs;
    public List<Unit> unitPrefabs;
    public List<Natural> naturalPrefabs;
    public List<Worksite> worksitePrefabs;
    public List<Company> companyPrefabs;
    public EntityManager entities;
    public static TriIsleland Instance;
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

}
