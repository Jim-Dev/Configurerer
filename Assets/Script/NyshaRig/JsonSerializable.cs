using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using UnityEngine;

namespace Assets.Script.NyshaRig
{
    public class JsonSerializable
    {

        public const string DEFAULT_ASSET_PATH = "Assets/Animations/JsonAssets/";
        private const string DEFAULT_FILE_EXTENSION = ".json";



        public JsonSerializable()
        {

        }

        public string ToJson()
        {
            return ToJson(true);
        }
        public string ToJson(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        public virtual JsonSerializable FromJson(string JsonString)
        {
            return JsonUtility.FromJson<JsonSerializable>(JsonString);
        }

        public virtual void SaveToFile(string fileName, string directoryPath)
        {
            string filePath = directoryPath + fileName + DEFAULT_FILE_EXTENSION;

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    string JSonContent = ToJson(true);
                    writer.Write(JSonContent);
                }
            }
        }

        public string GetJsonString()
        {
            return GetJsonString(false);
        }

        public string GetJsonString(bool prettyPrint)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }

        public static string LoadJsonFile(string directoryPath,string fileName)
        {
            string jsonString = string.Empty;

            //string filePath = string.Format(@"Assets\Animations\JsonAssets\{0}.json", fileName);
            string filePath = directoryPath + fileName + DEFAULT_FILE_EXTENSION;

            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        jsonString = reader.ReadToEnd();
                    }
                }
                
            }
            return jsonString;
        }

        //public string GetFullPath(string DirectoryPath,string fileName)
        //{
        //    return DirectoryPath + fileName + DEFAULT_FILE_EXTENSION;
        //}

    }
}
