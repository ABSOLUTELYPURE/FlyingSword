using UnityEngine;

namespace Actor
{
    public class Sword:Actor
    {
        private TouchEventHandler onClick;
        private TouchEventHandler onDoubleClick;

        private void Awake()
        {
            onClick = new TouchEventHandler(OnClick);
            onDoubleClick = new TouchEventHandler(OnDoubleClick);
        }

        private void Start()
        {
            TouchManager.GetInstance().onOneTouchBegan += onClick;
            TouchManager.GetInstance().onTwoTouchBegan += onDoubleClick;
        }
        private void OnDestroy()
        {
            TouchManager.GetInstance().onOneTouchBegan -= onClick;
            TouchManager.GetInstance().onTwoTouchBegan -= onDoubleClick;
        }

        private void OnClick(TouchEventArgs args)
        {
            Vector3 pos = Utils.Screen2WorldPosition(args.touchedPos);
            //Vector3 pos = new Vector3(args.touchedPos.x - Screen.width / 2.0f, args.touchedPos.y - Screen.height / 2.0f, 0);
            this.transform.Translate(pos);
            Debug.Log("-------OnMove-------"+args.touchedPos);
            Debug.Log("-------OnMove-------"+pos);
        }
        private void OnDoubleClick(TouchEventArgs args)
        {
            Debug.Log("-------OnSkill-------");
        }
    }
}
