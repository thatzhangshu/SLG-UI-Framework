using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红点树管理器。
/// 支持：
/// 1. 红点树注册
/// 2. 子节点变化向父节点传播
/// 3. 按 key 注册监听
/// 4. 只通知发生变化的 key 对应的 RedPointView
/// </summary>
public static class RedPointManager
{
    private static readonly Dictionary<string, RedPointNode> nodeDict =
        new Dictionary<string, RedPointNode>();

    private static readonly Dictionary<string, Action<int>> listenerDict =
        new Dictionary<string, Action<int>>();

    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// 初始化红点树。
    /// 当前 Demo 先注册 MainHUD 相关红点。
    /// 后续还没有的结构先留 TODO。
    /// </summary>
    public static void Initialize()
    {
        nodeDict.Clear();
        IsInitialized = false;

        BuildDefaultTree();

        IsInitialized = true;

        Debug.Log("[RedPointManager] Initialize completed.");
    }

    private static void BuildDefaultTree()
    {
        RegisterNode(RedPointKey.Root, null);
        RegisterNode(RedPointKey.MainHUD, RedPointKey.Root);

        // Mail
        RegisterNode(RedPointKey.Mail, RedPointKey.MainHUD);
        RegisterNode(RedPointKey.MailUnread, RedPointKey.Mail);
        RegisterNode(RedPointKey.MailReward, RedPointKey.Mail);

        // Hero
        RegisterNode(RedPointKey.Hero, RedPointKey.MainHUD);
        RegisterNode(RedPointKey.HeroNewHero, RedPointKey.Hero);
        RegisterNode(RedPointKey.HeroUpgradeable, RedPointKey.Hero);

        // Chat
        RegisterNode(RedPointKey.Chat, RedPointKey.MainHUD);
        RegisterNode(RedPointKey.ChatUnread, RedPointKey.Chat);

        // Activity
        RegisterNode(RedPointKey.Activity, RedPointKey.MainHUD);
        RegisterNode(RedPointKey.ActivityLoginReward, RedPointKey.Activity);
        RegisterNode(RedPointKey.ActivityDailyTask, RedPointKey.Activity);
        RegisterNode(RedPointKey.ActivityLimitedEvent, RedPointKey.Activity);

        // TODO：Alliance 红点树
        // RegisterNode(RedPointKey.Alliance, RedPointKey.MainHUD);
        // RegisterNode(RedPointKey.AllianceHelp, RedPointKey.Alliance);
        // RegisterNode(RedPointKey.AllianceGift, RedPointKey.Alliance);

        // TODO：World 红点树
        // RegisterNode(RedPointKey.World, RedPointKey.MainHUD);
        // RegisterNode(RedPointKey.WorldCity, RedPointKey.World);
        // RegisterNode(RedPointKey.WorldTroop, RedPointKey.World);
    }

