using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel;
using UnityEditor;
using UnityEngine;
using ConfigClassExport;

namespace Assets.Editor
{
    public class ConfigEditor {

        [MenuItem("游戏配置/1 -> 生成Excel对应的Class", false, 1001)]
        private static void Excel2Class()
        {
            if (CreateAllFolder())
            {
                string[] excelFiles = Directory.GetFiles("D:/Project/Git/FlyingSword/Assets/Config");
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

//                            CreateExcelClass(tables[0].TableName, tables[0].Rows);
                            foreach(DataTable table in tables)
                            {
								CreateExcelClass(table.TableName, table.Rows);
                            }
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        private static string currentExcelName = "";

        [MenuItem("游戏配置/2 -> 生成游戏中使用的配置文件", false, 1001)]
        private static void CreateScriptObject()
        {
            ConfigData data = ScriptableObject.CreateInstance<configdata>();
            ExcelAccess.SelectConfigExcel(ref data);

            if (!Directory.Exists(EditorStaticValue.ConfigScriptObjectPath))
            {
                Directory.CreateDirectory(EditorStaticValue.ConfigScriptObjectPath);
            }
            AssetDatabase.CreateAsset(data, EditorStaticValue.ConfigScriptObjectPath + "Config.asset");
            AssetDatabase.Refresh();
        }

       

        private static DataSet ReadExcel(string excelName)
        {
            string path = FilePath(excelName);
            try
            {
                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet result = excelReader.AsDataSet();
                return result;
            }
            catch (Exception ex)
            {
                string[] strArr = ex.Message.Split('\\');
                throw new Exception(string.Format("先关闭{0}", strArr[strArr.Length - 1]));
            }
        }
        private static string FilePath(string path)
        {
            return EditorStaticValue.DataFolderPath + "/" + path;
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
                string[] noteContents = fileLineContent[0].Split(new string[] {"$"}, System.StringSplitOptions.None);
                //变量的名字
                string[] VariableNameContents = fileLineContent[1].Split(new string[] {"$"}, System.StringSplitOptions.None);
                //变量的类型
                string[] TypeNameContents = fileLineContent[2].Split(new string[] {"$"}, System.StringSplitOptions.None);

                AutoCreateClass(tableName, TypeNameContents, VariableNameContents, noteContents,
                    EditorStaticValue.OutClassNameSpace);
            }
        }

        private static bool CreateAllFolder()
        {
            if (!Directory.Exists(EditorStaticValue.DataFolderPath))
            {
                Directory.CreateDirectory(EditorStaticValue.DataFolderPath);
                Debug.LogError(string.Format("把xlsx配置文件放入{0}文件夹内", EditorStaticValue.DataFolderPath));
                return false;
            }
            if (!Directory.Exists(EditorStaticValue.OutConfigClassPath))
            {
                Directory.CreateDirectory(EditorStaticValue.OutConfigClassPath);
            }
            else
            {
                DeleteAllFileInFolder(EditorStaticValue.OutConfigClassPath, "OutConfigClassPath  Class");
            }
            if (!Directory.Exists(EditorStaticValue.OutConfigExtendClassPath))
            {
                Directory.CreateDirectory(EditorStaticValue.OutConfigExtendClassPath);
            }
            return true;
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
        private static void DeleteAllFileInFolder(string path, string note)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();

            foreach (var item in files)
            {
                File.Delete(item.FullName);
            }
//            Debug.Log(string.Format("成功删除所有{0}文件！！", note));
        }
        /// <summary>
        /// 自动生成类
        /// </summary>
        /// <param name="className">类名.
        /// <param name="propertyNames">属性名称数组.
        /// <param name="noteNames">注释数组.
        /// <param name="nameSpace">命名空间.
        private static void AutoCreateClass(string className, string[] typeNames, string[] propertyNames, string[] noteNames, string nameSpace)
        {
            //声明自定义类
            CodeTypeDeclaration customerClass = new CodeTypeDeclaration(className);
            customerClass.IsClass = true;
            customerClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            customerClass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));
            CodeCompileUnit customerUnit = new CodeCompileUnit();//创建代码单元


