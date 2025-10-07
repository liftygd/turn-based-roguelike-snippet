using UnityEngine;

public abstract class TimelineClip : ScriptableObject
{
    public string clipName = "Clip";
    public float startTime;
    public float duration = 1f;
    public bool sizeLocked;

    public abstract void Play(float localTime, string trackName, TimelineContext context);
    public abstract void Stop(string trackName, TimelineContext context);
}