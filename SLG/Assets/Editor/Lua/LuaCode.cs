using System;
using System.Collections.Generic;

public static class LuaCode
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [XLua.LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(Dictionary<string, SLG.Event.Action<string>>),
        typeof(SLG.UnityAsset.UnityAsyncAsset),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [XLua.CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(SLG.Event.Action),
        typeof(SLG.Event.Action<string>),
        typeof(SLG.Event.Action<float>),
        typeof(SLG.Event.Action<float, float>),
        typeof(SLG.Event.Action<SLG.UI.UIBase>),
        typeof(SLG.Event.Action<bool, SLG.UnityAsset.UnityAsyncAsset>),
    };
}
