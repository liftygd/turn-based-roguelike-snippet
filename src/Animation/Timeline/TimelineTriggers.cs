using System;
using System.Collections.Generic;

public class TimelineTriggers
{
    public enum TriggerType
    {
        TriggerEnter,
        TriggerExit
    }

    private readonly Dictionary<string, Action> _enterTriggers = new();
    private readonly Dictionary<string, Action> _exitTriggers = new();

    public void InvokeTrigger(TriggerType type, string triggerName)
    {
        if (type == TriggerType.TriggerEnter)
            _InvokeTrigger(_enterTriggers, triggerName);
        if (type == TriggerType.TriggerExit)
            _InvokeTrigger(_exitTriggers, triggerName);
    }

    public void RegisterTrigger(TriggerType type, string triggerName, Action action)
    {
        if (type == TriggerType.TriggerEnter)
            _RegisterTrigger(_enterTriggers, triggerName, action);
        if (type == TriggerType.TriggerExit)
            _RegisterTrigger(_exitTriggers, triggerName, action);
    }

    public void DeregisterTrigger(TriggerType type, string triggerName, Action action)
    {
        if (type == TriggerType.TriggerEnter)
            _DeregisterTrigger(_enterTriggers, triggerName, action);
        if (type == TriggerType.TriggerExit)
            _DeregisterTrigger(_exitTriggers, triggerName, action);
    }

    private void _InvokeTrigger(Dictionary<string, Action> triggers, string triggerName)
    {
        if (!triggers.ContainsKey(triggerName)) return;

        triggers[triggerName]?.Invoke();
    }

    private void _RegisterTrigger(Dictionary<string, Action> triggers, string triggerName, Action action)
    {
        if (!triggers.ContainsKey(triggerName))
            triggers.Add(triggerName, delegate { });

        triggers[triggerName] += action;
    }

    private void _DeregisterTrigger(Dictionary<string, Action> triggers, string triggerName, Action action)
    {
        if (!triggers.ContainsKey(triggerName))
            return;

        triggers[triggerName] -= action;
    }
}