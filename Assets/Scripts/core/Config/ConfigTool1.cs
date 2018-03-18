using UnityEngine;
using System.Collections.Generic;
using Config;
using UnityEditor;

public class ConfigTool1
{






















    [MenuItem("Tools/Config/CreateAsset")]
    public static void CreateAsset()
    {
        test_data cof = ScriptableObject.CreateInstance<test_data>();

        cof.map = new Dictionary<int, test>();

        cof.map.Add(1, new test(1, "1"));
        cof.map.Add(2, new test(2, "2"));
        cof.map.Add(3, new test(3, "3"));

        cof.testList = new List<test>()
        {
            new test(1, "1"),
            new test(2, "2"),
            new test(3, "3")
        };

        AssetDatabase.CreateAsset(cof, "Assets/Bundle/ab/Config/test_data.asset");
    }

    [MenuItem("Tools/Config/LoadAsset")]
    public static void LoadAsset()
    {
        test_data cof = AssetDatabase.LoadAssetAtPath<test_data>("Assets/Bundle/ab/Config/test_data.asset");
        Debug.Log(cof.map.Count);
    }
}