    public static void RegisterNode(string key, string parentKey)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("[RedPointManager] RegisterNode failed: key is empty.");
            return;
        }

        RedPointNode node = GetOrCreateNode(key);

        if (string.IsNullOrEmpty(parentKey))
        {
            return;
        }

        RedPointNode parent = GetOrCreateNode(parentKey);
        parent.AddChild(node);
    }

    public static void AddListener(string key, Action<int> callback)
    {
        if (string.IsNullOrEmpty(key) || callback == null)
        {
            return;
        }

        if (!listenerDict.ContainsKey(key))
        {
            listenerDict[key] = null;
        }

        listenerDict[key] += callback;
    }

    public static void RemoveListener(string key, Action<int> callback)
    {
        if (string.IsNullOrEmpty(key) || callback == null)
        {
            return;
        }

        if (!listenerDict.ContainsKey(key))
        {
            return;
        }

        listenerDict[key] -= callback;

        if (listenerDict[key] == null)
        {
            listenerDict.Remove(key);
        }
    }

    public static void SetActive(string key, bool isActive)
    {
        SetCount(key, isActive ? 1 : 0);
    }

    public static void SetCount(string key, int count)
    {
        if (!IsInitialized)
        {
            Debug.LogWarning("[RedPointManager] SetCount failed: RedPointManager is not initialized.");
            return;
        }

        RedPointNode node = GetNode(key);

        if (node == null)
        {
            Debug.LogWarning($"[RedPointManager] SetCount failed, node not found: {key}");
            return;
        }

        int newCount = Mathf.Max(0, count);

        if (node.SelfCount == newCount)
        {
            return;
        }

        node.SetSelfCount(newCount);
        RefreshUpwards(node);
    }

    public static void ClearRedPoint(string key)
    {
        SetCount(key, 0);
    }

    /// <summary>
    /// 清除当前节点和所有子节点红点。
    /// 适合点击 MainHUD 入口时使用。
    /// 例如点击 Activity 后，清掉 Activity 下面所有子红点。
    /// </summary>
    public static void ClearSubTree(string key)
    {
        if (!IsInitialized)
        {
            Debug.LogWarning("[RedPointManager] ClearSubTree failed: RedPointManager is not initialized.");
            return;
        }

        RedPointNode node = GetNode(key);

        if (node == null)
        {
            Debug.LogWarning($"[RedPointManager] ClearSubTree failed, node not found: {key}");
            return;
        }

        ClearSelfCountRecursive(node);
        RecalculateSubTreeAndNotify(node);

        if (node.Parent != null)
        {
            RefreshUpwards(node.Parent);
        }
    }

    public static bool IsShow(string key)
    {
        RedPointNode node = GetNode(key);
        return node != null && node.IsShow;
    }

    public static int GetCount(string key)
    {
        RedPointNode node = GetNode(key);
        return node != null ? node.TotalCount : 0;
    }

    public static void ResetAllCounts()
    {
        foreach (RedPointNode node in nodeDict.Values)
        {
            node.SetSelfCount(0);
        }

        RedPointNode root = GetNode(RedPointKey.Root);

        if (root != null)
        {
            RecalculateSubTreeAndNotify(root);
        }
    }

    private static RedPointNode GetOrCreateNode(string key)
    {
        if (nodeDict.TryGetValue(key, out RedPointNode node))
        {
            return node;
        }

        node = new RedPointNode(key);
        nodeDict.Add(key, node);

        return node;
    }

    private static RedPointNode GetNode(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        nodeDict.TryGetValue(key, out RedPointNode node);
        return node;
    }

    /// <summary>
    /// 从当前节点向父节点逐级刷新。
    /// 只刷新当前节点到 Root 的路径。
    /// </summary>
    private static void RefreshUpwards(RedPointNode node)
    {
        while (node != null)
        {
            int oldTotalCount = node.TotalCount;

            node.RecalculateTotalCount();

            if (oldTotalCount != node.TotalCount)
            {
                Notify(node.Key, node.TotalCount);
            }

            node = node.Parent;
        }
    }

    private static void ClearSelfCountRecursive(RedPointNode node)
    {
        if (node == null)
        {
            return;
        }

        node.SetSelfCount(0);

        IReadOnlyList<RedPointNode> children = node.Children;

        for (int i = 0; i < children.Count; i++)
        {
            ClearSelfCountRecursive(children[i]);
        }
    }

    /// <summary>
    /// 清子树时，需要从叶子向上重新计算。
    /// </summary>
    private static void RecalculateSubTreeAndNotify(RedPointNode node)
    {
        if (node == null)
        {
            return;
        }

        IReadOnlyList<RedPointNode> children = node.Children;

        for (int i = 0; i < children.Count; i++)
        {
            RecalculateSubTreeAndNotify(children[i]);
        }

        int oldTotalCount = node.TotalCount;

        node.RecalculateTotalCount();

        if (oldTotalCount != node.TotalCount)
        {
            Notify(node.Key, node.TotalCount);
        }
    }

    private static void Notify(string key, int count)
    {
        Debug.Log($"[RedPointManager] Changed: {key} = {count}");

        if (listenerDict.TryGetValue(key, out Action<int> callback))
        {
            callback?.Invoke(count);
        }
    }

    public static void DebugPrintTree()
    {
        RedPointNode root = GetNode(RedPointKey.Root);

        if (root == null)
        {
            Debug.Log("[RedPointManager] Tree is empty.");
            return;
        }

        DebugPrintNode(root, 0);
    }

    private static void DebugPrintNode(RedPointNode node, int depth)
    {
        string indent = new string(' ', depth * 2);
        Debug.Log($"{indent}{node.Key} | Self={node.SelfCount}, Total={node.TotalCount}");

        IReadOnlyList<RedPointNode> children = node.Children;

        for (int i = 0; i < children.Count; i++)
        {
            DebugPrintNode(children[i], depth + 1);
        }
    }
}