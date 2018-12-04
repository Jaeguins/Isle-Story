public enum CommandType {
    MOVE,MIGRATE,GETIN,GETOUT
}

public struct Command {
    public CommandType type;
    public Entity target;
    public TriCell targetLocation;
    public Command(CommandType type,Entity target) {
        this.type = type;
        this.target = target;
        this.targetLocation = target.location;
    }
    public Command(CommandType type, TriCell targetL) {
        this.type = type;
        this.target = null;
        this.targetLocation = targetL;
    }
}