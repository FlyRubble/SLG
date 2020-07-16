using XLua;
using System.Collections.Generic;
using SLG.UI;
using SLG.Event;
using SLG.Singleton;
using SLG.UnityAsset;
using SLG;
using UnityEngine;

public class Lua : MonoBehaviourSingleton<Lua>
{
    #region Variable
    /// <summary>
    /// Lua虚拟机
    /// </summary>
    private LuaEnv m_luaEnv = null;

    /// <summary>
    /// 表
    /// </summary>
    private LuaTable m_luaTable = null;

    /// <summary>
    /// 开始
    /// </summary>
    private Action m_start = null;

    /// <summary>
    /// 更新
    /// </summary>
    private Action<float, float> m_update = null;

    /// <summary>
    /// 延迟更新
    /// </summary>
    private Action m_lateUpdate = null;

    /// <summary>
    /// 物理更新
    /// </summary>
    private Action<float> m_fixedUpdate = null;

    /// <summary>
    /// 销毁
    /// </summary>
    private Action m_destroy = null;
    #endregion

    #region Function
    /// <summary>
    /// 开始
    /// </summary>
    private void Awake()
    {
        m_luaEnv = new LuaEnv();
        m_luaEnv.AddLoader(Loader);
        m_luaTable = m_luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_luaEnv.NewTable();
        meta.Set("__index", m_luaEnv.Global);
        m_luaTable.SetMetaTable(meta);
        meta.Dispose();

        m_luaTable.Set("self", this);
        m_luaEnv.DoString("require('Main')", "XLua", m_luaTable);

        m_start = m_luaTable.Get<Action>("Start");
        m_update = m_luaTable.Get<Action<float, float>>("Update");
        m_lateUpdate = m_luaTable.Get<Action>("LateUpdate");
        m_fixedUpdate = m_luaTable.Get<Action<float>>("FixedUpdate");
        m_destroy = m_luaTable.Get<Action>("Destroy");
    }

    /// <summary>
    /// 开始
    /// </summary>
    private void Start()
    {
        if (m_start != null)
        {
            m_start();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        m_luaEnv.Tick();
        m_update(Time.deltaTime, Time.unscaledDeltaTime);
    }

    /// <summary>
    /// 延迟更新
    /// </summary>
    private void LateUpdate()
    {
        m_lateUpdate();
    }

    /// <summary>
    /// 物理更新
    /// </summary>
    private void FixedUpdate()
    {
        m_fixedUpdate(Time.fixedDeltaTime);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void Destroy()
    {
        if (m_destroy != null)
        {
            m_destroy();
        }
        m_start = null;
        m_update = null;
        m_lateUpdate = null;
        m_fixedUpdate = null;
        m_destroy = null;

        m_luaEnv.Dispose();
    }

    /// <summary>
    /// Lua加载器
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private byte[] Loader(ref string fileName)
    {
        byte[] result = null;

        fileName = fileName.Replace(".", "/");
        AssetManager.instance.LoadLua(fileName, (bResult, bytes) => {
            result = bytes;
        }, async: false);

        return result;
    }
    #endregion
}
