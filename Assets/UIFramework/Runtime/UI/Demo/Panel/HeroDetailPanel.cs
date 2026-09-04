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

        HeroConfig heroConfig = ConfigManager.GetHeroConfig(currentHeroData.heroId);

        if (heroConfig == null)
        {
            Debug.LogWarning($"HeroDetailPanel 打开失败：HeroConfig 未找到，heroId = {currentHeroData.heroId}");
            return;
        }

        if (txtName != null)
        {
            txtName.text = TextManager.GetText(heroConfig.nameTextId);
        }

        if (txtLevel != null)
        {
            txtLevel.text = TextManager.Format("UI_LEVEL_FORMAT", currentHeroData.level);
        }

        if (txtStar != null)
        {
            txtStar.text = TextManager.GetText(heroConfig.rarityTextId);
        }

        if (txtCamp != null)
        {
            txtCamp.text = TextManager.GetText(heroConfig.campTextId);
        }

    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
    }

    protected override void OnDispose()
    {
        if (btnBack != null)
        {
            btnBack.onClick.RemoveListener(OnClickBack);
        }

        base.OnDispose();
    }
}