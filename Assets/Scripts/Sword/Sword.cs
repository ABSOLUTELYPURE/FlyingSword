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
            this.transform.Translate(new Vector3(args.touchedPos.x,0,args.touchedPos.y));
            Debug.Log("-------OnMove-------"+this.transform.position);
        }
        private void OnDoubleClick(TouchEventArgs args)
        {
            Debug.Log("-------OnSkill-------");
        }
    }
}
