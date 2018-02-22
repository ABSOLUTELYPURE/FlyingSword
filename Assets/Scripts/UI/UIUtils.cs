using System.Collections;
using UnityEngine;

public class UIUtils
{   
    public static Camera GetUICamera()
    {
        GameObject go = GameObject.Find("GameUI");
        if(go)
        {
            Camera uiCam = go.GetComponentInChildren<Camera>();
            return uiCam;
        }
        Debug.LogWarningFormat("Can't find uiCamera");
        return null;
    }

    public static Vector3 UI2WorldPosition(Vector2 pos)//,float z = 0)
    {   
        Camera uicam = UIUtils.GetUICamera();
        Vector3 wPos = uicam.ScreenToWorldPoint(pos);
        return wPos;
    }
}