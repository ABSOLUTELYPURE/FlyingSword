using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class test
{
    public int id;
    public string name;

    public test(int _id, string _name)
    {
        id = _id;
        name = _name;
    }
}
public class test_data : ScriptableObject
{
    [SerializeField]
    public Dictionary<int, test> map;

    [SerializeField]
    public List<test> testList;

    public test GetItem(int id)
    {
        if(map.ContainsKey(id))
        {
            return map[id];
        }
        return null;
    }
}