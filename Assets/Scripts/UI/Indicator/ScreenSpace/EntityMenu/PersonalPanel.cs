using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalPanel : EntityPanel {
    public PersonList InsiderList, WorkerList, SpecialList;
    public GameObject GenerationPanel;
    public Button GenerateButton;
    public Progressbar GenProgress;
    public Text SpecialText;
    public void LateUpdate() {
        if (target && target is Inn) {
            GenerateButton.interactable = (target as Inn).CheckNewPerson();
            GenProgress.Value = (target as Inn).BirthStatus;
        }
        InsiderList.Refresh();
        SpecialList.Refresh();
        WorkerList.Refresh();
    }
    public void GeneratePerson() {
        Inn t = target as Inn;
        if (!t) {
            Debug.LogError("Invalid building command");
            return;
        }
        Person worker = t.GetCommandReceiver() as Person;
        worker.Work = t;

    }
    public override void Bind(Entity entity) {
        base.Bind(entity);
        gameObject.SetActive(true);
        if (entity is Building) {
            Building t = entity as Building;
            InsiderList.Bind(t, t.Insider);
            WorkerList.Bind(t, t.Workers);
            if (t is Inn) {
                SpecialList.Bind(t, (t as Inn).Livers);
                SpecialText.text = "Livers";
            }
            else if (t is Company) {
                SpecialList.Bind(t, (t as Company).Officers);
                SpecialText.text = "Officers";
            }
            else if (t is Hall) {
                SpecialList.Bind(t, (t as Hall).Homeless);
                SpecialText.text = "Homeless";
            }
            else {
                SpecialList.Close();
            }
        }
        GenerationPanel.SetActive(entity is Inn);
    }
    public override void Clear() {
        base.Clear();
        InsiderList.Clear();
        SpecialList.Clear();
        WorkerList.Clear();
    }
}
