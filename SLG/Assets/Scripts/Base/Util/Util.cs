using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SLG.Event;

namespace SLG
{
    public class Util
    {
        /// <summary>
        /// 目标平台标记
        /// </summary>
        public static string GetPlatformSign()
        {
            string platform = "Windows";
#if UNITY_ANDROID
            platform = "Android";
#elif UNITY_IOS
            platform = "iOS";
#endif
            return platform;
        }

        /// <summary>
        /// 得到一致性路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetUniformityPath(string filePath)
        {
            return filePath.Replace(@"\", @"/").Replace(@"\\", @"/");
        }

        /// <summary>
        /// 写入字节到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 得到MD5值
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetMD5(byte[] bytes)
        {
            string md5Value = string.Empty;
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytes);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                md5Value = sb.ToString();
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
            return md5Value;
        }

        /// <summary>
        /// 得到MD5值
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMD5(string file)
        {
            if (File.Exists(file))
            {
                return GetMD5(File.ReadAllBytes(file));
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到持久化字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        /// <summary>
        /// 设置持久化字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }




















        /// <summary>
        /// 查找对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Find<T>(Transform parent, string name) where T : Component
        {
            if (null == parent)
            {
                Debugger.LogError("The parent is null!");
                return null;
            }

            Transform child = parent.Find(name);
            if (null != child)
            {
                T t = child.GetComponent<T>();
                if (null == t)
                {
                    Debugger.LogErrorFormat("Get component failed! type = {0}", typeof(T));
                }
                return t;
            }
            else
            {
                Debugger.LogErrorFormat("Find child failed! name = {0}", name);
            }

            return null;
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="text"></param>
        public static void SetText(Transform transform, string text)
        {
            if (null == transform)
            {
                Debugger.LogError("The transform is null!");
            }
            if (null == text)
            {
                Debugger.LogError("The text is null!");
            }

            Text t = transform.GetComponent<Text>();
            if (null == t)
            {
                Debugger.LogErrorFormat("Get component failed! type = {0}", typeof(Text));
            }
            else
            {
                t.text = text;
            }
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="text"></param>
        public static void SetText(GameObject gameObject, string text)
        {
            if (null == gameObject)
            {
                Debugger.LogError("The gameObject is null!");
            }

            SetText(gameObject.transform, text);
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public static void SetText(Transform parent, string name, string text)
        {
            if (null == parent)
            {
                Debugger.LogError("The parent is null!");
            }
            if (null == text)
            {
                Debugger.LogError("The text is null!");
            }

            Transform child = parent.Find(name);
            if (null != child)
            {
                Text t = child.GetComponent<Text>();
                if (null == t)
                {
                    Debugger.LogErrorFormat("Get component failed! type = {0}", typeof(Text));
                }
                else
                {
                    t.text = text;
                }
            }
            else
            {
                Debugger.LogErrorFormat("Find child failed! name = {0}", name);
            }
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public static void SetText(GameObject gameObject, string name, string text)
        {
            if (null == gameObject)
            {
                Debugger.LogError("The gameObject is null!");
            }

            SetText(gameObject.transform, name, text);
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="active"></param>
        public static void SetActive(Transform transform, bool active)
        {
            if (null == transform)
            {
                Debugger.LogError("The transform is null!");
            }

            transform.gameObject.SetActive(active);
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="active"></param>
        public static void SetActive(GameObject gameObject, bool active)
        {
            if (null == gameObject)
            {
                Debugger.LogError("The gameObject is null!");
            }

            gameObject.SetActive(active);
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="active"></param>
        public static void SetActive(Transform parent, string name, bool active)
        {
            if (null == parent)
            {
                Debugger.LogError("The parent is null!");
            }

            Transform child = parent.Find(name);
            if (null != child)
            {
                child.gameObject.SetActive(active);
            }
            else
            {
                Debugger.LogErrorFormat("Find child failed! name = {0}", name);
            }
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <param name="active"></param>
        public static void SetActive(GameObject gameObject, string name, bool active)
        {
            if (null == gameObject)
            {
                Debugger.LogError("The gameObject is null!");
            }

            SetActive(gameObject.transform, name, active);
        }

        /// <summary>
        /// 设置按钮事件
        /// </summary>
        /// <param name="button"></param>
        /// <param name="action"></param>
        /// <param name="removeAllListeners"></param>
        public static void SetButton(Button button, Action action, bool removeAllListeners = true)
        {
            if (null == button)
            {
                Debugger.LogError("The button is null!");
            }
            if (null == action)
            {
                Debugger.LogError("The action is null!");
            }

            if (removeAllListeners)
            {
                button.onClick.RemoveAllListeners();
            }
            button.onClick.AddListener(() => { action(); });
        }

        /// <summary>
        /// 设置按钮事件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="removeAllListeners"></param>
        public static void SetButton(Transform parent, string name, Action action, bool removeAllListeners = true)
        {
            if (null == parent)
            {
                Debugger.LogError("The parent is null!");
            }

            Transform child = parent.Find(name);
            if (null != child)
            {
                Button t = child.GetComponent<Button>();
                SetButton(t, action, removeAllListeners);
            }
            else
            {
                Debugger.LogErrorFormat("Find child failed! name = {0}", name);
            }
        }

        /// <summary>
        /// 设置按钮事件
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="removeAllListeners"></param>
        public static void SetButton(GameObject gameObject, string name, Action action, bool removeAllListeners = true)
        {
            if (null == gameObject)
            {
                Debugger.LogError("The gameObject is null!");
            }

            SetButton(gameObject.transform, name, action, removeAllListeners);
        }

        /// <summary>
        /// 设置按钮事件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="action"></param>
        /// <param name="removeAllListeners"></param>
        public static void SetButton(Transform transform, Action action, bool removeAllListeners = true)
        {
            if (null == transform)
            {
                Debugger.LogError("The transform is null!");
            }

            Button t = transform.GetComponent<Button>();
            SetButton(t, action, removeAllListeners);
        }

        /// <summary>
        /// 设置按钮事件
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="action"></param>
        /// <param name="removeAllListeners"></param>
        public static void SetButton(GameObject gameObject, Action action, bool removeAllListeners = true)
        {
            if (null == gameObject)
            {
                Debugger.LogError("The gameObject is null!");
            }

            Button t = gameObject.GetComponent<Button>();
            SetButton(t, action, removeAllListeners);
        }

        /// <summary>
        /// 得到多语言文本
        /// </summary>
        /// <param name="id"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string GetLanguage(string id, string defaultText)
        {
            return defaultText;
        }
    }
}
