using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 邮件页面。
/// 负责邮件列表展示与返回逻辑。
/// </summary>
public class MailPanel : UIPanelBase
{
    [Header("Buttons")]
    [SerializeField] private Button btnBack;

    [Header("Mail List")]
    [SerializeField] private ScrollRect mailScrollRect;
    [SerializeField] private Transform mailContentRoot;
    [SerializeField] private MailItem mailItemPrefab;

    [Header("Popup Prefabs")]
    [SerializeField] private MailDetailPopup mailDetailPopupPrefab;
    private readonly List<MailData> mailDataList = new List<MailData>();
    private readonly List<MailItem> mailItemList = new List<MailItem>();

    public override void OnInit()
    {
        base.OnInit();

        BindButtons();
        GenerateMockData();
        RefreshMailList();
    }

    private void BindButtons()
    {
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }
    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
    }

    /// <summary>
    /// 生成假邮件数据。
    /// 当前阶段用于验证列表 UI。
    /// </summary>
    private void GenerateMockData()
    {
        mailDataList.Clear();

        for (int i = 0; i < 20; i++)
        {
            MailData data = new(
                i + 1,
                100,
                $"系统邮件 {i + 1}",
                i % 2 == 0 ? "系统" : "联盟",
                $"2026-05-{10 + i:00}",
                $"这是第 {i + 1} 封测试邮件内容。",
                i % 3 == 0,
                false
            );

            mailDataList.Add(data);
        }
    }

    /// <summary>
    /// 刷新邮件列表。
    /// 第一版直接 Instantiate，后续再升级为对象池。
    /// </summary>
    private void RefreshMailList()
    {
        ClearMailItems();

        foreach (MailData data in mailDataList)
        {
            MailItem item = Instantiate(mailItemPrefab, mailContentRoot);
            item.SetData(data, OnClickMailItem);

            mailItemList.Add(item);
        }
        StartCoroutine(ResetScrollToTop());
    }


    /// <summary>
    private IEnumerator ResetScrollToTop()
    {
        yield return null;

        Canvas.ForceUpdateCanvases();

        if (mailScrollRect != null)
        {
            mailScrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void OnClickMailItem(MailData data)
    {
        UIManager.Instance.OpenUI(mailDetailPopupPrefab, data);
    }
    private void ClearMailItems()
    {
        foreach (MailItem item in mailItemList)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        mailItemList.Clear();
    }

    public override void OnDestroy()
    {
        if (btnBack != null)
        {
            btnBack.onClick.RemoveListener(OnClickBack);
        }

        base.OnDestroy();
    }
}