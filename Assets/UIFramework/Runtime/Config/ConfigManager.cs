using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置管理器。
/// 当前负责统一加载静态配置表。
/// </summary>
public static class ConfigManager
{
    private static readonly Dictionary<int, HeroConfig> heroConfigDict = new Dictionary<int, HeroConfig>();

    public static bool IsLoaded { get; private set; }

    public static void LoadAll()
    {
        IsLoaded = false;

        TextManager.Load();
        LoadHeroConfig();

        IsLoaded = true;

        Debug.Log("[ConfigManager] 所有配置加载完成");
    }

    private static void LoadHeroConfig()
    {
        heroConfigDict.Clear();

        List<string[]> rows = ConfigLoader.LoadTSV("Configs/HeroConfig");

        foreach (string[] columns in rows)
        {
            if (columns.Length < 5)
            {
                Debug.LogWarning("[ConfigManager] HeroConfig 行格式错误，列数不足");
                continue;
            }

            if (!int.TryParse(columns[0], out int heroId))
            {
                Debug.LogWarning($"[ConfigManager] HeroId 解析失败：{columns[0]}");
                continue;
            }

            HeroConfig config = new HeroConfig(
                heroId,
                columns[1],
                columns[2],
                columns[3],
                columns[4],
                columns[5]
            );

            if (heroConfigDict.ContainsKey(heroId))
            {
                Debug.LogWarning($"[ConfigManager] HeroId 重复：{heroId}");
            }

            heroConfigDict[heroId] = config;
        }

        Debug.Log($"[ConfigManager] HeroConfig 加载完成，数量：{heroConfigDict.Count}");
    }

    public static HeroConfig GetHeroConfig(int heroId)
    {
        if (heroConfigDict.TryGetValue(heroId, out HeroConfig config))
        {
            return config;
        }

        Debug.LogWarning($"[ConfigManager] 找不到 HeroConfig：{heroId}");
        return null;
    }

    public static List<HeroConfig> GetAllHeroConfigs()
    {
        return new List<HeroConfig>(heroConfigDict.Values);
    }
}