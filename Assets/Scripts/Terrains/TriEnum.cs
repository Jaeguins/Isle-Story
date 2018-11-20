public enum TriDirection {
    VERT,RIGHT,LEFT
}
public static class TriDirectionExtensions {

    public static TriDirection Opposite(this TriDirection direction) {
        switch (direction) {
            case TriDirection.VERT:
                return TriDirection.VERT;
            case TriDirection.RIGHT:
                return TriDirection.LEFT;
            case TriDirection.LEFT:
                return TriDirection.RIGHT;
            default:
                return  TriDirection.VERT;
        }
    }
}