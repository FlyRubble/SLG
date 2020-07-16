local socket = require "socket"

main = {}

-- 开始
function Start()
	local nextCmd = nil
	-- 根据下一步指示，进入指定的状态
	if nil == CS.App.nextState then
		-- 映射表添加
		CS.App.manifestMapping:Add("res/lua/Common.Const.bytes", "res/lua/Common.Const.unity3d")
		CS.App.manifestMapping:Add("res/lua/State.StateMachine.bytes", "res/lua/State.StateMachine.unity3d")
		CS.App.manifestMapping:Add("res/lua/State.Init.bytes", "res/lua/State.Init.unity3d")
		CS.App.manifestMapping:Add("res/lua/State.AssetLoad.bytes", "res/lua/State.AssetLoad.unity3d")
		nextCmd = "State.Init"
	elseif "AssetLoad" == CS.App.nextState then
		nextCmd = "State.AssetLoad"
	end
	-- 引入状态机
	require("State.StateMachine")
	StateMachine:OnEnter(require(nextCmd))

	--local a = CS.Lua.instance.gameObject
	--CS.UnityEngine.GameObject.DestroyImmediate(a)
	--CS.App.nextState = ""
	--local a = CS.Lua.instance.gameObject
end

--逻辑Update
function Update(deltaTime, unscaledDeltaTime)
	StateMachine:OnUpdate()
	UpdateEvent:Call()
end

function LateUpdate()
	--LateUpdateEvent()
	--FixedUpdateEvent()
	--Time:SetFrameCount()
end

--物理Update
function FixedUpdate(fixedDeltaTime)
	CoroutineUpdateEvent:Call()
end

-- 销毁
function Destroy()
	StateMachine:OnExit()
end

return main