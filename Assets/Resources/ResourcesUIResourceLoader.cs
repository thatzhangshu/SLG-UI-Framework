using UnityEngine;

/// <summary>
/// 基于 Resources 的 UI 资源加载器。
/// 
/// 路径示例：
/// Assets/Resources/UI/Panel/MailPanel.prefab
/// 加载路径：
/// UI/Panel/MailPanel
/// </summary>
public class ResourcesUIResourceLoader : IUIResourceLoader
{
    public T LoadUI<T>(string path) where T : UIBase
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("[ResourcesUIResourceLoader] LoadUI failed. Path is empty.");
            return null;
        }

        GameObject prefabObject = Resources.Load<GameObject>(path);

        if (prefabObject == null)
        {
            Debug.LogError($"[ResourcesUIResourceLoader] LoadUI failed. Prefab not found. Path = Resources/{path}");
            return null;
        }

        T uiPrefab = prefabObject.GetComponent<T>();

        if (uiPrefab == null)
        {
            Debug.LogError(
                $"[ResourcesUIResourceLoader] LoadUI failed. " +
                $"Prefab does not have component {typeof(T).Name}. Path = Resources/{path}"
            );
            return null;
        }

        return uiPrefab;
    }
}