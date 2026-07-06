/// <summary>
/// 玩家拥有的武将运行时数据。
/// 实际上用heroid来读取静态配置 这是玩家的动态数据
/// </summary>
public class HeroData
{
    public int heroId;
    public int level;
    public int power;

    public HeroData(int heroId, int level, int power)
    {
        this.heroId = heroId;
        this.level = level;
        this.power = power;
    }
}