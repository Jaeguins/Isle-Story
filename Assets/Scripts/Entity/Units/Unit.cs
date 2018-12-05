using UnityEngine;
using System.Collections.Generic;
using System.IO;
public enum UnitType {
    PERSON
}
public class Unit : Entity {
    public UnitType type;
    public Coroutine nowRoutine;
    public Animator animator;
    List<TriCell> pathToTravel;
    const float travelSpeed = 1f;
    public bool acting = false;
    public Queue<Command> commandQueue;
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

    public static bool IsValidDestination(TriCell cell) {
        return !cell.IsUnderwater && !cell.Entity;
    }
    public void AddCommand(Command c) {
        commandQueue.Enqueue(c);
    }
    private void Awake() {
        commandQueue = new Queue<Command>();
        StartCoroutine(Act());
    }
    public IEnumerator<Coroutine> FindPathAndMove(TriCell target) {
        TriGrid inst = TriGrid.Instance;
        if (target && Unit.IsValidDestination(target)) {
            
            inst.FindPath(Location, target);
            if (inst.HasPath) {
                pathToTravel = inst.GetPath();
                CancelNowAct();
                //StopCoroutine(TravelPath());
                nowRoutine= StartCoroutine(TravelPath());
                yield return nowRoutine;
                inst.ClearPath();
            }
        }
        else {
            inst.ClearPath();
        }
        yield return null;
    }

    IEnumerator<WaitUntil> Act() {
        while (gameObject) {
            if (commandQueue.Count != 0) {
                Command tC = commandQueue.Dequeue();
                switch (tC.type) {
                    case CommandType.MOVE:
                        
                        StartCoroutine(FindPathAndMove(tC.targetLocation));
                        Debug.Log(commandQueue.Count);
                        break;
                }
                
            }

            yield return new WaitUntil(()=>!acting);
        }
    }

    public IEnumerator<WaitForEndOfFrame> TravelPath() {
        animator.SetBool("walking", true);
        acting = true;
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
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f;
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }
        animator.SetBool("walking", false);
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

    public new void Save(BinaryWriter writer,int index) {
        base.Save(writer,index);
        writer.Write((int)type);
        writer.Write(orientation);
    }
    public static Unit Load(BinaryReader reader) {
        TriCoordinates coord = TriCoordinates.Load(reader);
        UnitType type = (UnitType)reader.ReadInt32();
        float orientation = reader.ReadSingle();
        Unit ret = null;
        switch (type) {
            case UnitType.PERSON:
                ret = Person.Load(reader);
                break;

        }
        ret.orientation = orientation;
        ret.type = type;
        return ret;
    }
}
