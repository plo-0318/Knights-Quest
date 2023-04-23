using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TimedEvents
{
    private SortedDictionary<int, Action> _timedEvents;

    public TimedEvents()
    {
        _timedEvents = new SortedDictionary<int, Action>();
    }

    public void AddTimedEvent(int time, Action e)
    {
        _timedEvents.Add(time, e);
    }

    public bool Empty()
    {
        return _timedEvents.Count <= 0;
    }

    public Action NextEvent()
    {
        if (Empty())
        {
            return null;
        }

        return _timedEvents.First().Value;
    }

    public int NextEventTime()
    {
        if (Empty())
        {
            return -1;
        }

        return _timedEvents.First().Key;
    }

    public void Shift()
    {
        if (!Empty())
        {
            _timedEvents.Remove(_timedEvents.First().Key);
        }
    }
}
