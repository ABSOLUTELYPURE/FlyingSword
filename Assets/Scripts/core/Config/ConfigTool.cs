using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Excel;
using UnityEditor;
using UnityEngine;
//using ConfigClassExport;

namespace Config
{
    public class ConfigStaticValue
    {
        public static readonly string DataFolderPath = Application.dataPath + "/Config/";
        public static readonly string OutConfigClassPath = Application.dataPath + "/Scripts/Config/";
        public static readonly string ConfigScriptObjectPath = Application.dataPath + "/Bundle/ab/Config/";
    }

    public class ConfigTool
    {
        [MenuItem("Tools/Config/1 -> 生成Excel对应的Class", false, 1001)]
        private static void Excel2Class()
        {
            string[] excelFiles = Directory.GetFiles(ConfigStaticValue.DataFolderPath);
            foreach (string fileName in excelFiles)
            {
                if (fileName.EndsWith("xlsx"))
                {
                    string[] pathFiled = fileName.Split('/');
                    if (pathFiled.Length > 0)
                    {
                        string xlsfileName = pathFiled[pathFiled.Length - 1];
                        DataSet dataSet = ReadExcel(xlsfileName);
                        DataTableCollection tables = dataSet.Tables;

                        foreach (DataTable table in tables)
                        {
                            Excel2CSharp.Instance.CreateCSharpObject(table.TableName, table.Rows);
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        private static string currentExcelName = "";

        private static void CreateScriptObject(string name , DataRowCollection collection)
        {
            var cof = ScriptableObject.CreateInstance(string.Format("{0}Config", name));
            //cof = cof as Type.GetType("swordConfig");
            cof = cof as swordConfig;
            for (int i = 2;i<collection.Count;i++)
            {
                
            }
        }

        private static DataSet ReadExcel(string excelName)
        {
            string path = ConfigStaticValue.DataFolderPath + "/" + excelName;
            try
            {
                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet result = excelReader.AsDataSet();
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                string[] strArr = ex.Message.Split('\\');
                throw new Exception(string.Format("先关闭{0}", strArr[strArr.Length - 1]));
            }
        }
        private static void CreateExcelClass(string tableName, DataRowCollection collection)
        {
            object[] propertyRow = collection[0].ItemArray;

            List<string> allColumnStrList = new List<string>();

            allColumnStrList.Add(GetColumnString(propertyRow));
            for (int row = 1; row < collection.Count; row++)
            {
                allColumnStrList.Add(GetColumnString(collection[row].ItemArray));
            }

            string[] fileLineContent = allColumnStrList.ToArray();
            if (fileLineContent.Length > 0)
            {
                //注释的名字
                string[] noteContents = fileLineContent[0].Split(new string[] { "$" }, System.StringSplitOptions.None);
                //变量的名字
                string[] VariableNameContents = fileLineContent[1].Split(new string[] { "$" }, System.StringSplitOptions.None);
                //变量的类型
                string[] TypeNameContents = fileLineContent[2].Split(new string[] { "$" }, System.StringSplitOptions.None);

                //AutoCreateClass(tableName, TypeNameContents, VariableNameContents, noteContents,
                //    EditorStaticValue.OutClassNameSpace);
            }
        }
        private static string GetColumnString(object[] rowData)
        {
            List<string> ss = new List<string>();

            for (int i = 0; i < rowData.Length; i++)
            {
                ss.Add(rowData[i].ToString());
            }
            return string.Join("$", ss.ToArray());
        }
    }
}