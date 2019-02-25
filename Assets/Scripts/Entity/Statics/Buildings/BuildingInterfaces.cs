using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public interface ProductionSelectable {
    int GetCurrent();
    List<Production> GetItems();
    void SetProduction(int target);
}
[Serializable]
public struct TotalProduction {
    public ResourceType type;
    public int prod, max;
}
[Serializable]
public struct Production {
    public ItemType type;
    public int prod,max;
}
public interface Commandable {
    bool HasCommandReceiver();
    Unit GetCommandReceiver();
}
public interface Buildable :Commandable{
    int GetTech();
}
