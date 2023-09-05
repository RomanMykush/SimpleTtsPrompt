using System;
using System.Collections.Generic;

public class History<T>
{
    private LinkedList<T> Sequence = new();
    public LinkedListNode<T>? Selected { get; private set; }
    public int MaxCount;
    public History(int maxCount)
    {
        MaxCount = maxCount;
    }

    public bool SelectFirst()
    {
        if (Sequence.Count <= 0)
            return false;

        Selected = Sequence.First;
        return true;
    }
    public bool SelectLast()
    {
        if (Sequence.Count <= 0)
            return false;

        Selected = Sequence.Last;
        return true;
    }

    public bool SelectNext()
    {
        if (Sequence.Count <= 0)
            return false;

        if (Selected == null)
        {
            Selected = Sequence.Last;
            return true;
        }

        if (Selected.Next == null)
            return false;

        Selected = Selected.Next;
        return true;
    }

    public bool SelectPrevious()
    {
        if (Sequence.Count <= 0)
            return false;

        if (Selected == null)
        {
            Selected = Sequence.Last;
            return true;
        }

        if (Selected.Previous == null)
            return false;

        Selected = Selected.Previous;
        return true;
    }

    public void Add(T entry)
    {
        Sequence.AddLast(entry);

        if (Sequence.Count <= MaxCount)
            return;

        if (Sequence.Last == Selected)
            Deselect();
        Sequence.RemoveFirst();
    }

    public void Deselect() => Selected = null;
}