using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public delegate void TouchEventHandler(GF.TouchEventArgs args);
namespace GF
{
public class TouchManager:MonoBehaviour
{
    private static TouchManager instance;
    private static bool isInited = false;

    public static bool IsInited()
    {
        return (null != instance && isInited);
    }

    public static TouchManager GetInstance()
    {
        if(instance == null)
        {
            GameObject go = new GameObject("TouchManager");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<TouchManager>();
            instance.Init();
        }
        return instance;
    }

    public event TouchEventHandler onOneTouchBegan;
    public event TouchEventHandler onOneTouchMoved;
    public event TouchEventHandler onOneTouchEnded;
    public event TouchEventHandler onTwoTouchBegan;
    public event TouchEventHandler onTwoTouchMoved;
    public event TouchEventHandler onTwoTouchEnded;

    private Vector3 touchStartPos;
    private bool getOneTouch;
    private bool getTwoTouch;

    public void Init()
    {
        onOneTouchBegan = new TouchEventHandler(InitOneBeganHandler);
        onOneTouchMoved = new TouchEventHandler(InitOneMovedHandler);
        onOneTouchEnded = new TouchEventHandler(InitOneEndedHandler);
        onTwoTouchBegan = new TouchEventHandler(InitTwoBeganHandler);
        onTwoTouchMoved = new TouchEventHandler(InitTwoMovedHandler);
        onTwoTouchEnded = new TouchEventHandler(InitTwoEndedHandler);

        touchStartPos = Vector3.zero;
        getOneTouch = false;
        getTwoTouch = false;
        instance = this;
        isInited = true;
    }

    private void InitOneBeganHandler(TouchEventArgs args)
    {
        //Debug.Log("InitOneBeganHandler " + args.ToString());  
    }

    private void InitOneMovedHandler(TouchEventArgs args)
    {
        //Debug.Log("InitOneMovedHandler " + args.ToString());  
    }
    private void InitOneEndedHandler(TouchEventArgs args)
    {
        //Debug.Log("InitOneEndedHandler " + args.ToString());  
    }
    private void InitTwoBeganHandler(TouchEventArgs args)
    {
        //Debug.Log("InitTwoMovedHandler " + args.ToString());  
    }
    private void InitTwoMovedHandler(TouchEventArgs args)
    {
        //Debug.Log("InitTwoMovedHandler " + args.ToString());  
    }
    private void InitTwoEndedHandler(TouchEventArgs args)
    {
        //Debug.Log("InitTwoMovedHandler " + args.ToString());  
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GetTouchEvent();
        }
        else
        {
            GetMouseEvent();
        }
    }

    private void GetTouchEvent()
    {
        if (Input.touchCount == 0)
        {
            getOneTouch = false;
            if (getTwoTouch)
            {
                getTwoTouch = false;
                onTwoTouchEnded(new TouchEventArgs(Vector3.zero,0));
            }
            return;
        }
        if (Input.touchCount == 1)
        {
            if (getTwoTouch)
            {
                getTwoTouch = false;
                onTwoTouchEnded(new TouchEventArgs(Vector3.zero, 0));
            }
        }

        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch firstTouch = Input.GetTouch(0);
            if (firstTouch.phase == TouchPhase.Began)
            {
                touchStartPos = firstTouch.position;
                onOneTouchBegan(new TouchEventArgs(touchStartPos, 0));
                return;
            }
            if (firstTouch.phase == TouchPhase.Moved)
            {
                if (getTwoTouch) return;

                //Vector2 deltaMv = firstTouch.position - touchStartPos;
                onOneTouchMoved(new TouchEventArgs(touchStartPos, 0));
                touchStartPos = firstTouch.position;
                return;
            }
            if (firstTouch.phase == TouchPhase.Ended)
            {
                if (getTwoTouch) return;
                onOneTouchEnded(new TouchEventArgs(touchStartPos, 0));
                return;
            }
            return;
        }
        else if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            if (IsTouchDown(firstTouch) || IsTouchDown(secondTouch))
            {
                if (!getTwoTouch)
                {
                    onTwoTouchBegan(new TouchEventArgs(Vector3.zero, 0));
                }
                else
                {
                    onTwoTouchMoved(new TouchEventArgs(Vector3.zero, 0));
                }
                getTwoTouch = true;
                return;
            }
            getTwoTouch = true;
            return;
        }
    }

    private bool IsTouchDown(Touch touch)
    {
        return (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began);
    }

    private void GetMouseEvent()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            onOneTouchBegan(new TouchEventArgs(touchStartPos, 0));
            getOneTouch = true;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchStartPos = Input.mousePosition;
            onOneTouchEnded(new TouchEventArgs(touchStartPos, 0));
            getOneTouch = false;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (!getOneTouch) { return; }
            Vector3 deltaMv = ((Vector3)Input.mousePosition) - touchStartPos;
            if (Vector3.zero == deltaMv) { return; }
            onOneTouchMoved(new TouchEventArgs(touchStartPos, 0));
            touchStartPos = Input.mousePosition;
            return;
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (getTwoTouch) { return; }
            else
            {
                getTwoTouch = true;
                onTwoTouchBegan(new TouchEventArgs(Vector3.zero, 0));
            }
            return;
        }

        if(Input.GetMouseButtonUp(1))
        {
            onTwoTouchEnded(new TouchEventArgs(Vector3.zero, 0));
            getTwoTouch = false;
            return;
        }

        if(Input.GetMouseButton(1))
        {
            if (!getTwoTouch) { return; }
            onTwoTouchMoved(new TouchEventArgs(Vector3.zero, 0));
            return;
        }
    }
}

public class TouchEventArgs : System.EventArgs
{
    public Vector3 touchedPos;
    public float twoMvDis;

    public TouchEventArgs(Vector3 pos, float mvDis)
    {
        touchedPos = pos;//UnityHelper.GetGenScreenPos(pos);
        twoMvDis = mvDis;
    }

    public override string ToString()
    {
        return string.Format("touchedPos: {0}\t twoMvDis: {1}", touchedPos, twoMvDis);
    }
}

public class UnityHelper
{
    /// <summary>  
    ///  获取通用的屏幕坐标，即忽略dpi的坐标  
    /// </summary>  
    public static Vector2 GetGenScreenPos(Vector2 originPos)
    {
        Vector2 result = originPos;
        result.x /= (1.000f * Screen.width);
        result.y /= (1.000f * Screen.height);
        return result;
    }
    /// <summary>  
    /// 根据通用屏幕坐标计算出当前的实际屏幕坐标  
    /// </summary>  
    /// <param name="genPos"></param>  
    /// <returns></returns>  
    public static Vector2 GetScreenPosFromGen(Vector2 genPos)
    {
        Vector2 result = genPos;
        result.x *= Screen.width;
        result.y *= Screen.height;
        return result;
    }
    /// <summary>  
    /// 计算通用的屏幕距离，即可以忽略dpi的规范距离  
    /// </summary>  
    /// <param name="pos1"></param>  
    /// <param name="pos2"></param>  
    /// <returns></returns>  
    public static float GetGenScreenDis(Vector2 pos1, Vector2 pos2)
    {
        return Vector2.Distance(GetGenScreenPos(pos1), GetGenScreenPos(pos2));
    }
}
}