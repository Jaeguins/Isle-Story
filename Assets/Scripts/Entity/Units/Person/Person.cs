using UnityEngine;
using System.Collections;
using System.IO;
public class Person : Unit {
    public Inn Home {
        get {
            return home;
        }
        set {
            value.AddLiver(this);
            home = value;
        }
    }
    public Inn home;
    public Company Company {
        get {
            return company;
        }
        set {
            value.AddOfficer(this);
            company = value;
        }
    }
    public Company company;
    public Entity work;
    public override void Start() {
        base.Start();
        Inventory.Size = 25;
    }

    public override void ChangeHomeInternal() {
        ((ChangeHomeCommand)nowWork).target.AddLiver(this);
        Home = ((ChangeHomeCommand)nowWork).target;
        Debug.Log("Change Home");
    }

    public override void GoHome() {
        CancelAllAct();
        if (Home) {
            AddCommand(new MoveCommand(Home.EntranceLocation));
            AddCommand(new GetInCommand(Home));
        }
        else {
            AddCommand(new GetInCommand(TriIsleland.Instance.entities.GetCamp()));
        }
    }

    public override void GoJob() {
        CancelAllAct();
        if (Company) {
            AddCommand(new MoveCommand(Company.EntranceLocation));
            AddCommand(new GetInCommand(Company));
        }
        else GoHome();

    }

    public override void GoWork() {
        CancelAllAct();
        AddCommand(new MoveCommand(Company.EntranceLocation));
        AddCommand(new GetInCommand(Company));
    }


    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        if (Home)
            Home.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
        if (Company)
            Company.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
        if (work)
            work.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
    }

    public static new Person Load(BinaryReader reader) {
        Person ret = Instantiate((Person)TriIsleland.Instance.unitPrefabs[(int)UnitType.PERSON]);
        TriCell tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.Home = (Inn)tLoc.Entity;
        }
        tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.Company = (Company)tLoc.Entity;
        }
        tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.work = tLoc.Entity;
        }
        return ret;
    }
    public override void Build() {
        BuildCommand c = (BuildCommand)nowWork;
        Building t = TriMapEditor.Instance.CreateBuilding(c.dir, c.location, (Building)c.target);
        AddCommand(new ChangeWorkCommand(t));
        t.AddInsider(this);
    }
    public override void ChangeJobInternal() {
        Company = ((ChangeJobCommand)nowWork).target;
        Debug.Log("Change Job");
    }
    public override void ChangeWorkInternal() {
        ChangeWorkCommand k = ((ChangeWorkCommand)nowWork);
        if (work)
            ((Building)work).Workers.Remove(this);
        work = k.target;
        if (work)
            ((Building)work).Workers.Add(this);
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
    public override void Tick() {
        Debug.Log(ToString() + GetInstanceID() + " : Start");
    }
}