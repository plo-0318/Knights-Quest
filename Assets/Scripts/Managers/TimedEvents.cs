using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TimedEvents
{
    private List<TimedEvent> _timedEvents;

    public TimedEvents()
    {
        _timedEvents = new List<TimedEvent>();
    }

    public void AddTimedEvents(TimedEvent[] timedEvents)
    {
        foreach (var e in timedEvents)
        {
            _timedEvents.Add(e);
        }

        _timedEvents.Sort();
    }

    public void AddTimedEvent(TimedEvent timedEvent)
    {
        _timedEvents.Add(timedEvent);
        _timedEvents.Sort();
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

        return _timedEvents[0].Callback;
    }

    public int NextEventTime()
    {
        if (Empty())
        {
            return -1;
        }

        return _timedEvents[0].Time;
    }

    public void Shift()
    {
        if (!Empty())
        {
            _timedEvents.RemoveAt(0);
        }
    }
}

public class TimedEvent : IComparable<TimedEvent>
{
    private int time;
    private Action callback;

    public TimedEvent(int time, Action callback)
    {
        this.time = time;
        this.callback = callback;
    }

    public int CompareTo(TimedEvent other)
    {
        return time.CompareTo(other.time);
    }

    public int Time => time;
    public Action Callback => callback;
}
