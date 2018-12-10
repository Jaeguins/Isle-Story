using UnityEngine;
using System.Collections;
using System.IO;
public class Person : Unit {
    
    Inn home;
    public void Start() {
        type = UnitType.PERSON;
        UIName = "person";
    }
    public void migrate (Inn target){
        target.addPerson(this);
        home = target;
    }
    public void GoHome() {
        CancelAllAct();
        AddCommand(new Command(CommandType.MOVE, home));
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
        if (home)
            home.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
    }
    public static new Person Load(BinaryReader reader) {
        Person ret= Instantiate((Person)TriIsleland.Instance.unitPrefabs[(int)UnitType.PERSON]);
        TriCell homeLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (homeLoc) {
            Inn home = (Inn)(homeLoc.Building);
            ret.migrate(home);
        }
        return ret;
    }

}
