using System.Collections.Generic;

public static class DictPool<T,U> {
    static Stack<Dictionary<T,U>> stack = new Stack<Dictionary<T,U>>();
    public static Dictionary<T,U> Get() {
        if (stack.Count > 0) {
            return stack.Pop();
        }
        return new Dictionary<T, U>();
    }
    public static void Add(Dictionary<T,U> list) {
        list.Clear();
        stack.Push(list);
    }
}
