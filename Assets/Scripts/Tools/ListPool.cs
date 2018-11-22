using System.Collections.Generic;

public static class ListPool<T> {
    static Stack<List<T>> stack = new Stack<List<T>>();
}
