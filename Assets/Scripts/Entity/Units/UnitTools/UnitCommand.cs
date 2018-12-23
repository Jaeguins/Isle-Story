using System.IO;
public enum CommandType {
    MOVE, GETIN, GETOUT, CHANGEHOME,GOHOME, CHANGEJOB,GOJOB, CHANGEWORK,GOWORK, BUILD
}
public class Command {
    public CommandType type;

    public virtual void Save(BinaryWriter writer) {
        writer.Write((int)type);
    }

    public static Command Load(BinaryReader reader) {
        TriGrid grid = TriGrid.Instance;
        CommandType type = (CommandType)reader.ReadInt32();
        Command t = new Command();
        switch (type) {
            case CommandType.MOVE:
                t = MoveCommand.Load(reader);
                break;
            case CommandType.GETIN:
                t = GetInCommand.Load(reader);
                break;
            case CommandType.GETOUT:
                t = GetOutCommand.Load(reader);
                break;
            case CommandType.CHANGEHOME:
                t = ChangeHomeCommand.Load(reader);
                break;
            case CommandType.CHANGEJOB:
                t = ChangeJobCommand.Load(reader);
                break;
            case CommandType.CHANGEWORK:
                t = ChangeWorkCommand.Load(reader);
                break;
            case CommandType.BUILD:
                t = BuildCommand.Load(reader);
                break;
        }
        t.type = type;
        return t;
    }
}

public class MoveCommand : Command {
    public TriCell location;

    public MoveCommand(TriCell location) {
        type = CommandType.MOVE;
        this.location = location;
    }

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        location.coordinates.Save(writer);
    }

    public static new MoveCommand Load(BinaryReader reader) {
        return new MoveCommand(TriGrid.Instance.GetCell(TriCoordinates.Load(reader)));
    }

}

public class GetInCommand : Command {
    public Building target;
    public GetInCommand(Building target) {
        this.target = target;
        type = CommandType.GETIN;
    }

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        target.Location.coordinates.Save(writer);
    }

    public static new GetInCommand Load(BinaryReader reader) {
        return new GetInCommand((Building)TriGrid.Instance.GetCell(TriCoordinates.Load(reader)).Entity);
    }
}

public class ChangeHomeCommand : Command {
    public Inn target;
    public ChangeHomeCommand(Inn target) {
        this.target = target;
        type = CommandType.CHANGEHOME;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        target.Location.coordinates.Save(writer);
    }

    public static new ChangeHomeCommand Load(BinaryReader reader) {
        return new ChangeHomeCommand((Inn)TriGrid.Instance.GetCell(TriCoordinates.Load(reader)).Entity);
    }
}

public class GetOutCommand : Command {
    public GetOutCommand() {
        type = CommandType.GETOUT;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public static new GetOutCommand Load(BinaryReader reader) {
        return new GetOutCommand();
    }
}

public class ChangeJobCommand : Command {
    public Company target;
    public ChangeJobCommand(Company target) {
        this.target = target;
        type = CommandType.CHANGEJOB;
    }

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        target.Location.coordinates.Save(writer);
    }

    public static new ChangeJobCommand Load(BinaryReader reader) {
        return new ChangeJobCommand((Company)TriGrid.Instance.GetCell(TriCoordinates.Load(reader)).Entity);
    }
}

public class ChangeWorkCommand : Command {
    public Entity target;
    public ChangeWorkCommand(Entity target) {
        this.target = target;
        type = CommandType.CHANGEWORK;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        target.Location.coordinates.Save(writer);
    }

    public static new ChangeWorkCommand Load(BinaryReader reader) {
        return new ChangeWorkCommand(TriGrid.Instance.GetCell(TriCoordinates.Load(reader)).Entity);
    }
}

public class BuildCommand : Command {
    public TriCell location;
    public TriDirection dir;
    public Entity target;
    public BuildCommand(TriCell location, TriDirection dir, Entity target) {
        this.location = location;
        this.dir = dir;
        this.target = target;
        type = CommandType.BUILD;
    }
    public BuildCommand(Entity target) {
        this.target = target;
        type = CommandType.BUILD;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        location.coordinates.Save(writer);
        writer.Write((int)dir);
        BuildingType type = ((Building)target).type;
        writer.Write((int)type);
        switch (type) {//TODO BuildCommand list
            case BuildingType.INN:
                writer.Write((int)((Inn)target).subType);
                break;
            default:
                break;
        }
    }

    public static new BuildCommand Load(BinaryReader reader) {
        TriGrid instance = TriGrid.Instance;
        TriCell tCell = instance.GetCell(TriCoordinates.Load(reader));
        TriDirection tDir = (TriDirection)reader.ReadInt32();
        Entity prefab;
        switch (reader.ReadInt32()) {
            case (int)BuildingType.INN:
                prefab = TriIsleland.Instance.innPrefabs[reader.ReadInt32()];
                break;
            default:
                prefab = null;
                break;
        }
        return new BuildCommand(tCell,tDir, prefab);
    }
}

public class GoHomeCommand:Command{
    public GoHomeCommand() {
        type = CommandType.GOHOME;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public static new GoHomeCommand Load(BinaryReader reader) {
        return new GoHomeCommand();
    }
}

public class GoWorkCommand : Command {
    public GoWorkCommand() {
        type = CommandType.GOWORK;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public static new GoWorkCommand Load(BinaryReader reader) {
        return new GoWorkCommand();
    }
}

public class GoJobCommand : Command {
    public GoJobCommand() {
        type = CommandType.GOJOB;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public static new GoJobCommand Load(BinaryReader reader) {
        return new GoJobCommand();
    }
}
