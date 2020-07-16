
local Event = class("Event")
Event.__index = Event

Event.State = {
    ADD = "Add",
    REMOVE = "Remove",
    RUNNING = "Running",
}

function Event:Ctor()
    self.list = {}
    self.calling = false
    self.clear = false
    self.temp = {}
end

-- 避免了重复事件的监听
function Event:AddListener(event, useObj)
    local pos, handle = self:GetEventIndex(event, useObj)
    if handle then
        if handle.state == Event.State.REMOVE then
            handle.state = Event.State.ADD
        end
    else
        table.insert(self.list, {
            event = event,
            useObj = useObj,
            state = self.calling and Event.State.ADD or Event.State.RUNNING
        })
    end
end

function Event:RemoveListener(event, useObj)
    local pos, handle = self:GetEventIndex(event, useObj)
    if pos > -1 then
        if self.calling then
            handle.state = Event.State.REMOVE
        else
            table.remove(self.list, pos)
        end
    end
end

function Event:GetEventIndex(event, useObj)
    for i, v in ipairs(self.list) do
        if event == v.event or (nil ~= useObj and useObj == v.useObj) then
            return i, v
        end
    end
    return -1
end

function Event:Count()
    return #self.list
end

function Event:Clear()
    if self.calling then
        self.clear = true
    else
        self.list = {}
    end
end

function Event:Call(...)
    local args = {...}
    local flag, traceMsg = xcall(function ()
        self.calling = true
        self.temp = {}
        for i, v in ipairs(self.list) do
            if (v.state == Event.State.RUNNING) then
                v.event(table.unpack(args))
            elseif v.state == Event.State.ADD then
                v.state = Event.State.RUNNING
            elseif v.state == Event.State.REMOVE then
                table.insert(self.temp,1, i)
            end
        end
        for i, v in ipairs(self.temp) do
            table.remove(self.list, v)
        end
        if self.clear then
            self.list = {}
            self.clear = false
        end
        self.calling = false
    end)
    if not flag then
        debug.LogError(traceMsg)
    end
end

Event.__call = function(self, ...)
    self:Call()
end

function Event.CreateEvent(ab)
    return Event.New()
end

-- 通用事件
EventListener = Event.CreateEvent()
-- 更新事件
UpdateEvent = EventListener.CreateEvent()
LateUpdateEvent = EventListener.CreateEvent()
FixedUpdateEvent = EventListener.CreateEvent()
CoroutineUpdateEvent = EventListener.CreateEvent()