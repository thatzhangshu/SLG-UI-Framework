using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用 UI Item 对象池。
/// 
/// 适用于 MailItem、HeroCardItem、ChatMessageItem 等列表子节点。
/// 注意：
/// 这个池管理的是 UI 子节点，不是完整 Panel / Popup。
/// 完整 UI 的缓存仍由 UIManager 负责。
/// </summary>
public class UIItemPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;

    private readonly Queue<T> inactiveItems = new Queue<T>();
    private readonly List<T> activeItems = new List<T>();

    public int ActiveCount => activeItems.Count;
    public int InactiveCount => inactiveItems.Count;
    public int TotalCount => ActiveCount + InactiveCount;

    public UIItemPool(T prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    /// <summary>
    /// 从池中取出一个 Item。
    /// 池中没有可用对象时才 Instantiate。
    /// </summary>
    public T Get()
    {
        if (prefab == null)
        {
            Debug.LogError("[UIItemPool] Get failed. Prefab is null.");
            return null;
        }

        if (parent == null)
        {
            Debug.LogError("[UIItemPool] Get failed. Parent is null.");
            return null;
        }

        T item = null;

        while (inactiveItems.Count > 0 && item == null)
        {
            item = inactiveItems.Dequeue();
        }

        if (item == null)
        {
            item = Object.Instantiate(prefab, parent);
        }
        else
        {
            item.transform.SetParent(parent, false);
        }

        item.gameObject.SetActive(true);

        if (item is IUIItemPoolable poolable)
        {
            poolable.OnGetFromPool();
        }

        activeItems.Add(item);

        return item;
    }

    /// <summary>
    /// 回收一个指定 Item。
    /// </summary>
    public void Release(T item)
    {
        if (item == null)
        {
            return;
        }

        if (!activeItems.Remove(item))
        {
            return;
        }

        if (item is IUIItemPoolable poolable)
        {
            poolable.OnReleaseToPool();
        }

        item.gameObject.SetActive(false);
        item.transform.SetParent(parent, false);

        inactiveItems.Enqueue(item);
    }

    /// <summary>
    /// 回收当前所有激活 Item。
    /// </summary>
    public void ReleaseAll()
    {
        for (int i = activeItems.Count - 1; i >= 0; i--)
        {
            T item = activeItems[i];

            if (item == null)
            {
                continue;
            }

            if (item is IUIItemPoolable poolable)
            {
                poolable.OnReleaseToPool();
            }

            item.gameObject.SetActive(false);
            item.transform.SetParent(parent, false);

            inactiveItems.Enqueue(item);
        }

        activeItems.Clear();
    }

    /// <summary>
    /// 彻底销毁对象池中的所有 Item。
    /// 一般在 Panel 真正销毁时调用。
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (activeItems[i] != null)
            {
                Object.Destroy(activeItems[i].gameObject);
            }
        }

        activeItems.Clear();

        while (inactiveItems.Count > 0)
        {
            T item = inactiveItems.Dequeue();

            if (item != null)
            {
                Object.Destroy(item.gameObject);
            }
        }
    }

    public void DebugPrint(string poolName)
    {
        Debug.Log(
            $"[UIItemPool] {poolName} | " +
            $"Active = {ActiveCount}, " +
            $"Inactive = {InactiveCount}, " +
            $"Total = {TotalCount}"
        );
    }
}