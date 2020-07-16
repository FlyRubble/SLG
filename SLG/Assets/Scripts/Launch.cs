using UnityEngine;
using System;
using System.Collections.Generic;
using SLG;
using SLG.Pool;
using SLG.Event;
using SLG.UnityAsset;
using System.Security.Cryptography;

[Serializable]
public struct DictPair
{
    [SerializeField]
    public string key;

    [SerializeField]
    public string value;
}

public class Launch : MonoBehaviour
{
    /// <summary>
    /// 启动信息
    /// </summary>
    [SerializeField]
    private List<DictPair> m_data = new List<DictPair>();

    /// <summary>
    /// 启动信息
    /// </summary>
    public List<DictPair> data => m_data;

    /// <summary>
    /// 启动
    /// </summary>
    void Awake()
    {
        App.Init(data);
        // 配置参数设置
        Debugger.logEnabled = App.log;
        Debugger.webLogEnabled = App.webLog;
        Debugger.logLevel = App.logLevel;
        AssetManager.instance.maxLoader = Const.MAX_LOADER;
        // 启动定时器
        Schedule.instance.Start();
        // 准备对象池
        PoolManager.instance.Create();
    }

    /// <summary>
    /// 开始
    /// </summary>
    void Start()
    {
        Lua instance = Lua.instance;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        AssetManager.instance.Update();
        Schedule.instance.Update(Time.deltaTime);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {

    }
}