
local Time = CS.UnityEngine.Time

Timer = class("Timer")

-- unscaled false 采用deltaTime计时，true 采用 unscaledDeltaTime计时,isSinceStartUp true 使用游戏起始时间计时
function Timer.Create(func, duration, loop, unscaled, isSinceStartUp, timerName)
    local timer = Timer.New()
    timer:Reset(func, duration, loop, unscaled, isSinceStartUp, timerName)
    return timer
end

function Timer:Start()
    if not self.running then
        self.running = true
        UpdateEvent:AddListener(function () self:Update() end, self)
    end
end

function Timer:Reset(func, duration, loop, unscaled, isSinceStartUp, timerName)
    self.func		= func
    self.duration 	= duration
    self.loop		= loop or 1
    self.unscaled	= unscaled or false and true
    self.isSinceStartUp = isSinceStartUp or false and true
    self.timerName  = timerName

    self.time		= duration
    self.running    = false
    self.count      = Time.frameCount + 1
    self.lastTime   = Time.realtimeSinceStartup
end

function Timer:Stop()
    self.running = false
    UpdateEvent:RemoveListener(nil, self)
end

function Timer:Update()
    if not self.running then
        return
    end
    --如果使用游戏起始时间，计算时间差，四舍五入获取秒数，倒计时为0时停止计时，每超过一秒执行一次，返回当前剩余时间
    if self.isSinceStartUp then
        local deltaTime = Time.realtimeSinceStartup - self.lastTime
        if deltaTime >= 1 and Time.frameCount > self.count then
            second = getRound(deltaTime)
            if self.loop > 0 then
                self.loop = self.loop - second
            end
            if self.loop <= 0 then
                self:Stop()
            end
            if self.func ~= nil then
                self.func(self.loop)
            end
            self.lastTime = Time.realtimeSinceStartup
        end
    else
        local delta = self.unscaled and Time.unscaledDeltaTime or Time.deltaTime
        self.time = self.time - delta

        if self.time <= 0 then
            self.func(self.loop)

            if self.loop > 0 then
                self.loop = self.loop - 1
            end

            if self.loop == 0 then
                self:Stop()
            else
                self.time = self.time + self.duration
            end
        end
    end
end

--给协同使用的帧计数timer
FrameTimer = class("FrameTimer")

function FrameTimer.Create(func, duration, loop)
    local timer = FrameTimer.New()
    timer:Reset(func, duration, loop)
    return timer
end

function FrameTimer:Reset(func, duration, loop)
    self.func = func
    self.duration = duration
    self.loop = loop or 1
    self.count = Time.frameCount + duration
    self.running = false
end

function FrameTimer:Start()
    if not self.running then
        self.running = true
        CoroutineUpdateEvent:AddListener(function () self:Update() end, self)
    end
end

function FrameTimer:Stop()
    self.running = false
    CoroutineUpdateEvent:RemoveListener(nil, self)
end

function FrameTimer:Update()
    if not self.running then
        return
    end

    if Time.frameCount >= self.count then
        self.func(self.loop)

        if self.loop > 0 then
            self.loop = self.loop - 1
        end

        if self.loop == 0 then
            self:Stop()
        else
            self.count = Time.frameCount + self.duration
        end
    end
end

CoroutineTimer = class("CoroutineTimer")

function CoroutineTimer.Create(func, duration, loop)
    local timer = CoroutineTimer.New()
    timer:Reset(func, duration, loop)
    return timer
end

function CoroutineTimer:Start()
    if not self.running then
        self.running = true
        CoroutineUpdateEvent:AddListener(function () self:Update() end, self)
    end
end

function CoroutineTimer:Reset(func, duration, loop)
    self.func		= func
    self.duration 	= duration
    self.loop		= loop or 1
    self.time		= duration
    self.running	= false
end

function CoroutineTimer:Stop()
    self.running = false
    CoroutineUpdateEvent:RemoveListener(nil, self)
end

function CoroutineTimer:Update()
    if not self.running then
        return
    end

    if self.time <= 0 then
        self.func(self.loop)

        if self.loop > 0 then
            self.loop = self.loop - 1
        end

        if self.loop == 0 then
            self:Stop()
        else
            self.time = self.time + self.duration
        end
    end

    self.time = self.time - Time.deltaTime
end