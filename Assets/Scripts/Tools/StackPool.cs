using System.Collections.Generic;

public static class StackPool<T> {
    static Stack<Stack<T>> stack = new Stack<Stack<T>>();
    public static Stack<T> Get() {
        if (stack.Count > 0) {
            return stack.Pop();
        }
        return new Stack<T>();
    }
    public static void Add(Stack<T> list) {
        list.Clear();
        stack.Push(list);
    }
}
