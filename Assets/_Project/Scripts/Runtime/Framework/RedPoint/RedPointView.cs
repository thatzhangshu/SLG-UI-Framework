using TMPro;
using UnityEngine;

/// <summary>
/// 红点显示组件。
/// 挂在按钮或父节点上。
/// 不要挂在 RedPoint 小红点自己身上。
/// </summary>
public class RedPointView : MonoBehaviour
{
    [Header("Red Point")]
    [SerializeField] private string redPointKey;
    [SerializeField] private GameObject redPointNode;

    [Header("Count")]
    [SerializeField] private bool showCount;
    [SerializeField] private TMP_Text txtCount;
    [SerializeField] private int maxShowCount = 99;

    private void OnEnable()
    {
        RedPointManager.AddListener(redPointKey, OnRedPointChanged);
        Refresh();
    }

    private void OnDisable()
    {
        RedPointManager.RemoveListener(redPointKey, OnRedPointChanged);
    }

    private void OnRedPointChanged(int count)
    {
        SetCount(count);
    }

    private void Refresh()
    {
        int count = RedPointManager.GetCount(redPointKey);
        SetCount(count);
    }

    private void SetCount(int count)
    {
        bool isShow = count > 0;

        if (redPointNode != null)
        {
            redPointNode.SetActive(isShow);
        }

        if (txtCount == null)
        {
            return;
        }

        bool shouldShowCount = isShow && showCount;
        txtCount.gameObject.SetActive(shouldShowCount);

        if (!shouldShowCount)
        {
            return;
        }

        if (count > maxShowCount)
        {
            txtCount.text = $"{maxShowCount}+";
        }
        else
        {
            txtCount.text = count.ToString();
        }
    }
}