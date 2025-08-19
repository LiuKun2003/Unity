#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public static class HierarchyEnhancer
{
    private static readonly Color DefaultBg   = new Color(0.16f, 0.16f, 0.16f, 1f);
    private static readonly Color DefaultText = new Color(0.82f, 0.82f, 0.82f, 1f);

    static HierarchyEnhancer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null || !IsTitleObject(go))
            return;
        
        if (!TryParseColors(go.name, out var bg, out var text, out var display))
        {
            bg   = DefaultBg;
            text = DefaultText;
        }

        EditorGUI.DrawRect(selectionRect, bg);

        var style = new GUIStyle(EditorStyles.label)
        {
            fontSize  = 14,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            normal    = { textColor = text }
        };

        var textRect = new Rect(selectionRect) { x = selectionRect.x + 2, width = selectionRect.width - 4 };
        EditorGUI.LabelField(textRect, display, style);
    }

    private static bool IsTitleObject(GameObject go) => go.name.StartsWith("---") && go.GetComponents<Component>().Length == 1;

    /// <summary>
    /// 解析末尾颜色码，格式必须严格：
    /// 1. 正好 1 个 #RRGGBB → 仅字体色
    /// 2. 正好 2 个 #RRGGBB → 第 1 个字体色，第 2 个背景色
    /// 其余任何情况返回 false，使用默认色。
    /// </summary>
    private static bool TryParseColors(string raw, out Color bg, out Color text, out string display)
    {
        bg = DefaultBg;
        text = DefaultText;
        display = raw[3..];   // 去掉前缀 ---

        // 末尾必须连续出现 1 或 2 个 #RRGGBB，且后面无其他任何字符
        var m = Regex.Match(display, @"^(.*?)(?:(#[0-9a-fA-F]{6})( #[0-9a-fA-F]{6})?)$");
        if (!m.Success) return false;

        var colorsPart = m.Groups[2].Value + (m.Groups[3].Success ? m.Groups[3].Value : "");
        var colors   = colorsPart.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        // 确保只有 1 或 2 个颜色码
        if (colors.Length != 1 && colors.Length != 2) return false;

        // 解析字体颜色
        if (!ColorUtility.TryParseHtmlString(colors[0], out text)) return false;

        // 如果有第二个颜色码，解析背景颜色
        if (colors.Length == 2)
        {
            if (!ColorUtility.TryParseHtmlString(colors[1], out bg)) return false;
        }

        display = m.Groups[1].Value.TrimEnd();
        return true;
    }
}
#endif