public enum TriDirection {
    VERT,LEFT,RIGHT
}
public static class TriDirectionExtensions {

    public static TriDirection Next(this TriDirection direction) {
        return direction == TriDirection.RIGHT ? TriDirection.VERT : (direction + 1);
    }

    public static TriDirection Previous(this TriDirection direction) {
        return direction == TriDirection.VERT ? TriDirection.RIGHT : (direction - 1);
    }
    public static TriDirection Previous2(this TriDirection direction) {
        return direction.Next();
    }

    public static TriDirection Next2(this TriDirection direction) {
        return direction.Previous();
    }
}
