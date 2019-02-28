using UnityEngine;
using System.Collections;
using System.IO;
public enum Tool {
    //Hex flag for WeaponType,ToolType
    None = 0x00,
    Axe = 0x11,
    Hoe = 0x02,
    Forks = 0x03,
    Hummer = 0x24,
    Pick = 0x05,
    Saw = 0x36,
    Sword = 0x40,
    Spear = 0x50
}
public class Human : Unit {
    public Statics RoutineTarget {
        get {
            return routineTarget;
        }
        set {
            string[] data = { this.ToString(), " changed target", value.ToString() };
            if (routineTarget != value) Debug.Log(string.Concat(data));
            routineTarget = value;
        }
    }
    public Statics routineTarget;
    public Inn Home {
        get {
            return home;
        }
        set {
            Debug.Log(ToString() + " trying to change home from " + home + " to " + value);
            if (home)
                home.Livers.Remove(this);
            else if ((TriIsland.GetCamp() as Hall).Homeless.Contains(this))
                (TriIsland.GetCamp() as Hall).Homeless.Remove(this);
            if (value)
                value.Livers.Add(this);
            else
                TriIsland.Instance.entities.camp.Homeless.Add(this);
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

    public override IEnumerator ChangeHomeInternal() {
        Home = ((ChangeHomeCommand)nowWork).target;
        Debug.Log("Change Home : " + Home);
        acting = false;
        yield return null;
    }

    public override IEnumerator DestroyTarget() {
        DestroyCommand work = nowWork as DestroyCommand;
        (work.target as Statics).DeconstructionStart(this);
        AddCommand(new ChangeWorkCommand(work.target));
        Debug.Log(work.target.ToString() + " destroing start");
        acting = false;
        ActResult = true;
        yield return null;
    }

    public override IEnumerator GoHome() {
        CancelAllAct();
        if (Home) {
            AddCommand(new MoveCommand(Home.EntranceLocation, true, true));
            AddCommand(new GetInCommand(Home));
        }
        else {
            AddCommand(new MoveCommand(TriIsland.GetCamp().EntranceLocation, true, true));
            AddCommand(new GetInCommand(TriIsland.GetCamp()));
        }
        acting = false;
        ActResult = true;
        yield return null;
    }

    public override IEnumerator GoJob() {
        if (Company) {
            CancelAllAct();
            AddCommand(new MoveCommand(Company.EntranceLocation, true, true));
            AddCommand(new GetInCommand(Company));
        }
        acting = false;
        ActResult = true;
        yield return null;
    }

    public override IEnumerator GoWork() {
        if (Work) {
            CancelAllAct();
            AddCommand(new MoveCommand(Work.EntranceLocation, true, true));
            AddCommand(new GetInCommand(Work));
        }
        acting = false;
        ActResult = true;
        yield return null;
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

    public static new Human Load(BinaryReader reader) {
        Human ret = Instantiate((Human)TriIsland.GetUnitPrefabs((int)UnitType.PERSON, 0));
        TriCell tLoc = TriGrid.Instance.GetCell(TriCoordinates.Load(reader));
        if (tLoc)
            ret.Home = (Inn)tLoc.Statics;
        else (TriIsland.GetCamp() as Hall).Homeless.Add(ret);
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
    public override IEnumerator Build() {
        Debug.Log(nowWork.ToString() + " from " + Location + " left order : " + commandQueue.Count);
        BuildCommand c = (BuildCommand)nowWork;
        if (c.location != Location) {
            Debug.Log("Cannot reach");
            ActResult = false;
        }
        else {
            Building t = TriMapEditor.Instance.CreateBuilding(c.dir, c.location, (Building)c.target);
            AddCommand(new ChangeWorkCommand(t));
            AddCommand(new GetInCommand(t));
            ActResult = true;
        }
        acting = false;
        yield return null;
    }
    public override IEnumerator ChangeJobInternal() {
        Debug.Log(nowWork.ToString() + " from " + Location + " left order : " + commandQueue.Count);
        Company = ((ChangeJobCommand)nowWork).target;
        if (Company)
            Debug.Log("Change Job to " + Company);
        acting = false;
        ActResult = true;
        yield return null;
    }
    public override IEnumerator ChangeWorkInternal() {
        Work = ((ChangeWorkCommand)nowWork).target;
        if (Work)
            Debug.Log("Change Work to " + Work);
        acting = false;
        ActResult = true;
        yield return null;
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
    public override IEnumerator GetIn() {
        if ((nowWork as GetInCommand).target == work && work != null)
            animator.SetInteger("Status", (int)ActionStatus.Work);
        return base.GetIn();
    }
    public override void Tick() {
        base.Tick();
        if (Clock.IsDay()) {
            if (Work) RoutineTarget = Work;
            else if (Company) RoutineTarget = Company;
            else if (Home) RoutineTarget = Home;
            else RoutineTarget = TriIsland.GetCamp();
        }
        else {
            if (Home) RoutineTarget = Home;
            else RoutineTarget = TriIsland.GetCamp();
        }
        if (nowWork == null && !acting && commandQueue.Count == 0 && RoutineTarget && RoutineTarget != Building) {
            CancelAllAct();
            Debug.LogWarning("<color=#ff0000>act canceled</color> and moved to : " + routineTarget);
            AddCommand(new MoveCommand(RoutineTarget.EntranceLocation, true, true));
            AddCommand(new GetInCommand(RoutineTarget));
        }
    }
}