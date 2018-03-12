using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Config;
using GF;

namespace Actor
{
    public class Sword:Actor
    {
        private TouchManager m_touchManager;
        private TouchEventHandler onClick;
        private TouchEventHandler onDoubleClick;

        private void Awake()
        {
            onClick = new TouchEventHandler(OnClick);
            onDoubleClick = new TouchEventHandler(OnDoubleClick);

            List<test_data.test> t = test_data.Instance.list;
            Debug.Log(t.Count);
        }

        private void Start()
        {
            m_touchManager = GF.TouchManager.GetInstance();
            m_touchManager.onOneTouchBegan += onClick;
            m_touchManager.onTwoTouchBegan += onDoubleClick;
        }
        private void OnDestroy()
        {
            if(m_touchManager)
            {
                m_touchManager.onOneTouchBegan -= onClick;
                m_touchManager.onTwoTouchBegan -= onDoubleClick;
            }
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
