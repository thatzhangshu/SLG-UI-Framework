using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局事件管理器。
/// 支持无参数事件和一个参数事件。
/// 用于模块之间解耦通信。
/// </summary>
public static class EventManager
{
    /// <summary>
    /// 无参数事件表。
    /// string：事件名
    /// Action：监听回调
    /// </summary>
    private static readonly Dictionary<string, Action> eventDict =
        new Dictionary<string, Action>();

    /// <summary>
    /// 一个参数事件表。
    /// string：事件名
    /// Delegate：Action<T>
    /// </summary>
    private static readonly Dictionary<string, Delegate> genericEventDict =
        new Dictionary<string, Delegate>();

    // =========================
    // 无参数事件
    // =========================

    public static void AddListener(string eventName, Action callback)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("[EventManager] AddListener failed: eventName is empty.");
            return;
        }

        if (callback == null)
        {
            Debug.LogWarning($"[EventManager] AddListener failed: callback is null. Event = {eventName}");
            return;
        }

        if (!eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] = null;
        }

        if (ContainsDelegate(eventDict[eventName], callback))
        {
            Debug.LogWarning($"[EventManager] Duplicate listener ignored. Event = {eventName}, Callback = {callback.Method.Name}");
            return;
        }

        eventDict[eventName] += callback;
    }

    public static void RemoveListener(string eventName, Action callback)
    {
        if (string.IsNullOrEmpty(eventName) || callback == null)
        {
            return;
        }

        if (!eventDict.ContainsKey(eventName))
        {
            return;
        }

        eventDict[eventName] -= callback;

        if (eventDict[eventName] == null)
        {
            eventDict.Remove(eventName);
        }
    }

    public static void Dispatch(string eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("[EventManager] Dispatch failed: eventName is empty.");
            return;
        }

        if (!eventDict.TryGetValue(eventName, out Action callback))
        {
            return;
        }

        callback?.Invoke();
    }

    // =========================
    // 一个参数事件
    // =========================

    public static void AddListener<T>(string eventName, Action<T> callback)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("[EventManager] AddListener<T> failed: eventName is empty.");
            return;
        }

        if (callback == null)
        {
            Debug.LogWarning($"[EventManager] AddListener<T> failed: callback is null. Event = {eventName}");
            return;
        }

        if (!genericEventDict.ContainsKey(eventName))
        {
            genericEventDict[eventName] = null;
        }

        Delegate currentDelegate = genericEventDict[eventName];

        if (currentDelegate != null && !(currentDelegate is Action<T>))
        {
            Debug.LogError(
                $"[EventManager] AddListener<T> type mismatch. Event = {eventName}, " +
                $"Expected = {currentDelegate.GetType()}, Add = Action<{typeof(T).Name}>"
            );
            return;
        }

        if (ContainsDelegate(currentDelegate, callback))
        {
            Debug.LogWarning($"[EventManager] Duplicate generic listener ignored. Event = {eventName}, Callback = {callback.Method.Name}");
            return;
        }

        genericEventDict[eventName] = (Action<T>)genericEventDict[eventName] + callback;
    }

    public static void RemoveListener<T>(string eventName, Action<T> callback)
    {
        if (string.IsNullOrEmpty(eventName) || callback == null)
        {
            return;
        }

        if (!genericEventDict.ContainsKey(eventName))
        {
            return;
        }

        Delegate currentDelegate = genericEventDict[eventName];

        if (currentDelegate != null && !(currentDelegate is Action<T>))
        {
            Debug.LogError(
                $"[EventManager] RemoveListener<T> type mismatch. Event = {eventName}, " +
                $"Expected = {currentDelegate.GetType()}, Remove = Action<{typeof(T).Name}>"
            );
            return;
        }

        genericEventDict[eventName] = (Action<T>)genericEventDict[eventName] - callback;

        if (genericEventDict[eventName] == null)
        {
            genericEventDict.Remove(eventName);
        }
    }

    public static void Dispatch<T>(string eventName, T arg)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("[EventManager] Dispatch<T> failed: eventName is empty.");
            return;
        }

        if (!genericEventDict.TryGetValue(eventName, out Delegate currentDelegate))
        {
            return;
        }

        if (currentDelegate is Action<T> callback)
        {
            callback.Invoke(arg);
        }
        else
        {
            Debug.LogError(
                $"[EventManager] Dispatch<T> type mismatch. Event = {eventName}, " +
                $"Expected = {currentDelegate.GetType()}, Dispatch = Action<{typeof(T).Name}>"
            );
        }
    }

    // =========================
    // Debug / Utility
    // =========================

    public static void ClearAll()
    {
        eventDict.Clear();
        genericEventDict.Clear();

        Debug.Log("[EventManager] ClearAll");
    }

    public static int GetListenerCount(string eventName)
    {
        int count = 0;

        if (eventDict.TryGetValue(eventName, out Action action) && action != null)
        {
            count += action.GetInvocationList().Length;
        }

        if (genericEventDict.TryGetValue(eventName, out Delegate genericDelegate) && genericDelegate != null)
        {
            count += genericDelegate.GetInvocationList().Length;
        }

        return count;
    }

    public static void DebugPrint()
    {
        Debug.Log("========== EventManager Debug ==========");

        foreach (KeyValuePair<string, Action> pair in eventDict)
        {
            int count = pair.Value != null ? pair.Value.GetInvocationList().Length : 0;
            Debug.Log($"[Event] {pair.Key} | Listener Count = {count}");
        }

        foreach (KeyValuePair<string, Delegate> pair in genericEventDict)
        {
            int count = pair.Value != null ? pair.Value.GetInvocationList().Length : 0;
            Debug.Log($"[Generic Event] {pair.Key} | Listener Count = {count}");
        }

        Debug.Log("========================================");
    }

    private static bool ContainsDelegate(Delegate source, Delegate target)
    {
        if (source == null || target == null)
        {
            return false;
        }

        Delegate[] invocationList = source.GetInvocationList();

        for (int i = 0; i < invocationList.Length; i++)
        {
            if (invocationList[i] == target)
            {
                return true;
            }
        }

        return false;
    }
}