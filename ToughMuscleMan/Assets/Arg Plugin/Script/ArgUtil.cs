using UnityEngine;

public static class ArgUtil
{
    static string prefix = "<color=" + Keys.ColorSkyblue + ">[ARG LOGS] >>> </color>";
    public static void log(string Message, string color = "white")
    {
#if UNITY_EDITOR
        Debug.Log(prefix + "<color=" + color + ">" + Message + "</color>");
#else
        if (ARGManager.manager.data.showlogs)
        {
            Debug.Log(">>>> ARG LOGS <<<< " + Message);
        }
#endif
    }

    public static Color getColor(string colorcode, float alpha = 1f)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + colorcode, out color);
        color.a = alpha;
        return color;
    }
}
