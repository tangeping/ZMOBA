using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUtils  {



    // check if the cursor is over a UI or OnGUI element right now
    // note: for UI, this only works if the UI's CanvasGroup blocks Raycasts
    // note: for OnGUI: hotControl is only set while clicking, not while zooming
    public static bool IsCursorOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject() //返回一个布尔值,进入了UI上就返回true
               || GUIUtility.hotControl != 0;//鼠标点击在UI上面时，hotControl为控件的controlID，弹起时 hotControl = 0
    }
}
