using UnityEngine.UI;
using System.Collections;
using System.IO;


public class Item {
    static string[] NameField ={ "carrot", "wood" };
    static int[] StackSize = { 64, 64 };
    public string Name;
    public int Id;
    public int stackSize;
    public ItemFlag flag;
    public static ItemFlag matchItem(int id) {
        switch (id) {
            case 0:
                return ItemFlag.CARROT;
            case 1:
                return ItemFlag.WOOD;
            default:
                return ItemFlag.NONE;
        }
    }
    public static implicit operator bool(Item value) {
        return value != null ? true : false;
    }
    public Item(int id) {
        Id = id;
        Name = NameField[id];
        flag = matchItem(id);
    }
}
