local socket = require "socket"
socket.gettime()
main = {}

-- 初始化
function init(config)
	local jsonConfig = require "Common.Config"
	
	local tb = jsonConfig:Init()
	for key,value in pairs(tb) do
		config:Add(key, value)
	end
end

-- 开始
function start()
	require("Global")
	print(CS.App.currentState.getName)
	UI.OpenUI(UI.UILoading)
	local a = CS.Lua.instance.gameObject
	CS.UnityEngine.GameObject.DestroyImmediate(a)
	--local a = CS.Lua.instance.gameObject
end

-- 更新
function update()
	--print(socket.gettime())
end

-- 销毁
function destroy()
	print("destroy")
end

return main