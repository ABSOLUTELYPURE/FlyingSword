using Excel;
using System.Data;
using System.Text;

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
            AddClass(_builder, tableName.Replace("_", ""), "ScriptableObject");
            AddClass(_builder, tableName.Split('_')[0]);
        }


        private void AddClass(StringBuilder sb,string classname,string basename = "")
        {
            if(basename == "")
                sb.AppendFormat("public class {0}/n",classname);
            else
                sb.AppendFormat("public class {0}:{1}/n", classname,basename);
            sb.Append("{/n");
        }
        private void AddEnd(StringBuilder sb)
        {
            sb.Append("}/n");
        }

        private void AddNamespace(StringBuilder sb)
        {
            sb.Append("using System.Collections.Generic;/n");
            sb.Append("using UnityEngine;/n");
        }

        private void AddCreatInfo(StringBuilder sb)
        {
            sb.Append("");
        }
    }
}