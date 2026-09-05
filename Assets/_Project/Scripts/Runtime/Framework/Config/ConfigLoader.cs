using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置加载工具。
/// 当前 Demo 使用 Resources + TSV 文本格式。
/// </summary>
public static class ConfigLoader
{
    /// <summary>
    /// 加载 TSV 配置。
    /// path 不需要写 Resources，也不需要写后缀。
    /// 例如：Configs/TextConfig
    /// </summary>
    public static List<string[]> LoadTSV(string path, bool skipHeader = true)
    {
        List<string[]> rows = new List<string[]>();

        TextAsset textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            Debug.LogError($"[ConfigLoader] 加载失败，找不到配置：Resources/{path}");
            return rows;
        }

        string content = textAsset.text.Replace("\r\n", "\n").Replace("\r", "\n");
        string[] lines = content.Split('\n');

        int startIndex = skipHeader ? 1 : 0;

        for (int i = startIndex; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] columns = line.Split('\t');

            for (int j = 0; j < columns.Length; j++)
            {
                columns[j] = columns[j].Trim();
            }

            rows.Add(columns);
        }

        return rows;
    }
}