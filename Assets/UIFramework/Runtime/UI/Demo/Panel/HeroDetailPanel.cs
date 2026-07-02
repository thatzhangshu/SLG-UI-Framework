using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDetailPanel : UIPanelBase
{
    [Header("Texts")]
    [SerializeField] private TMP_Text txtName;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private TMP_Text txtStar;
    [SerializeField] private TMP_Text txtCamp;
    // [SerializeField] private TMP_Text txtSoldierType;
    // [SerializeField] private TMP_Text txtPower;
    // [SerializeField] private TMP_Text txtDesc;

    [Header("Buttons")]
    [SerializeField] private Button btnBack;

    private HeroData currentHeroData;

    public override void OnInit()
    {
        base.OnInit();

        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }
    }

    public override void OnOpen(object data)
    {
        base.OnOpen(data);

        currentHeroData = data as HeroData;

        RefreshView();
    }

    private void RefreshView()
    {
        if (currentHeroData == null)
        {
            Debug.LogWarning("HeroDetailPanel 打开失败：HeroData 为空");
            return;
        }

        // HeroConfig config = ConfigManager.GetHeroConfig(currentHeroData.heroId);

        // if (config == null)
        // {
        //     Debug.LogWarning($"HeroDetailPanel 打开失败：HeroConfig 未找到，heroId = {currentHeroData.heroId}");
        //     return;
        // }

        // if (txtName != null)
        // {
        //     txtName.text = TextManager.GetText(config.nameTextId);
        // }

        // if (txtLevel != null)
        // {
        //     txtLevel.text = $"{TextManager.GetText("UI_LEVEL")}：{currentHeroData.level}";
        // }

        // if (txtRarity != null)
        // {
        //     txtRarity.text = $"{TextManager.GetText("UI_RARITY")}：{TextManager.GetText(config.rarityTextId)}";
        // }

        // if (txtSoldierType != null)
        // {
        //     txtSoldierType.text = $"{TextManager.GetText("UI_SOLDIER_TYPE")}：{TextManager.GetText(config.soldierTypeTextId)}";
        // }

        // if (txtPower != null)
        // {
        //     txtPower.text = $"{TextManager.GetText("UI_POWER")}：{currentHeroData.power}";
        // }

        // if (txtDesc != null)
        // {
        //     txtDesc.text = TextManager.GetText(config.descTextId);
        // }
        if (currentHeroData == null)
        {
            return;
        }

        if (txtName != null)
        {
            txtName.text = currentHeroData.heroName;
        }

        if (txtLevel != null)
        {
            txtLevel.text = $"Lv.{currentHeroData.heroLevel}";
        }

        if (txtStar != null)
        {
            txtStar.text = $"★{currentHeroData.cardStar}";
        }

        if (txtCamp != null)
        {
            txtCamp.text = currentHeroData.heroCamp;
        }

    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
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