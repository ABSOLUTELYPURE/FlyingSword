using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager:MonoBehaviour
{
    #region Instance
    private static DataManager m_inst;
    public static DataManager Instance
    {
        get
        {
            if (m_inst == null)
            {
                GameObject go = new GameObject("DataManager");
                m_inst = go.AddComponent<DataManager>();
                m_inst.Init();
                DontDestroyOnLoad(go);
            }
            return m_inst;
        }
    }
    #endregion

    private bool m_isInited = false;

    private void Init()
    {
        if(!m_isInited)
        {
            //todo
        }
    }
}
