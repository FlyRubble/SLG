
UIBase = {}

UIComponent = {
    Image = "Image",
    RawImage = "RawImage",
    Button = "Button",
    Text = "Text",
    InputField = "InputField",
    Slider = "Slider",
    ScrollRect = "ScrollRect",
    Dropdown = "Dropdown",
    Toggle = "Toggle",
    UIItemsView = "UIItemsView",
    UIDynamic = "UIDynamic",
    Animator = "Animator",
    NumberView = "NumberView",
    RectTransform = "RectTransform",
    GridLayoutGroup = "GridLayoutGroup",
    UIToggle = "UIToggle",
}

-- 构造
function UIBase:Ctor(subClass)
    if self == subClass then
        self.super.subClass = subClass
    else
        self.subClass = subClass
    end

    self.loadCompleteEvent = function(uiBase)
        self.uiBase = uiBase
        self.uiBase.openUI = function() self.subClass:Open() end
        self.uiBase.closeUI = function() self.subClass:Close() end
        self.uiBase.onDestroy = function() self.subClass:Destroy() end
        self.gameObject = self.uiBase.gameObject
        self.transform = self.uiBase.transform
        self.subClass:InitOnce()
    end
end

---@module UIBase.lua
---@author 炎枭
---@since 2020/7/9 11:45
---@see 只调用一次，用于初始化
function UIBase:InitOnce()
    self.subClass:InitComponent()
    self.subClass:BindEvent()
    self.subClass:AddListener()
end

-- 初始化组件
function UIBase:InitComponent()end

-- 绑定事件
function UIBase:BindEvent()end

-- 添加监听事件
function UIBase:AddListener()end

-- 移除监听事件
function UIBase:RemoveListener()end

-- 打开
function UIBase:Open()
    if self.subClass.openCompleteEvent then
        self.subClass:openCompleteEvent()
    end
end

-- 关闭
function UIBase:Close()end

-- 销毁
function UIBase:Destroy()
    self.subClass:RemoveListener()
end

--移除并添加点击事件
---@param button UnityEngine.UI.Button
---@param action function
function UIBase:AddClickAndClear(button, event)
    button.onClick:RemoveAllListeners()
    button.onClick:AddListener(function() event(self.subClass, button.gameObject) end)
end

---@param eventName string
---@param event function
---@param useObj table
function UIBase:AddEvent(eventName, event, useObj)
    useObj = useObj or self.subClass
    UIEvent:AddEvent(eventName, event, useObj)
end

---@param eventName string
---@param event function
---@param useObj table
function UIBase:RemoveEvent(eventName, event, useObj)
    useObj = useObj or self.subClass
    UIEvent:RemoveEvent(eventName, event, useObj)
end

---@param eventName string
function UIBase:DispatchEvent(eventName, ...)
    UIEvent:DispatchEvent(eventName, ...)
end

---@param name string
---@param type string
function UIBase:GetComponentCS(name, type)
    return self.uiBase:GetComponent(name, type)
end

---@param type UIComponent
---@param path string
---@param root UnityEngine.GameObject
function UIBase:GetComponent(type, path, root)
    local transform = root or self.transform
    if path and #path > 0 then
        transform = self:Find(path, transform)
    end
    if not transform then
        return nil
    end
    return transform:GetComponent(type)
end

-- 查找子GameObject
---@param path string
---@param root UnityEngine.GameObject
---@return  UnityEngine.Transform
function UIBase:Find(path, root)
    if not root then
        return self.transform:Find(path)
    else
        return root.transform:Find(path)
    end
end