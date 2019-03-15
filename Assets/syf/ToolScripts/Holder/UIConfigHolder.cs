using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UIConfigElement
{
    public string sName;//控件名称
    public string atlasName;//图集名称
}

public class UIConfigHolder : ScriptableObject
{
    public List<UIConfigElement> content;

    public void Add(UIConfigElement element)
    {
        if (null == content)
        {
            content = new List<UIConfigElement>();
        }
        content.Add(element);
    }
}
