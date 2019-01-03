using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InventoryViewer : MonoBehaviour {
    public List<Sprite> Sprites;
    public static InventoryViewer Instance;
    public Entity TargetEntity;
    public Inventory Target;
    public GameObject SlotGroup;
    public ItemSlotView SlotPrefab;
    public List<ItemSlotView> Slots;
    public Queue<ItemSlotView> slotPool;
    bool isUnit = false;
    private void Awake() {
        Instance = this;
        Slots = new List<ItemSlotView>();
        slotPool = new Queue<ItemSlotView>();

    }
    public void Start() {
        gameObject.SetActive(false);
    }
    public void Bind(Unit targetUnit) {
        TargetEntity = targetUnit;
        Target = targetUnit.Inventory;
        BindInternal();
    }
    void BindInternal() {
        int i = 0;
        foreach (ItemSlot t in Target.Slots) {
            AddIndicator(i++, t);
        }
        SlotGroup.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, i / ItemSlotView.ColSize * 40);
        gameObject.SetActive(true);
    }
    public void Bind(Building targetBuilding, Inventory target) {
        TargetEntity = targetBuilding;
        Target = target;
        BindInternal();
    }
    public void AddIndicator(int count, ItemSlot target) {
        ItemSlotView tRet = slotPool.Count > 0 ?
            slotPool.Dequeue() :
            Instantiate(SlotPrefab, transform);
        tRet.Bind(target, count);
        tRet.gameObject.SetActive(true);
        tRet.transform.parent = SlotGroup.transform;
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
        ClearInternal();
    }
    void ClearInternal() {
        for (int i = 0; i < Slots.Count; i++) {
            Slots[i].gameObject.SetActive(false);
            slotPool.Enqueue(Slots[i]);
        }
        Slots.Clear();
    }
    public void Refresh() {
        ClearInternal();
        BindInternal();
    }
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }

}
