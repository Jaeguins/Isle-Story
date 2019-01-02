using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WareHouse : Company{
    public Inventory Inventory;
    /*
     * save sequence
     * supperclassed saves
     * >inventory
     */
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        Inventory.Save(writer);
    }
    public new static WareHouse Load(BinaryReader reader) {
        WareHouse ret = (WareHouse)Instantiate(TriIsleland.Instance.companyPrefabs[0]);
        ret.Inventory=Inventory.Load(reader);
        return ret;
    }
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        if (UnderConstruct) return;
        if (Officers.Count > 0)
            menu.BindButton(1, "Officers", ShowOfficers);
    }
    private void LateUpdate() {
    }
}
