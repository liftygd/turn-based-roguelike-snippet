using System;
using System.Collections.Generic;
using UnityEngine;

public class TimelineTrack : ScriptableObject
{
    public string trackName;

    public List<TimelineClip> clips = new();
    public virtual Color trackColor => Color.white;
    public virtual string trackIcon => "";
    public virtual Type clipType => null;

    public virtual void Play(float previousTime, float currentTime, TimelineContext context)
    {
        foreach (var clip in clips)
        {
            var localTime = currentTime - clip.startTime;
            if (localTime >= 0 && localTime <= clip.duration)
                clip.Play(localTime, trackName, context);
        }
    }

    public virtual void Stop(TimelineContext context)
    {
        foreach (var clip in clips)
            clip.Stop(trackName, context);
    }
}