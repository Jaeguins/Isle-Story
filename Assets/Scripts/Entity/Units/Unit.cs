using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
public enum UnitType {
    PERSON
}
public enum ActionStatus {
    Idle = 0, Walk = 1, Work = 2
}

public class Unit : Entity {
    public SkinnedMeshRenderer mesh;
    public override void Awake() {
        base.Awake();
        EntityType = EntityType.Unit;
    }
    public Statics Building {
        get {
            return building;
        }
        set {
            if (building)
                building.Insider.Remove(this);
            if (value)
                value.Insider.Add(this);
            building = value;
        }
    }
    public Statics building;
    public UnitType type;
    public Coroutine nowRoutine;
    List<TriCell> pathToTravel;
    const float travelSpeed = 1f;
    public bool acting = false;
    public Queue<Command> commandQueue = new Queue<Command>();
    public Command nowWork {
        get {
            return commandQueue.Count==0?null:commandQueue.Peek();
        }
    }
    float orientation;
    public float Orientation {
        get {
            return orientation;
        }
        set {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }
    public void SetVisible(bool val) {
        //mesh.enabled = val;
        col.enabled = val;
    }
    public static bool IsValidDestination(TriCell cell) {
        return !cell.IsUnderwater;
    }
    public void AddCommand(Command c) {//Warn : Don't use AddCommand() in AddCommand()!
        switch (c.type) {
            case CommandType.GETIN:
                Statics t = ((GetInCommand)c).target;
                if (Location != t.Location) {
                    if (Location != t.EntranceLocation)
                        commandQueue.Enqueue(new MoveCommand(t.EntranceLocation));
                    commandQueue.Enqueue(new MoveCommand(t.Location, false));
                }
                break;
            case CommandType.BUILD:
                BuildCommand k = (BuildCommand)c;
                commandQueue.Enqueue(new GetOutCommand());
                commandQueue.Enqueue(new MoveCommand(Building.EntranceLocation));
                commandQueue.Enqueue(new MoveCommand(k.location));
                break;
            case CommandType.MOVE:
                if (((MoveCommand)c).flag)
                    commandQueue.Enqueue(new GetOutCommand());
                break;
            default:
                break;
        }
        commandQueue.Enqueue(c);
        switch (c.type) {
            case CommandType.GETOUT:
                commandQueue.Enqueue(new MoveCommand(Building.EntranceLocation, false));
                break;
        }
    }
    private void OnEnable() {
        StartCoroutine(Act());
    }
    private void OnDisable() {
        StopAllCoroutines();
    }


    public IEnumerator<Coroutine> FindPathAndMove(TriCell target, bool entityCheck) {
        TriGrid inst = TriGrid.Instance;
        if (target && IsValidDestination(target)) {

            inst.FindPath(Location, target, entityCheck);
            if (inst.HasPath) {
                pathToTravel = inst.GetPath();
                CancelNowAct();
                nowRoutine = StartCoroutine(TravelPath());
                yield return nowRoutine;
                inst.ClearPath();
            }
        }
        else {
            inst.ClearPath();
        }
        yield return null;
    }

    public virtual IEnumerator GetIn() {
        Debug.Log(nowWork.ToString() + " from " + Location + " left order : " + commandQueue.Count);
        yield return StartCoroutine(GetIn(((GetInCommand)nowWork).target));

    }
    public IEnumerator GetIn(Statics target) {
        Location = target.Location;
        Building = target;
        SetVisible(false);
        acting = false;
        yield return null;
    }
    public IEnumerator GetOut() {
        Debug.Log(nowWork.ToString() + " from " + Location + " left order : " + commandQueue.Count);
        if (!Building) {
            Debug.LogWarning(ToString() + " : not in building");
            acting = false;
        }
        else {
            acting = true;
            Building = null;
            SetVisible(true);
            acting = false;
        }
        yield return null;
    }
    public IEnumerator Move(bool entityCheck) {
        Debug.Log(nowWork.ToString() + " from " + Location + " left order : " + commandQueue.Count);
        yield return StartCoroutine(FindPathAndMove(((MoveCommand)nowWork).location, entityCheck));
    }
    public virtual IEnumerator Build() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    public virtual IEnumerator ChangeJobInternal() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    public virtual IEnumerator ChangeWorkInternal() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    public virtual IEnumerator ChangeHomeInternal() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    public virtual IEnumerator GoJob() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    public virtual IEnumerator GoWork() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    public virtual IEnumerator GoHome() {
        Debug.Log("unexpected Order");
        acting = false;
        yield return null;
    }
    IEnumerator Act() {
        while (gameObject) {
            if (commandQueue.Count != 0) {
                acting = true;
                switch (nowWork.type) {
                    case CommandType.MOVE:
                        yield return StartCoroutine(Move(((MoveCommand)nowWork).flag));
                        break;
                    case CommandType.GETIN:
                        yield return StartCoroutine(GetIn());
                        break;
                    case CommandType.GETOUT:
                        yield return StartCoroutine(GetOut());
                        break;
                    case CommandType.BUILD:
                        yield return StartCoroutine(Build());
                        break;
                    case CommandType.CHANGEJOB:
                        yield return StartCoroutine(ChangeJobInternal());
                        break;
                    case CommandType.CHANGEWORK:
                        yield return StartCoroutine(ChangeWorkInternal());
                        break;
                    case CommandType.CHANGEHOME:
                        yield return StartCoroutine(ChangeHomeInternal());
                        break;
                    case CommandType.GOJOB:
                        yield return StartCoroutine(GoJob());
                        break;
                    case CommandType.GOWORK:
                        yield return StartCoroutine(GoWork());
                        break;
                    case CommandType.GOHOME:
                        yield return StartCoroutine(GoHome());
                        break;
                }
                commandQueue.Dequeue();
            }
            yield return new WaitUntil(() => !acting);
        }
    }

    public IEnumerator TravelPath() {
        animator.SetInteger("Status", (int)ActionStatus.Walk);
        Vector3 a, b, c = transform.localPosition;
        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++) {
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + pathToTravel[i].Position) * 0.5f;
            for (; t < 1f; t += Time.deltaTime * travelSpeed) {
                Vector3 p = Bezier.GetPoint(a, b, c, t);
                p.y = b.y;
                Location = pathToTravel[i - 1];
                transform.localPosition = p;
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            t -= 1f;
        }
        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;
        for (; t < 1f; t += Time.deltaTime * travelSpeed) {
            Vector3 p = Bezier.GetPoint(a, b, c, t);
            p.y = b.y;
            Location = pathToTravel[pathToTravel.Count - 1];
            transform.localPosition = p;
            Vector3 d = Bezier.GetPoint(a, b, c, t);
            d.y = 0f;
            //transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }
        animator.SetInteger("Status", (int)ActionStatus.Idle);
        acting = false;
    }

    public void CancelAllAct() {
        commandQueue.Clear();
        CancelNowAct();
    }

    public void CancelNowAct() {
        if (nowRoutine != null) {
            StopCoroutine(nowRoutine);
        }
        acting = false;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)type);
        writer.Write(orientation);
    }
    public static Unit Load(BinaryReader reader) {
        UnitType type = (UnitType)reader.ReadInt32();
        float orientation = reader.ReadSingle();
        List<Command> tCommand = ListPool<Command>.Get();
        Unit ret = null;
        switch (type) {
            case UnitType.PERSON:
                ret = Human.Load(reader);
                break;
        }
        if (ret) {
            ret.orientation = orientation;
            ret.type = type;
            /*
            if (acting)
                foreach (Command i in tCommand) {
                    ret.AddCommand(i);
                }
            */
        }
        tCommand.Clear();
        ListPool<Command>.Add(tCommand);

        return ret;
    }
}
