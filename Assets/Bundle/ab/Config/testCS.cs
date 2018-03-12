using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    public class test_data:ScriptableObject
    {
        private static test_data _instance;
        public static test_data Instance
        {
            get
            {
                if (!_instance)
                {
                    // 如果为空，先试着从Resource中找到该对象
                    test_data[] dlist = Resources.FindObjectsOfTypeAll<test_data>();
                    if (dlist.Length > 0) _instance = dlist[0];
                }
                if (!_instance)
                {
                    // 如果仍然没有，就从默认状态中创建一个新的
                    // CreateDefaultGameState函数可以是从JSON文件中读取，并且在实例化完后指明_instance.hideFlags = HideFlags.HideAndDontSave
                    _instance = new test_data();
                }
                return _instance;
            }
        }

        public class test
        {
            public int id;
            public string name;

            public test(int _id,string _name)
            {
                id = _id;
                name = _name;
            }
        }

        [SerializeField]
        public List<test> list = new List<test>
        {
            new test(1,""),
        };  
    }
}