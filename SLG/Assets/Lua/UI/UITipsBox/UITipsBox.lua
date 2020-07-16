
local UITipsBox = class("UITipsBox", UIBase)

-- 初始化组件
function UITipsBox:InitComponent()
    self.tipsText = self:GetComponentCS("TipsText", UIComponent.Text)
    self.closeBtn = self:GetComponentCS("CloseBtn", UIComponent.Button)
    self.leftText = self:GetComponentCS("LeftBtn", UIComponent.Text)
    self.leftBtn = self:GetComponentCS("LeftBtn", UIComponent.Button)
    self.rightText = self:GetComponentCS("RightText", UIComponent.Text)
    self.rightBtn = self:GetComponentCS("RightBtn", UIComponent.Button)
end

-- 绑定事件
function UITipsBox:BindEvent()

end

-- 添加监听事件
function UITipsBox:AddListener()
    self:AddEvent(UIEvent.UITipsBox.TipsBox, function (tb) self:UpdateShow(tb) end)
end

-- 移除监听事件
function UITipsBox:RemoveListener()
    self:RemoveEvent(UIEvent.UITipsBox.TipsBox)
end

-- 更新显示
function UITipsBox:UpdateShow(tb)
    self.tipsText.text = tb.text
    -- 关闭
    if tb.close then
        self.closeBtn.gameObject:SetActive(true)
        if tb.close == true then
            self:AddClickAndClear(self.closeBtn, function ()
                UI.CloseUI(UI.UITipsBox)
            end)
        else
            self:AddClickAndClear(self.closeBtn, tb.close)
        end
    else
        self.closeBtn.gameObject:SetActive(false)
    end
    -- 左按钮
    if tb.left then
        self.leftBtn.gameObject:SetActive(true)
        self.leftText.text = tb.leftText
        self:AddClickAndClear(self.leftBtn, tb.left)
    else
        self.leftBtn.gameObject:SetActive(false)
    end
    -- 右按钮
    if tb.right then
        self.rightBtn.gameObject:SetActive(true)
        self.rightText.text = tb.rightText
        self:AddClickAndClear(self.rightBtn, tb.right)
    else
        self.rightBtn.gameObject:SetActive(false)
    end
end

-- 打开
function UITipsBox:Open()
    self.super:Open()
end

-- 关闭
function UITipsBox:Close()
end

-- 销毁
function UITipsBox:Destroy()
    self.super:Destroy()
end

return UITipsBox