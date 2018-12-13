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
    public override void Migrate (){
        ((Inn)nowWork.target).addPerson(this);
        home = (Inn)nowWork.target;
    }
    public void GoHome() {
        CancelAllAct();
        AddCommand(new Command(CommandType.MOVE, TriDirection.VERT,home));
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
            ret.AddCommand(new Command(CommandType.MIGRATE,TriDirection.VERT, (Inn)homeLoc.Entity));
        }
        return ret;
    }
    public override void Build(){
        Building t = GameUI.Instance.mapEditor.CreateBuilding(nowWork.dir,nowWork.targetLocation,(Building)nowWork.target);
        t.Location = nowWork.targetLocation;
        TriIsleland.Instance.entities.AddBuilding(t);
        AddCommand(new Command(CommandType.CHANGEWORK, TriDirection.VERT,t));
    }
    public override void ChangeJob() {
        company = (Building)nowWork.target;
    }
    public override void ChangeWork() {
        work = nowWork.target;
    }
    public override void BindOptions(EntityMenu menu) {
        base.BindOptions(menu);
        menu.BindButton(5, "build", menu.switchBuildMenu);
    }
}
