using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 文本管理器。
/// 负责读取 TextConfig，并根据 textId 返回显示文本。
/// </summary>
public static class TextManager
{
    private static readonly Dictionary<string, string> textDict = new Dictionary<string, string>();

    public static void Load()
    {
        textDict.Clear();

        List<string[]> rows = ConfigLoader.LoadTSV("Configs/TextConfig");

        // Debug.Log($"[TextManager] 加载文本：{rows.Count}");

        foreach (string[] columns in rows)
        {
            if (columns.Length < 2)
            {
                Debug.LogWarning("[TextManager] TextConfig 行格式错误，列数不足");
                Debug.Log($"[TextManager] 行数据：{string.Join(", ", columns)}");
                continue;
            }

            string textId = columns[0];
            string zhCN = columns[1];

            if (string.IsNullOrEmpty(textId))
            {
                continue;
            }

            if (textDict.ContainsKey(textId))
            {
                Debug.LogWarning($"[TextManager] TextId 重复：{textId}");
            }

            textDict[textId] = zhCN;
            // Debug.Log($"[TextManager] 加载文本：{textId} - {zhCN}");
        }

        Debug.Log($"[TextManager] 加载完成，文本数量：{textDict.Count}");
    }

    public static string GetText(string textId)
    {
        if (string.IsNullOrEmpty(textId))
        {
            return string.Empty;
        }

        if (textDict.TryGetValue(textId, out string text))
        {
            return text;
        }

        Debug.LogWarning($"[TextManager] 找不到文本：{textId}");
        return $"#{textId}";
    }

    public static string Format(string textId, params object[] args)
    {
        string format = GetText(textId);

        try
        {
            return string.Format(format, args);
        }
        catch
        {
            Debug.LogWarning($"[TextManager] 文本格式化失败：{textId}");
            return format;
        }
    }
}