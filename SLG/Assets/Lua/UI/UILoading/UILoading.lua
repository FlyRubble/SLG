
local UILoading = class("UILoading", UIBase)

-- 初始化组件
function UILoading:InitComponent()
    self:GetComponentCS("Agreement", UIComponent.Text).text = Config.GetLang("CozyTips")
    self.tips = self:GetComponentCS("Tips", UIComponent.Text)
    self.slider = self:GetComponentCS("Slider", UIComponent.Slider)
    self.details = self:GetComponentCS("Details", UIComponent.Text)
    self.tips.text = ""
    self.slider.value = 0
    self.details.text = ""
end

-- 绑定事件
function UILoading:BindEvent()

end

-- 添加监听事件
function UILoading:AddListener()
    self:AddEvent(UIEvent.UILoading.UpdateTips, function (tips) self.tips.text = tips end)
end

-- 移除监听事件
function UILoading:RemoveListener()
    self:RemoveEvent(UIEvent.UILoading.UpdateTips)
end

-- 打开
function UILoading:Open()
    self.super:Open()
end

-- 关闭
function UILoading:Close()
end

-- 销毁
function UILoading:Destroy()
    self.super:Destroy()
end

return UILoading