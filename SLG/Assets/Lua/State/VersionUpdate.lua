
local VersionUpdate = {}

function VersionUpdate:OnEnter()
    -- 加载多语言
    Config.InitLang()
    -- 加载出Loading界面
    UI.OpenUI(UI.UILoading, function ()
        self.infoCor = coroutine.start(function ()
            local versionInfo = Config.GetLang("GetVersionInfo")
            repeat
                UIEvent:DispatchEvent(UIEvent.UILoading.UpdateTips, versionInfo..".")
                coroutine.wait(0.5)
                UIEvent:DispatchEvent(UIEvent.UILoading.UpdateTips, versionInfo.."..")
                coroutine.wait(0.5)
                UIEvent:DispatchEvent(UIEvent.UILoading.UpdateTips, versionInfo.."...")
                coroutine.wait(0.5)
            until false
        end)
    end)
    -- 获取远程版本文件
    local remoteUrl = CS.System.IO.Path.Combine(CS.App.cdn..CS.App.platform, string.format(Const.REMOTEVERSION, CS.App.innerVersion))
    self.www = CS.UnityEngine.WWW(remoteUrl)
end

function VersionUpdate:OnUpdate()
    if self.www ~= nil and self.www.isDone then
        local forceUpdate = false
        if string.IsNullOrEmpty(self.www.error) then
            CS.App.Update(self.www.text)
        else
            -- 获取版本信息失败
            UI.OpenUI(UI.UITipsBox, function ()
                UIEvent:DispatchEvent(UIEvent.UITipsBox.TipsBox, {
                    text = Config.GetLang("GetVersionFail"),
                    close = false,
                    rightText = Config.GetLang("Quit"),
                    right = function ()
                        CS.UnityEngine.Application.Quit()
                        print("quit")
                    end
                })
            end)
        end
        self.www:Dispose()
        self.www = nil
    end


--[[
        {

        string[] local = App.innerVersion.Split('.');
        string[] remote = remoteVersion.ContainsKey(Const.INNERVERSION) ? remoteVersion[Const.INNERVERSION].ToString().Split('.') : new string[0];
    if (local.Length == remote.Length)
        {
        forceUpdate = !(int.Parse(local[0]) >= int.Parse(remote[0]) && int.Parse(local[1]) >= int.Parse(remote[1]));
        }
        coroutine.stop(self.infoCor)
        self.www = nil
    end

    m_www.Dispose();
    m_www = null;

    if (forceUpdate)
    {
    // 需要更新版本
    UnityEngine.Events.UnityAction cancel = () =>
    {
    UIManager.instance.CloseUI(Const.UI_TIPSBOX);
    Application.Quit();
    };
    UnityEngine.Events.UnityAction sure = () =>
    {
    UIManager.instance.CloseUI(Const.UI_TIPSBOX);
    Application.OpenURL(App.newVersionDownloadUrl);
    };
    Action open = () =>
    {
    EventListener.instance.OnEvent<Param>("ShowTipsBox", Param.Create(new object[]
    {
    "context", ConfigManager.GetLang("InstallNewVersion"),
    "cancelText", ConfigManager.GetLang("Quit"), "cancel", cancel,
    "sureText", ConfigManager.GetLang("Sure"), "sure", sure,
    }));
    };
    UIManager.instance.OpenUI(Const.UI_TIPSBOX, Param.Create(new object[] { Const.OPENUI, open }), immediate: false);
    }
    else
    {
    // 资源更新
    StateMachine.instance.OnEnter(new AssetUpdate());
    }
    }
    }
    ]]
end

function VersionUpdate:OnExit()
    print("VersionUpdate:OnExit")
end

return VersionUpdate