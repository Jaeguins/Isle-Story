using UnityEngine;
using System.Collections;
using System.IO;
public class Person : Unit {
    public bool gender;
    public Inn home;
    public Building company;
    public Entity work;
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
    public new void Build(){
        Construction t = Instantiate(TriIsleland.Instance.constructionPrefab);
        t.Location = nowWork.targetLocation;
        t.target = (Building)nowWork.target;
        TriIsleland.Instance.entities.AddBuilding(t);
        AddCommand(new Command(CommandType.CHANGEWORK, t));
    }
    public new void ChangeJob() {
        company = (Building)nowWork.target;
    }
    public new void ChangeWork() {
        work = nowWork.target;
    }
    public new void Migrate() {
        home = (Inn)nowWork.target;
    }
}
