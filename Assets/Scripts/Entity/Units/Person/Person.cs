using UnityEngine;
using System.Collections;
using System.IO;
public class Person : Unit {
    public Inn home;
    public Building company;
    public Building building;
    public Entity work;
    
    public override void ChangeHomeInternal (){
        ((ChangeHomeCommand)nowWork).target.AddPerson(this);
        home = ((ChangeHomeCommand)nowWork).target;
        Debug.Log("Change Home");
    }

    public override void GoHome() {
        CancelAllAct();
        if (home) {
            AddCommand(new MoveCommand(home.EntranceLocation));
            AddCommand(new GetInCommand(home));
        }
        else {
            AddCommand(new GetInCommand(TriIsleland.Instance.entities.GetCamp()));
        }
    }
    
    public override void GoJob() {
        CancelAllAct();
        if (company) {
            AddCommand(new MoveCommand(company.EntranceLocation));
            AddCommand(new GetInCommand(company));
        }
        else GoHome();
        
    }

    public override void GoWork() {
        CancelAllAct();
        AddCommand(new MoveCommand(company.EntranceLocation));
        AddCommand(new GetInCommand(company));
    }


    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        if (home)
            home.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
        if (company)
            company.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
        if (work)
            work.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
    }
    
    public static new Person Load(BinaryReader reader) {
        Person ret= Instantiate((Person)TriIsleland.Instance.unitPrefabs[(int)UnitType.PERSON]);
        TriCell tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.home = (Inn)tLoc.Entity;
        }
        tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.company= (Company)tLoc.Entity;
        }
        tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.work= tLoc.Entity;
        }
        return ret;
    }
    public override void Build(){
        BuildCommand c = (BuildCommand)nowWork;
        Building t = TriMapEditor.Instance.CreateBuilding(c.dir, c.location, (Building)c.target);
        AddCommand(new ChangeWorkCommand(t));
        t.AddWorker(this);
    }
    public override void ChangeJobInternal() {
        company = ((ChangeJobCommand)nowWork).target;
        Debug.Log("Change Job");
    }
    public override void ChangeWorkInternal() {
        work = ((ChangeWorkCommand)nowWork).target;
        Debug.Log("Change Work");
    }
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        menu.BindButton(5, "build", BindingBuildingMenu);
        menu.BindButton(6, "change\nhome", ChangeHome);
        menu.BindButton(7, "change\ncompany", ChangeJob);
        menu.BindButton(8, "change\nwork", ChangeWork);
    }
    public void BindingBuildingMenu() {
        BuildingMenu.Instance.Bind(this);
    }
    public void ChangeHome() {
        Selector.Instance.RequestTarget(this, new ChangeHomeCommand(null));
    }
    public void ChangeJob() {
        Selector.Instance.RequestTarget(this, new ChangeJobCommand(null));
    }
    public void ChangeWork() {
        Selector.Instance.RequestTarget(this, new ChangeWorkCommand(null));
    }
    
}