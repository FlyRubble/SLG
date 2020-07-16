
local create = coroutine.create
local running = coroutine.running
local resume = coroutine.resume
local yield = coroutine.yield
local unpack = table.unpack

local cmap = {}
setmetatable(cmap, { __mode = "kv" })
local frameTimerPool = {}
local coroutineTimerPool = {}

function coroutine.start(f, ...)
    local c = create(f)

    if running() == nil then
        local flag, msg = resume(c, ...)

        if not flag then
            debug.LogError(debug.traceback(c, msg))
        end
    else
        local args = {...}
        local timer = nil

        local action = function()
            cmap[c] = nil
            timer.func = nil
            local flag, msg = resume(c, unpack(args))
            table.insert(frameTimerPool, timer)

            if not flag then
                timer:Stop()
                debug.LogError(debug.traceback(c, msg))
            end
        end

        if #frameTimerPool > 0 then
            timer = table.remove(frameTimerPool)
            timer:Reset(action, 0, 1)
        else
            timer = FrameTimer.Create(action, 0, 1)
        end

        cmap[c] = timer
        timer:Start()
    end

    return c
end

function coroutine.wait(t, c, ...)
    local args = {...}
    c = c or running()
    local timer = nil

    local action = function()
        cmap[c] = nil
        timer.func = nil
        local flag, msg = resume(c, unpack(args))
        table.insert(coroutineTimerPool, timer)

        if not flag then
            timer:Stop()
            debug.LogError(debug.traceback(c, msg))
            return
        end
    end

    if #coroutineTimerPool > 0 then
        timer = table.remove(coroutineTimerPool)
        timer:Reset(action, t or 1, 1)
    else
        timer = CoroutineTimer.Create(action, t or 1, 1)
    end

    cmap[c] = timer
    timer:Start()
    return yield()
end

function coroutine.step(t, c, ...)
    local args = {...}
    c = c or running()
    local timer = nil

    local action = function()
        cmap[c] = nil
        timer.func = nil
        local flag, msg = resume(c, unpack(args))
        table.insert(frameTimerPool, timer)

        if not flag then
            timer:Stop()
            debug.LogError(debug.traceback(c, msg))
            return
        end
    end

    if #frameTimerPool > 0 then
        timer = table.remove(frameTimerPool)
        timer:Reset(action, t or 1, 1)
    else
        timer = FrameTimer.Create(action, t or 1, 1)
    end

    cmap[c] = timer
    timer:Start()
    return yield()
end

function coroutine.www(www, c)
    c = c or running()
    local timer = nil

    local action = function()
        if not www.isDone then
            return
        end

        cmap[c] = nil
        timer:Stop()
        timer.func = nil
        local flag, msg = resume(c)
        table.insert(frameTimerPool, timer)

        if not flag then
            debug.LogError(debug.traceback(c, msg))
            return
        end
    end

    if #frameTimerPool > 0 then
        timer = table.remove(frameTimerPool)
        timer:Reset(action, 1, -1)
    else
        timer = FrameTimer.Create(action, 1, -1)
    end
    cmap[c] = timer
    timer:Start()
    return yield()
end

function coroutine.stop(c)
    local timer = cmap[c]
    if timer ~= nil then
        cmap[c] = nil
        timer:Stop()
    end
end

-- 停止所有协程
function coroutine.stopAll()
    for i, v in pairs(cmap) do
        cmap[i] = nil
        v:Stop()
    end
end