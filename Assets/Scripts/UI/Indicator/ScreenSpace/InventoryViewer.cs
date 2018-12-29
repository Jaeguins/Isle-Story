using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InventoryViewer: MonoBehaviour {
    public static InventoryViewer Instance;
    public Entity TargetEntity;
    public Inventory Target;
    public GameObject SlotGroup;
    public ItemSlotView SlotPrefab;
    public List<ItemSlotView> Slots;
    public Queue<ItemSlotView> slotPool;
    private void Awake() {
        Instance = this;
        Slots = new List<ItemSlotView>();
        slotPool = new Queue<ItemSlotView>();

    }
    public void Start() {
        gameObject.SetActive(false);
    }
    public void Bind(Entity targetEntity,Inventory target) {
        TargetEntity = targetEntity;
        Target = target;
        int i = 0;
        foreach (ItemSlot t in Target.Slots) {
            AddIndicator(i++, t);
        }
        gameObject.SetActive(true);
    }
    public void AddIndicator(int count, ItemSlot target) {
        ItemSlotView tRet = slotPool.Count > 0 ?
            slotPool.Dequeue() :
            Instantiate(SlotPrefab, transform);
        tRet.Bind(target, count);
        tRet.gameObject.SetActive(true);
        Slots.Add(tRet);
    }

    void LateUpdate() {
        if (Target != null && Target.refreshed) {
            Refresh();
        }
    }

    public void Clear() {
        TargetEntity = null;
        Target = null;
    }
    public void Refresh() {
        Entity t = TargetEntity;
        Inventory tList = Target;
        Clear();
        Bind(t, tList);
    }
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }

}
