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
    public static TriDirection Next(this TriDirection direction,bool inverted) {
        switch (direction) {
            case TriDirection.VERT:
                if (inverted) return TriDirection.RIGHT;
                else return TriDirection.LEFT;
            case TriDirection.RIGHT:
                if (inverted) return TriDirection.LEFT;
                else return TriDirection.VERT;
            default:
                if (inverted) return TriDirection.VERT;
                else return TriDirection.RIGHT;
        }
    }

    public static TriDirection Previous(this TriDirection direction, bool inverted) {
        switch (direction) {
            case TriDirection.VERT:
                if (inverted) return TriDirection.LEFT;
                else return TriDirection.RIGHT;
            case TriDirection.RIGHT:
                if (inverted) return TriDirection.VERT;
                else return TriDirection.LEFT;
            default:
                if (inverted) return TriDirection.RIGHT;
                else return TriDirection.VERT;
        }
    }
}