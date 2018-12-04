using UnityEngine;
using System.Collections;

public class Person : Unit {
    Inn home;
    public new UnitType type = UnitType.PERSON;
    public void migrate (Inn target){
        home = target;
    }
    public void GoHome() {
        
    }

}
