/// <summary>
/// 英雄数据。
/// </summary>
public class HeroData
{
    /// <summary>
    /// 英雄ID。
    /// </summary>
    public int heroId;

    /// <summary>
    /// 英雄名称。
    /// </summary>
    public string heroName;

    /// <summary>
    /// 英雄等级。
    /// </summary>
    public int heroLevel;

    /// <summary>
    /// 卡片星级【1-5】
    /// </summary>
    public int cardStar;

    /// <summary>
    /// 基础攻击
    /// </summary>
    public int baseAttack;

    /// <summary>
    /// 基础防御
    /// </summary>
    public int baseDefense;

    /// <summary>
    /// 基础谋略
    /// </summary>
    public int baseStrategy;

    /// <summary>
    /// 基础速度
    /// </summary>
    public int baseSpeed;

    /// <summary>
    /// 主战法id
    /// </summary>
    public int mainSkillId;

    /// <summary>
    /// 武将生平
    /// </summary>
    public string heroBiography;

    /// <summary>
    /// 英雄阵营
    /// </summary>
    public string heroCamp;

    // public HeroData(int heroId, string heroName, int heroLevel, int cardStar, int baseAttack, int baseDefense, int baseStrategy, int baseSpeed, int mainSkillId, string heroBiography, int heroCamp)
    // {
    //     this.heroId = heroId;
    //     this.heroName = heroName;
    //     this.heroLevel = heroLevel;
    //     this.cardStar = cardStar;
    //     this.baseAttack = baseAttack;
    //     this.baseDefense = baseDefense;
    //     this.baseStrategy = baseStrategy;
    //     this.baseSpeed = baseSpeed;
    //     this.mainSkillId = mainSkillId;
    //     this.heroBiography = heroBiography;
    //     this.heroCamp = heroCamp;
    // }
    public HeroData(int heroId, string heroName, int heroLevel, int cardStar, string heroCamp)
    {
        this.heroId = heroId;
        this.heroName = heroName;
        this.heroLevel = heroLevel;
        this.cardStar = cardStar;
        // this.baseAttack = baseAttack;
        // this.baseDefense = baseDefense;
        // this.baseStrategy = baseStrategy;
        // this.baseSpeed = baseSpeed;
        // this.mainSkillId = mainSkillId;
        // this.heroBiography = heroBiography;
        this.heroCamp = heroCamp;
    }
}
