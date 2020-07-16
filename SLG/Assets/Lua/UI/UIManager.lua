
UI = {
	UILoading = { scriptName = "UILoading.UILoading", resName = "UILoading"},
	UITipsBox = { scriptName = "UITipsBox.UITipsBox", resName = "UITipsBox"},
	UILogin = { scriptName = "UILogin", resName = "UILogin"},
}

function UI.OpenUI(uiConfig, openCompleteEvent)
	local ui = require("UI."..uiConfig.scriptName)
	ui = ui.New()
	ui.openCompleteEvent = openCompleteEvent
	return Global.LCC.OpenUI(uiConfig.resName, ui.loadCompleteEvent)
end

function UI.CloseUI(uiInfo)
	return Global.LCC.CloseUI(uiInfo.resName, nil)
end