            string ExtensionsClassName = string.Format("{0}{1}", className, "Extend");
            CodeTypeDeclaration customerExtensionsClass = new CodeTypeDeclaration(ExtensionsClassName);
            customerExtensionsClass.IsClass = true;
            customerExtensionsClass.TypeAttributes = TypeAttributes.Public;
            CodeCompileUnit customerExtensionsUnit = new CodeCompileUnit();//创建代码单元

            //创建命名空间
            if (!string.IsNullOrEmpty(nameSpace))
            {
                CodeNamespace customerNameSpace = new CodeNamespace(nameSpace);
                customerNameSpace.Imports.AddRange(new CodeNamespaceImport[] { new CodeNamespaceImport("System") });
                customerNameSpace.Types.Add(customerClass);
                customerUnit.Namespaces.Add(customerNameSpace);

                CodeNamespace customerExtensionsNameSpace = new CodeNamespace(nameSpace);
                customerExtensionsNameSpace.Types.Add(customerExtensionsClass);
                customerExtensionsUnit.Namespaces.Add(customerExtensionsNameSpace);
            }

            for (var i = 0; i < propertyNames.Length; i++)
            {
                string propertyName = propertyNames[i];
                //创建类中的变量
                //                Type.GetType(string.Format("System.{0}", typeNames[i]));
                string fieldName = propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
                CodeMemberField field = new CodeMemberField(Type.GetType(string.Format("System.{0}", typeNames[i])), fieldName);
				field.Attributes = MemberAttributes.Public;
                customerClass.Members.Add(field);

                //创建类中的属性
                CodeMemberProperty property = new CodeMemberProperty();
                property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                property.Name = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);
                property.HasGet = true;
                property.HasSet = true;
                property.Type = new CodeTypeReference(Type.GetType(string.Format("System.{0}", typeNames[i])));
                property.Comments.Add(new CodeCommentStatement(noteNames[i]));
                property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
                property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
                customerClass.Members.Add(property);
            }

            //创建代码生成类-C#
            CodeDomProvider providerCS = CodeDomProvider.CreateProvider("C#");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = true;

            //生成代码
            string outputFile = string.Format("{0}{1}.cs", EditorStaticValue.OutConfigClassPath, className);
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                providerCS.GenerateCodeFromCompileUnit(customerUnit, sw, options);
            }

            string outputExtensionsFile = string.Format("{0}{1}.cs", EditorStaticValue.OutConfigExtendClassPath, ExtensionsClassName);
            if (!File.Exists(outputExtensionsFile))
            {
                using (StreamWriter sw = new StreamWriter(outputExtensionsFile))
                {
                    providerCS.GenerateCodeFromCompileUnit(customerExtensionsUnit, sw, options);
                }
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("输出类文件", "成功输出所有类文件！！", "确定");
        }
    }

    public class EditorStaticValue  
    {  
        public static readonly string DataFolderPath = Application.dataPath + "/Config/";  
        public static readonly string OutConfigClassPath = Application.dataPath + "/Scripts/Config/ConfigDataBase/";  
        public static readonly string OutConfigExtendClassPath = Application.dataPath + "/Scripts/Config/ConfigExtend/";  
        public static readonly string OutClassNameSpace = "ConfigClassExport";  
        public static readonly string ConfigScriptObjectPath = "Assets/Resources/ConfigAsset/";  
        public static readonly int ExcelPropertyLine = 1;  
        public static readonly int ExcelValueLineStart = 3;  
  
        public static readonly string LocalConfigAssetNames = "ConfigAsset/Config";  
    }  

    public class ConfigData : ScriptableObject  
    {  
        public DataScene[] DataSceneList;  
        public DataPlayer[] DataPlayerList;  
    }  
}