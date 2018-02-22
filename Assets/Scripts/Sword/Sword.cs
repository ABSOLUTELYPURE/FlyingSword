using UnityEngine;
using DG.Tweening;

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
            this.gameObject.transform.DOKill(false);
            this.gameObject.transform.DOMove(pos,0.5f);//.SetSpeedBased(true);
            Debug.Log("-------OnMove-------"+args.touchedPos);
            Debug.Log("-------OnMove-------"+pos);
        }
        private void OnDoubleClick(TouchEventArgs args)
        {
            Debug.Log("-------OnSkill-------");
        }
    }
}
