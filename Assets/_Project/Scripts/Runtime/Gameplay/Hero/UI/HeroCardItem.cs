using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// 英雄卡片
/// </summary>

public class HeroCardItem : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button btnSelect;

    [Header("Texts")]
    [SerializeField] private TMP_Text txtHeroName;
    [SerializeField] private TMP_Text txtHeroLevel;
    [SerializeField] private TMP_Text txtHeroCardStar;
    [SerializeField] private TMP_Text txtHeroCamp;

    private HeroData heroData;
    private Action<HeroData> onClickCallback;

    private void Awake()
    {
        if (btnSelect != null)
        {
            btnSelect.onClick.AddListener(OnClickSelect);
        }
    }

    private void OnClickSelect()
    {
        if (heroData != null && onClickCallback != null)
        {
            onClickCallback.Invoke(heroData);
        }
    }

    public void SetData(HeroData data, Action<HeroData> onClick)
    {
        heroData = data;
        onClickCallback = onClick;
        RefreshView();
    }

    private void RefreshView()
    {
        if (heroData == null)
        {
            return;
        }

        HeroConfig heroConfig = ConfigManager.GetHeroConfig(heroData.heroId);

        if (heroConfig == null)
        {
            return;
        }

        if (txtHeroName != null)
        {
            txtHeroName.text = TextManager.GetText(heroConfig.nameTextId);
        }

        if (txtHeroLevel != null)
        {
            txtHeroLevel.text = TextManager.Format("UI_LEVEL_FORMAT", heroData.level);
        }

        if (txtHeroCardStar != null)
        {
            txtHeroCardStar.text = TextManager.GetText(heroConfig.rarityTextId);
        }

        if (txtHeroCamp != null)
        {
            txtHeroCamp.text = TextManager.GetText(heroConfig.campTextId);
        }

    }

    private void OnDestroy()
    {
        if (btnSelect != null)
        {
            btnSelect.onClick.RemoveListener(OnClickSelect);
        }
    }

}
