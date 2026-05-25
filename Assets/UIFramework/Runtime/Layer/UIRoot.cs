using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI 根节点管理器。
/// 
/// 负责收集并管理所有 UILayerRoot。
/// UIManager 后续会通过 UIRoot 找到正确的 UI 层级，
/// 然后把实例化出来的 UI 挂到对应节点下。
/// </summary>
public class UIRoot : MonoBehaviour
{
    [SerializeField]
    private PopupMask popupMask;

    public PopupMask PopupMask => popupMask;
    /// <summary>
    /// UI 层级映射表。
    /// Key：UILayer 枚举
    /// Value：该层级对应的 Transform
    /// </summary>
    private Dictionary<UILayer, Transform> layerRoots = new Dictionary<UILayer, Transform>();

    /// <summary>
    /// 添加 UI 层级节点。
    /// </summary>
    public void AddLayerRoot(UILayer layer, Transform root)
    {
        layerRoots[layer] = root;
    }
    
    public Transform GetLayerRoot(UILayer layer)
    {
        if (layerRoots.TryGetValue(layer, out Transform root))
        {
            return root;
        }
        Debug.LogError($"Layer root {layer} not found");
        return null;
    }

    private void InitLayerRoots()
    {
        layerRoots.Clear();
        UILayerRoot[] layers = GetComponentsInChildren<UILayerRoot>(true);
        foreach (UILayerRoot layerRoot in layers)
        {
            if (layerRoot == null) continue;
            if (layerRoots.ContainsKey(layerRoot.layer))
            {
                Debug.LogError($"Layer root {layerRoot.name} already exists");
                continue;
            } 
            AddLayerRoot(layerRoot.layer, layerRoot.transform);
        }
    }
    private void Awake()
    {
        InitLayerRoots();
    }
    
    /// <summary>
    /// 根据 UI 层级类型获取对应的根节点。
    /// </summary>

}
