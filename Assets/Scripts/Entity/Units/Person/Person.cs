using UnityEngine;
using System.Collections;
using System.IO;
public class Person : Unit {

    public Inn Home {
        get {
            return home;
        }
        set {
            if (home)
                home.Livers.Remove(this);
            if (value)
                value.Livers.Add(this);
            home = value;
        }
    }
    public Inn home;

    public Company Company {
        get {
            return company;
        }
        set {
            if (company)
                company.Officers.Remove(this);
            if (value)
                value.Officers.Add(this);
            company = value;
        }
    }
    public Company company;

    public Statics Work {
        get {
            return work;
        }
        set {
            if (work)
                work.Workers.Remove(this);
            if (value)
                value.Workers.Add(this);
            work = value;
        }
    }
    public Statics work;

    public bool needAttend = false;
    public bool needGoWork = false;
    public override void Start() {
        base.Start();
    }

    public override void ChangeHomeInternal() {
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
        if (!Company) return;
        CancelAllAct();
        if (Company) {
            AddCommand(new MoveCommand(Company.EntranceLocation));
            AddCommand(new GetInCommand(Company));
        }
    }

    public override void GoWork() {
        if (!Work) return;
        CancelAllAct();
        AddCommand(new MoveCommand(Work.EntranceLocation));
        AddCommand(new GetInCommand(Work));
    }


    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        if (Home)
            Home.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
        if (Company)
            Company.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
        if (Work)
            Work.Location.coordinates.Save(writer);
        else new TriCoordinates(-1, -1).Save(writer);
    }

    public static new Person Load(BinaryReader reader) {
        Person ret = Instantiate((Person)TriIsleland.GetUnitPrefabs((int)UnitType.PERSON, 0));
        TriCell tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.Home = (Inn)tLoc.Statics;
        }
        tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.Company = (Company)tLoc.Statics;
        }
        tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc) {
            ret.Work = tLoc.Statics;
        }
        return ret;
    }
    public override void Build() {
        BuildCommand c = (BuildCommand)nowWork;
        Building t = TriMapEditor.Instance.CreateBuilding(c.dir, c.location, (Building)c.target);
        AddCommand(new ChangeWorkCommand(t));
        AddCommand(new GetInCommand(t));
    }
    public override void ChangeJobInternal() {
        Company = ((ChangeJobCommand)nowWork).target;
        Debug.Log("Change Job");
    }
    public override void ChangeWorkInternal() {
        Work = ((ChangeWorkCommand)nowWork).target;
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
        base.Tick();
    }
}