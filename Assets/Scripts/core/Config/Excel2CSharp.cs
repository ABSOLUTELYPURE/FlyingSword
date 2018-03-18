using Excel;
using System.Data;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace Config
{
    public class Excel2CSharp
    {
        private static Excel2CSharp _instance;
        public static Excel2CSharp Instance
        {
            get
            {
                if (_instance == null) _instance = new Excel2CSharp();
                _instance._builder = new StringBuilder();
                return _instance;
            }
        }

        private string _suffix = ".asset";
        private string _savepath = "/Bundle/ab/";
        private StringBuilder _builder;

        public void CreateCSharpObject(string tableName, DataRowCollection collection)
        {
            _builder = new StringBuilder();
            AddCreatInfo(_builder);
            AddNamespace(_builder);           
            AddClassData(_builder, collection, tableName);
            AddClassConfig(_builder, tableName);

            System.IO.File.WriteAllText(string.Format("{0}/{1}Config.cs", ConfigStaticValue.OutConfigClassPath,tableName), _builder.ToString(), Encoding.UTF8);
        }

        private void AddClassData(StringBuilder sb, DataRowCollection collection, string classname)
        {
            if(collection.Count < 3)
            {
                Debug.LogErrorFormat("Excel Table:{0} is Error, Please Check it.",classname);
                return;
            }
            object[] propertyRow = collection[1].ItemArray;

            sb.Append("\n");
            sb.Append("[Serializable]\n");
            sb.AppendFormat("public class {0}\n", classname);
            sb.Append("{\n");
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach(object ob in propertyRow)
            {
                PropertyInfo info = new PropertyInfo(ob.ToString());
                properties.Add(info);
                sb.AppendFormat("    public {0} {1};\n",info.m_type,info.m_name);
            }

            sb.AppendFormat("    public {0}(", classname);
            for(int i = 0;i<properties.Count;i++)
            {
                if (i == 0)
                {
                    sb.AppendFormat("{0} _{1}",properties[i].m_type, properties[i].m_name);
                }
                else
                {
                    sb.AppendFormat(", {0} _{1}", properties[i].m_type, properties[i].m_name);
                }
            }
            sb.Append(")\n");

            sb.Append("    {\n");
            foreach (PropertyInfo p in properties)
            {
                sb.AppendFormat("        {0} = _{1};\n", p.m_name, p.m_name);
            }
            sb.Append("    }\n");

            sb.Append("}\n");
        }
        private void AddClassConfig(StringBuilder sb ,string classname,string basename = "")
        {
            sb.AppendFormat("public class {0}Config : {1} \n", classname, "ScriptableObject");
            sb.Append("{\n");
            sb.Append("    [SerializeField]\n");
            sb.AppendFormat("    public Dictionary<int, {0}> map;\n",classname,classname);
            sb.AppendFormat("    public {0} GetItem(int id) \n",classname);
            sb.Append(      "    {                          \n");
            sb.Append(      "        if(map.ContainsKey(id))\n");
            sb.Append(      "        {                      \n");
            sb.Append(      "            return map[id];    \n");
            sb.Append(      "        }                      \n");
            sb.Append(      "        return null;           \n");
            sb.Append(      "    }                          \n");
            sb.Append("}\n");
        }
        
        private void AddNamespace(StringBuilder sb)
        {
            sb.Append("using System.Collections.Generic;\n");
            sb.Append("using UnityEngine;\n");
            sb.Append("using System;\n");
        }

        private void AddCreatInfo(StringBuilder sb)
        {
            sb.Append("//Create By Auto\n");
        }

        public class PropertyInfo
        {
            public string m_type;
            public string m_name;

            public PropertyInfo(string value)
            {
                string[] property = value.Split('_');
                if (property.Length >= 2)
                {
                    m_type = property[1];
                    m_name = property[0];
                }
                else
                {
                    Debug.LogError("Excel Property is Error, Please Check it.");
                }
            }
        }
    }
}