
local Init = {}

function Init:OnEnter()
    require("Common.Const")
    -- 沙盒版本检测
    if CS.App.innerVersion ~= CS.SLG.Util.GetString(Const.SANDBOX_VERSION) then
        -- 清空文件夹，以便重新解压
        if CS.System.IO.Directory.Exists(CS.SLG.PathUtil.persistentDataPath) then
            CS.System.IO.Directory.Delete(CS.SLG.PathUtil.persistentDataPath, true)
        end
        CS.SLG.Util.SetString(Const.SANDBOX_VERSION, CS.App.innerVersion)
    end
    -- 获取清单数据
    local asset = CS.LuaCallCS.AssetBundleLoad(Const.MANIFESTFILE)
    if asset.error == nil or asset.error == "" then
        CS.LuaCallCS.UpdateManifestConfig(asset.text)
        CS.LuaCallCS.UnloadAsset(asset.asyncAsset, true)
    end
    -- 获取清单映射数据
    asset = CS.LuaCallCS.AssetBundleLoad(Const.MANIFESTFILEMAPPING)
    if asset.error == nil or asset.error == "" then
        CS.LuaCallCS.UpdateManifestMappingConfig(asset.text)
        CS.LuaCallCS.UnloadAsset(asset.asyncAsset, true)
    end

    require("Global")
    StateMachine:OnEnter(require("State.VersionUpdate"))
end

function Init:OnUpdate()

end

function Init:OnExit()

end

return Init