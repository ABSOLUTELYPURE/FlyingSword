using UnityEngine;

public class Utils
{
    public static Vector3 Screen2WorldPosition(Vector2 pos, float z = 0)
    {
        Camera cam = Camera.main;
        //pos = pos - new Vector2(Screen.width/2.0f,Screen.height/2.0f);
        Vector3 wPos = cam.ScreenToWorldPoint(pos);
        wPos = new Vector3(wPos.x, wPos.y, z);
        return wPos;
    }
    public static Vector3 View2WorldPosition(Vector2 pos)
    {
        Camera cam = Camera.main;
        Vector3 wPos = cam.ViewportToWorldPoint(pos);
        wPos = new Vector3(wPos.x, wPos.y, 0);
        return wPos;
    }
}