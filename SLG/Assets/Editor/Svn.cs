using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


/*
 * 我们通过TortoiseSVN的TortoiseProc.exe程序添加参数，来实现在C#或bat批处理脚本中调用SVN。
 * 举个栗子： TortoiseProc.exe /command:update /path:xxx /closeonend:0
 * /command表示SVN的操作命令
 * /path表示操作的路径，可以有多个，用*分隔
 * /closeonend用于在执行完毕后关闭对话框：
 * /closeonend:0不自动关闭对话框
 * /closeonend:1没有错误，则自动关闭对话框
 * /closeonend:2没有错误、冲突，则自动关闭对话框
 * /closeonend:3没有错误、冲突、合并，自动关闭对话框
 */
public class Svn
{
    /// <summary>
    /// Svn可执行文件路径
    /// </summary>
    const string SVN_PATH = "E:/TortoiseSVN/bin/TortoiseProc.exe";

    /// <summary>
    /// 得到选择的路径
    /// </summary>
    static List<string> selectPaths
    {
        get
        {
            List<string> pathList = new List<string>();
            string[] assetGUIDs = Selection.assetGUIDs;
            if (null != assetGUIDs)
            {
                for (int i = 0; i < assetGUIDs.Length; ++i)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
                    if (!string.IsNullOrEmpty(path))
                    {
                        pathList.Add(path);
                    }
                }
            }
            return pathList;
        }
    }

    /// <summary>
    /// 命令路径
    /// </summary>
    static string pathCommand
    {
        get
        {
            string path = string.Empty;

            List<string> pathList = selectPaths;
            for (int i = 0; i < pathList.Count; ++i)
            {
                path += pathList[i];
                if (i + 1 < pathList.Count)
                {
                    path += "*";
                }
            }

            return path;
        }
    }

    [MenuItem("Tools/Svn/Update All")]
    static void UpdateAll()
    {
        string path = System.IO.Directory.GetParent(Application.dataPath).ToString();
        if (!string.IsNullOrEmpty(path))
        {
            ExecuteSvn(string.Format("/command:update /path:{0}", path));
        }
    }

    [MenuItem("Tools/Svn/Update &u")]
    static void Update()
    {
        string path = pathCommand;
        if (!string.IsNullOrEmpty(path))
        {
            ExecuteSvn(string.Format("/command:update /path:{0}", path));
        }
    }

    [MenuItem("Tools/Svn/Revert &r")]
    static void Revert()
    {
        string path = pathCommand;
        if (!string.IsNullOrEmpty(path))
        {
            ExecuteSvn(string.Format("/command:revert -r /path:{0}", path));
        }
    }

    [MenuItem("Tools/Svn/Commit &c")]
    static void Commit()
    {
        string path = pathCommand;
        if (!string.IsNullOrEmpty(path))
        {
            ExecuteSvn(string.Format("/command:commit /path:{0}", path));
        }
    }

    /// <summary>
    /// 执行Svn
    /// </summary>
    /// <param name="command"></param>
    static void ExecuteSvn(string command)
    {
        System.Diagnostics.Process.Start(SVN_PATH, command);
    }
}
