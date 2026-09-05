using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 邮件列表中的单个 Item。
/// </summary>
public class MailItem : MonoBehaviour, IUIItemPoolable
{
    [Header("UI References")]
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtSender;
    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private GameObject unreadNode;
    [SerializeField] private GameObject rewardNode;
    [SerializeField] private Button btnClick;

    private MailData currentData;
    private Action<MailData> onClick;

    private void Awake()
    {
        if (btnClick != null)
        {
            btnClick.onClick.AddListener(OnClickItem);
        }
    }

    private void OnDestroy()
    {
        if (btnClick != null)
        {
            btnClick.onClick.RemoveListener(OnClickItem);
        }
    }

    public void SetData(MailData data, Action<MailData> clickCallback)
    {
        currentData = data;
        onClick = clickCallback;

        RefreshView();
    }

    private void RefreshView()
    {
        if (currentData == null)
        {
            return;
        }

        if (txtTitle != null)
        {
            txtTitle.text = currentData.Title;
        }

        if (txtSender != null)
        {
            txtSender.text = currentData.Sender;
        }

        if (txtTime != null)
        {
            txtTime.text = currentData.TimeText;
        }

        if (unreadNode != null)
        {
            unreadNode.SetActive(!currentData.IsRead);
        }

        if (rewardNode != null)
        {
            rewardNode.SetActive(currentData.HasReward);
        }
    }

    private void OnClickItem()
    {
        if (currentData == null)
        {
            return;
        }

        onClick?.Invoke(currentData);
    }

    public void OnGetFromPool()
    {
        // 当前暂无特殊逻辑。
        // 后续如果有选中状态、动画状态，可以在这里重置。
    }

    public void OnReleaseToPool()
    {
        currentData = null;
        onClick = null;

        if (txtTitle != null)
        {
            txtTitle.text = string.Empty;
        }

        if (txtSender != null)
        {
            txtSender.text = string.Empty;
        }

        if (txtTime != null)
        {
            txtTime.text = string.Empty;
        }

        if (unreadNode != null)
        {
            unreadNode.SetActive(false);
        }

        if (rewardNode != null)
        {
            rewardNode.SetActive(false);
        }
    }
}