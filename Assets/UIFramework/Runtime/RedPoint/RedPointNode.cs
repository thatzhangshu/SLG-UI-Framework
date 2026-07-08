using System.Collections.Generic;

/// <summary>
/// 红点树节点。
/// SelfCount：当前节点自身红点数量。
/// TotalCount：当前节点自身 + 所有子节点的总红点数量。
/// </summary>
public class RedPointNode
{
    public string Key { get; private set; }

    public RedPointNode Parent { get; private set; }

    private readonly List<RedPointNode> children = new List<RedPointNode>();

    public IReadOnlyList<RedPointNode> Children => children;

    public int SelfCount { get; private set; }

    public int TotalCount { get; private set; }

    public bool IsShow => TotalCount > 0;

    public RedPointNode(string key)
    {
        Key = key;
    }

    public void SetParent(RedPointNode parent)
    {
        Parent = parent;
    }

    public void AddChild(RedPointNode child)
    {
        if (child == null)
        {
            return;
        }

        if (children.Contains(child))
        {
            return;
        }

        children.Add(child);
        child.SetParent(this);
    }

    public void SetSelfCount(int count)
    {
        SelfCount = count < 0 ? 0 : count;
    }

    public void RecalculateTotalCount()
    {
        int total = SelfCount;

        for (int i = 0; i < children.Count; i++)
        {
            total += children[i].TotalCount;
        }

        TotalCount = total;
    }
}