using UnityEngine.UI;
using System.Collections;
using System.IO;


public class Item {
    static string[] NameField ={ "carrot", "wood" };
    public string Name;
    public int Id;
    public int stackSize;
    public ItemType type;
    public static ItemType matchItem(int id) {
        switch (id) {
            case 0:
                return ItemType.CARROT;
            case 1:
                return ItemType.WOOD;
            default:
                return ItemType.NONE;
        }
    }
    public static implicit operator bool(Item value) {
        return value != null ? true : false;
    }
    public Item(int id) {
        Id = id;
        Name = NameField[id];
        type = matchItem(id);
    }
}
