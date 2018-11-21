public enum TriDirection {
    VERT,LEFT,RIGHT
}
public static class TriDirectionExtensions {

    public static TriDirection Opposite(this TriDirection direction) {
        return  direction;
    }
    public static TriDirection Next(this TriDirection direction,bool inverted) {
        return direction == TriDirection.RIGHT ? TriDirection.VERT : (direction + 1);
    }

    public static TriDirection Previous(this TriDirection direction, bool inverted) {
        return direction == TriDirection.VERT ? TriDirection.RIGHT : (direction - 1);
    }
}