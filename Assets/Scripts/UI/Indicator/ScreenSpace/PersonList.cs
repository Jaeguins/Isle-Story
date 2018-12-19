using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonList : MonoBehaviour {
    public List<PersonIndicator> buttons=new List<PersonIndicator>();
    Queue<PersonIndicator> buttonPool=new Queue<PersonIndicator>();
    public Transform contentList;
    public GameObject listPrefab;
    public Selector selector;
    public static PersonList Instance;
    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void ClearList() {
        for (int i = 0; i < buttons.Count; i++) {
            RemoveButton(0);
        }
    }

    void RemoveButton(int index) {
        buttons[index].gameObject.SetActive(false);
        buttons[index].button.onClick.RemoveAllListeners();
        buttonPool.Enqueue(buttons[index]);
        buttons.RemoveAt(index);
    }

    void AddButton(int index,Person person) {
        PersonIndicator ind =
            buttonPool.Count > 0 ?
            buttonPool.Dequeue()
            : Instantiate(listPrefab, contentList).GetComponent<PersonIndicator>();
        buttons.Add(ind);
        ind.gameObject.SetActive(true);
        RectTransform tp = (RectTransform)buttons[index].gameObject.transform;
        Vector3 tV = tp.localPosition;
        tV.y = -30 * index + 75;
        tp.localPosition = tV;
        buttons[index].nameText.text = person.UIName;
        buttons[index].selector = selector;
        buttons[index].target = person;
        buttons[index].button.onClick.AddListener(buttons[index].SelectUnit);
    }
    public void AddPerson(Person person) {
        int t = buttons.Count;
        AddButton(t,person);

    }
    public void RemovePerson(Person person) {
        PersonIndicator t=null;
        foreach(PersonIndicator b in buttons) {
            if (b.target == person) {
                t = b;
            }
        }
        if(!t)
        RemoveButton(buttons.IndexOf(t));
    }
    public void SetActive(bool val) {
        gameObject.SetActive(val);
        if (!val) ClearList();
    }
    public void Hide() {
        SetActive(false);
    }
}
