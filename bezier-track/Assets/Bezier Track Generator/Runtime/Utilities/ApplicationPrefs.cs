using System.IO;
using UnityEngine;

namespace ptl.bezier
{
    public class ApplicationPrefs
    {
        public static void SetObject(string key, object obj)
        {
            bool flag;

#if UNITY_EDITOR
            flag = true;
#else
        flag = false;
#endif
            SaveToFile(key, JsonUtility.ToJson(obj, flag));
        }

        public static T GetObject<T>(string key)
        {
            return JsonUtility.FromJson<T>(ReadFromFile(key));
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) == 1;
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void DeleleKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static bool HasObject(string key)
        {
            return File.Exists(GetFilePath(key));
        }

        public static void DeleteObject(string key)
        {
            File.Delete(GetFilePath(key));
        }

        private static void SaveToFile(string fileName, string fileContent)
        {
            File.WriteAllText(GetFilePath(fileName), fileContent);
        }

        private static string ReadFromFile(string fileName)
        {
            string content;

            if (File.Exists(GetFilePath(fileName)))
                content = File.ReadAllText(GetFilePath(fileName));
            else
                content = string.Empty;

            return content;
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}