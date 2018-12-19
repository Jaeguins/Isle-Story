using System.IO;
public enum CommandType {
    MOVE, GETIN, GETOUT, CHANGEHOME, CHANGEJOB, CHANGEWORK, BUILD
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
        target.Location.coordinates.Save(writer);
    }

    public static new BuildCommand Load(BinaryReader reader) {
        TriGrid instance = TriGrid.Instance;
        return new BuildCommand(instance.GetCell(TriCoordinates.Load(reader)),(TriDirection)reader.ReadInt32(), instance.GetCell(TriCoordinates.Load(reader)).Entity);
    }
}

public struct Command_ {
    public CommandType type;
    public TriDirection dir;
    public Entity target;
    public TriCell targetLocation;
    public int flag;

    public override string ToString() {
        return type.ToString() + '\n' + dir + '\n';
    }

    public Command_(CommandType type) {
        dir = TriDirection.VERT;
        this.type = type;
        target = null;
        targetLocation = null;
        flag = 0;
    }
    public Command_(CommandType type, TriDirection dir, Entity target) {
        this.dir = dir;
        this.type = type;
        this.target = target;
        this.targetLocation = target.Location;
        flag = 1;
    }
    public Command_(CommandType type, TriCell targetL) {
        dir = TriDirection.VERT;
        this.type = type;
        this.target = null;
        this.targetLocation = targetL;
        flag = 2;
    }
    public Command_(CommandType type, TriDirection dir, Entity target, TriCell targetL) {
        this.dir = dir;
        this.type = type;
        this.target = target;
        this.targetLocation = targetL;
        flag = 3;
    }
    public void Save(BinaryWriter writer) {
        writer.Write((int)type);
        writer.Write(flag);
        if (flag % 2 == 1) {
            target.Location.coordinates.Save(writer);
        }
        else if (flag >= 2) {
            targetLocation.coordinates.Save(writer);
        }
    }
    public static Command_ Load(BinaryReader reader) {
        TriGrid grid = TriGrid.Instance;
        CommandType type = (CommandType)reader.ReadInt32();
        int flag = reader.ReadInt32();
        Command_ t = new Command_(type);
        if (flag % 2 == 1) {
            t.target = grid.GetCell(TriCoordinates.Load(reader)).Entity;
        }
        else if (flag >= 2) {
            t.targetLocation = grid.GetCell(TriCoordinates.Load(reader));
        }
        t.flag = flag;
        return t;
    }
}