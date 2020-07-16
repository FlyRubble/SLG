
Config = {}

local json = require "common.json"

-- 初始化
function Config.InitLang()
	Config.lang = {}
	Global.LCC.LoadConf("lang", function (result, asset)
		local lang = json.decode(asset.text)
		for i, v in ipairs(lang["lang"]) do
			Config.lang[v.uuid] = v.value
		end
	end, false)
end

function Config.GetLang(uuid)
	return Config.lang[uuid] or "nil"
end

-- 初始化
function Config.Init()

	local tb = {}
	tb["lang"] = function (text)
		Config.lang = json.decode(text)
	end
	
	return tb
end