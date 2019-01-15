using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public interface ProductionSelectable {
    int GetCurrent();
    List<ItemResource> GetItems();
    void SetProduction(int target);
}
public interface Commandable {
    bool HasCommandReceiver();
    Unit GetCommandReceiver();
}
public interface Buildable :Commandable{
    int GetTech();
}
