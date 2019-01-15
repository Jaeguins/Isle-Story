using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public interface ProductionSelectable {
    int GetCurrent();
    List<ItemResource> GetItems();
    void SetProduction(int target);
}

