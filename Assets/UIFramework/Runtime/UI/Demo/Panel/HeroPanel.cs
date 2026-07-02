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

        string[] names =
        {
            "赵云", "关羽", "张飞", "马超", "黄忠",
            "诸葛亮", "周瑜", "吕蒙", "陆逊", "司马懿"
        };

        int cardStar = 5;

        string[] camps = { "魏", "蜀", "吴", "群" };

        for (int i = 0; i < 30; i++)
        {
            string heroName = names[i % names.Length];

            HeroData data = new HeroData(
                i + 1,
                heroName,
                1 + i % 50,
                cardStar,
                camps[i % camps.Length]
            );

            heroDataList.Add(data);
        }
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
            Debug.Log($"生成武将：{data.heroId} - {data.heroName}");

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
