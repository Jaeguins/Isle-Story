using UnityEngine;
using System.Collections.Generic;

public class Inn : Building {
    List<Person> livers;
    protected int capacity;
    void setLiver(Person person) {
        if (livers.Count < 4) {
            livers.Add(person);
        }
    }

}
