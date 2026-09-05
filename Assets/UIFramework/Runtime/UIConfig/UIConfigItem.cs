/// <summary>
/// 单个 UI 配置项。
/// 当前 MVP 阶段只记录 UI 名称和 Prefab 路径。
/// 后续可以扩展 Layer、Type、Cache、AnimationType 等字段。
/// </summary>
public class UIConfigItem
{
    public string Name { get; private set; }

    public string PrefabPath { get; private set; }

    public UIConfigItem(string name, string prefabPath)
    {
        Name = name;
        PrefabPath = prefabPath;
    }
}