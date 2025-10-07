using System;
using System.Collections.Generic;

[Serializable]
public class TimelineContext
{
    public TimelinePlayer player;
    private Dictionary<string, object> _trackReferences = new();

    public void AddReference(string tag, object obj)
    {
        _trackReferences.Add(tag, obj);
    }

    public T GetReference<T>(string tag)
    {
        if (!_trackReferences.ContainsKey(tag)) return default;

        var obj = _trackReferences[tag];
        if (obj is T reference)
            return reference;

        return default;
    }
}