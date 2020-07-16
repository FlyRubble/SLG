
local EventManager = class("EventManager")

function EventManager:Ctor()
    self.events = {}
end

-- 添加事件
function EventManager:AddEvent(eventName, event, useObj)
    local eventListener = self.events[eventName]
    if eventListener == nil then
        eventListener = EventListener.CreateEvent()
        self.events[eventName] = eventListener
    end
    eventListener:AddListener(event, useObj)
end

-- 移除事件
function EventManager:RemoveEvent(eventName, event, useObj)
    local eventListener = self.events[eventName]
    if eventListener ~= nil then
        eventListener:RemoveListener(event, useObj)
    end
end

-- 执行事件
function EventManager:DispatchEvent(eventName, ...)
    if self.events[eventName] then
        self.events[eventName]:Call(...)
    end
end

-- 得到事件索引
 function EventManager:GetEventIndex(eventName, event, useObj)
     local eventListener = self.events[eventName]
     if eventListener ~= nil then
         return eventListener:GetEventIndex(event, useObj)
     end
     return -1
 end

-- 是否存在事件
function EventManager:HasEvent(eventName, event, useObj)
    return self:GetEventIndex(eventName, event, useObj) > -1
end

-- 清除事件
function EventManager:ClearEvent()
    for k, v in pairs(self.events) do
        v:Clear()
    end
    self.events = {}
end

-- UI事件使用
UIEvent = EventManager.New("UIEvent")
-- MSG事件使用
MSGEvent = EventManager.New("MSGEvent")