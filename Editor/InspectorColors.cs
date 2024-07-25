#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
using UnityEngine;

public enum ThemeColor
{
    Col1,
    Col2,
    Col3,
    Col4,
    Col5
}

public static class InspectorColors
{
    public static string Color(ThemeColor color)
    {
        string target = "";

        switch (color)
        {
            case ThemeColor.Col1:
                target = "teal";
                break;
            case ThemeColor.Col2:
                target = "#38c194";
                break;
            case ThemeColor.Col3:
                target = "#31bdb6";
                break;
            case ThemeColor.Col4:
                target = "#2c99bc";
                break;
            case ThemeColor.Col5:
                target = "#3775bf";
                break;
        }

        if (target == "")
        {
            Debug.Log($"<color=red>Target was empty.</color>");
            return null;
        }
        return target;
    }
}
#endif