//Create By Auto
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class sword
{
    public int ID;
    public string Name;
    public sword(int _ID, string _Name)
    {
        ID = _ID;
        Name = _Name;
    }
}
public class swordConfig : ScriptableObject 
{
    [SerializeField]
    public Dictionary<int, sword> map;
    public sword GetItem(int id) 
    {                          
        if(map.ContainsKey(id))
        {                      
            return map[id];    
        }                      
        return null;           
    }                          
}
