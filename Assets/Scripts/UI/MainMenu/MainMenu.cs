using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class MainMenu : MonoBehaviour
{
    public InputField NewSaveName;
    public GameObject ContinueButton;
    public string LoadSaveName;
    public TextChecker nameChecker;
    public List<Save> NowList = new List<Save>();
    public List<string> nameList = new List<string>();

    public string lastSave, lastPath;
    public void Awake() {
        lastPath = Path.Combine(Application.persistentDataPath, "lastSave.dat");
    }
    public void Start() {
        RefreshSaveList();
        if (File.Exists(lastPath))
            using (BinaryReader reader = new BinaryReader(File.OpenRead(lastPath))) {
                lastSave = reader.ReadString();
            }
        else
            ContinueButton.SetActive(false);
    }
    public void NewGame() {
        StartIneternal(NewSaveName.text);
    }

    public void LoadGame() {
        StartIneternal(LoadSaveName);
    }

    public void Continue() {
        StartIneternal(lastSave);
    }

    void StartIneternal(string saveVal) {
        Intent.InputData(Strings.SaveInd, saveVal);
        SceneManager.LoadScene(Strings.GameSceneName);
        using(BinaryWriter writer=new BinaryWriter(File.Open(lastPath,FileMode.Create))) {
            writer.Write(saveVal);
        }
    }
    public void RefreshSaveList() {
        NowList.Clear();
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/save");
        if (di.Exists) {
            NowList.Clear();
            nameList.Clear();
            foreach (DirectoryInfo dir in di.GetDirectories()) {
                Save tmp = new Save {
                    Name = dir.Name,
                    Path = dir.FullName
                };
                NowList.Add(tmp);
                nameList.Add(dir.Name);
            }
            nameChecker.Refresh(nameList);
        }
    }
}
