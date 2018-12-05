using UnityEngine;
using System.Collections;
using System.IO;
public class Person : Unit {
    Inn home;
    public void Awake() {
        type = UnitType.PERSON;
    }
    public void migrate (Inn target){
        home = target;
    }
    public void GoHome() {
        CancelAllAct();
        AddCommand(new Command(CommandType.MOVE, home));
    }
    public void Save(BinaryWriter writer) {
        if (home)
            home.location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
    }
    public static new Person Load(BinaryReader reader) {
        Person ret= Instantiate((Person)Isleland.Instance.unitPrefabs[(int)UnitType.PERSON]);
        Inn home = (Inn)TriGrid.Instance.GetCell(TriCoordinates.Load(reader)).Building;
        ret.migrate(home);
        return ret;
    }

}
