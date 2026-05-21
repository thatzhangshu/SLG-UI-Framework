using UnityEngine;

/// <summary>
/// 单个 UI 层级节点。
/// 
/// 例如：
/// Layer_HUD
/// Layer_Panel
/// Layer_Popup
/// Layer_Toast
/// 
/// 该脚本用于告诉 UI 框架：
/// 当前 GameObject 对应哪一个 UI 层级。
/// </summary>
public class UILayerRoot : MonoBehaviour
{
    /// <summary>
    /// 当前节点代表的 UI 层级类型。
    /// </summary>
    public UILayer layer;

    /// <summary>
    /// 当前层级的根节点。
    /// UI 实例最终会挂载到该 Transform 下。
    /// </summary>
    public Transform Root => transform;
}