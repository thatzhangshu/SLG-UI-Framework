/// <summary>
/// 武将静态配置。
/// 来自 HeroConfig.txt。
/// 这东西在工业级别项目里应该直接由导表工具生成到客户端 实际上就是配置文件->生成类 避免使用dict之类的东西来浪费性能
/// </summary>
public class HeroConfig
{
    public int heroId;
    public string nameTextId;
    public string descTextId;
    public string soldierTypeTextId;
    public string rarityTextId;
    public string campTextId;

    public HeroConfig(
        int heroId,
        string nameTextId,
        string descTextId,
        string soldierTypeTextId,
        string rarityTextId,
        string campTextId)
    {
        this.heroId = heroId;
        this.nameTextId = nameTextId;
        this.descTextId = descTextId;
        this.soldierTypeTextId = soldierTypeTextId;
        this.rarityTextId = rarityTextId;
        this.campTextId = campTextId;
    }
}