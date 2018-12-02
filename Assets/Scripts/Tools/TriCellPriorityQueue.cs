using System.Collections.Generic;

public class TriCellPriorityQueue {
    int count = 0;
    int minimum = int.MaxValue;
    public int Count {
        get {
            return count;
        }
    }

    List<TriCell> list = new List<TriCell>();

    public void Enqueue(TriCell cell) {
        count++;
        int priority = cell.SearchPriority;
        if (priority < minimum) {
            minimum = priority;
        }
        while (priority >= list.Count) {
            list.Add(null);
        }
        cell.NextWithSamePriority = list[priority];
        list[priority] = cell;
    }

    public TriCell Dequeue() {
        count--;
        for (;minimum< list.Count; minimum++) {
            TriCell cell = list[minimum];
            if (cell != null) {
                list[minimum] = cell.NextWithSamePriority;
                return cell;
            }
        }
        return null;
    }

    public void Change(TriCell cell,int oldPriority) {
        TriCell current = list[oldPriority];
        TriCell next = current.NextWithSamePriority;
        if (current == cell) {
            list[oldPriority] = next;
        }
        else {
            while (next != cell) {
                current = next;
                next = current.NextWithSamePriority;
            }
            current.NextWithSamePriority = cell.NextWithSamePriority;
        }
        Enqueue(cell);
        count -= 1;
    }

    public void Clear() {
        list.Clear();
        count = 0;
        minimum = int.MaxValue;
    }
}