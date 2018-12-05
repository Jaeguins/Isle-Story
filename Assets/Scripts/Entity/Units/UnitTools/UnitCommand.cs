using System.IO;
public enum CommandType {
    MOVE,MIGRATE,GETIN,GETOUT
}

public struct Command {
    public CommandType type;
    public Building target;
    public TriCell targetLocation;
    public bool flag;












    public Command(CommandType type,Building target) {
        this.type = type;
        this.target = target;
        this.targetLocation = target.Location;
        flag = true;
    }
    public Command(CommandType type, TriCell targetL) {
        this.type = type;
        this.target = null;
        this.targetLocation = targetL;
        flag = false;
    }
    public void Save(BinaryWriter writer) {
        writer.Write((int)type);
        writer.Write(flag);
        if (flag) {
            target.Location.coordinates.Save(writer);
        }
        else {
            targetLocation.coordinates.Save(writer);
        }
    }
    public static Command Load(BinaryReader reader) {
        TriGrid grid = TriGrid.Instance;
        CommandType type = (CommandType)reader.ReadInt32();
        bool flag = reader.ReadBoolean();
        if (flag) {
            return new Command(type,grid.GetCell(TriCoordinates.Load(reader)).Building);
        }
        else {
            return new Command(type, grid.GetCell(TriCoordinates.Load(reader)));
        }
    }
}