using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 武将页面。
/// 用于展示和管理玩家拥有的武将。
/// </summary>
public class HeroPanel : UIPanelBase
{
    [Header("Buttons")]
    [SerializeField] private Button btnBack;
    [Header("Hero List")]
    [SerializeField] private ScrollRect heroScrollRect;
    [SerializeField] private Transform heroContentRoot;
    [SerializeField] private HeroCardItem heroCardItemPrefab;

    [Header("Popup Prefabs")]
    [SerializeField] private HeroDetailPanel heroDetailPanelPrefab;

    private readonly List<HeroData> heroDataList = new List<HeroData>();
    private readonly List<HeroCardItem> heroCardItemList = new List<HeroCardItem>();
    public override void OnInit()
    {
        Debug.Log("HeroPanel OnInit");
        base.OnInit();
        BindButtons();
        GenerateMockData();
        RefreshHeroList();
    }


    /// <summary>
    /// 生成假武将数据。
    /// </summary>
    private void GenerateMockData()
    {
        heroDataList.Clear();
        heroDataList.Add(new HeroData(1001, 30, 12500));
        heroDataList.Add(new HeroData(1002, 28, 11800));
        heroDataList.Add(new HeroData(1003, 25, 9600));
        heroDataList.Add(new HeroData(1004, 32, 13200));
        heroDataList.Add(new HeroData(1005, 22, 8900));

        heroDataList.Add(new HeroData(1001, 18, 7600));
        heroDataList.Add(new HeroData(1002, 20, 8200));
        heroDataList.Add(new HeroData(1003, 16, 6400));
        heroDataList.Add(new HeroData(1004, 26, 10400));
        heroDataList.Add(new HeroData(1005, 14, 5100));

    }

    /// <summary>
    /// 刷新武将列表。
    /// 第一版直接 Instantiate，后续再升级为对象池。
    /// </summary>
    private void RefreshHeroList()
    {
        ClearHeroItems();

        foreach (HeroData data in heroDataList)
        {
            HeroCardItem item = Instantiate(heroCardItemPrefab, heroContentRoot);
            item.SetData(data, OnClickHeroItem);
            heroCardItemList.Add(item);
        }

        StartCoroutine(ResetScrollToTop());
    }

    private IEnumerator ResetScrollToTop()
    {
        yield return null;

        Canvas.ForceUpdateCanvases();

        if (heroScrollRect != null)
        {
            heroScrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void ClearHeroItems()
    {
        foreach (HeroCardItem item in heroCardItemList)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        heroCardItemList.Clear();
    }

    public void BindButtons()
    {
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }
    }

    public void OnClickBack()

    {
        Debug.Log("OnClickBack");
        UIManager.Instance.Back();
    }

    private void OnClickHeroItem(HeroData data)
    {
        // Debug.Log($"点击武将：{data.heroId} - {data.heroName}");
        UIManager.Instance.OpenUI(heroDetailPanelPrefab, data);
    }
}
