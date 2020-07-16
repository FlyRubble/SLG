
function string.IsNullOrEmpty(s)
    return nil == s or "" == s
end

function IsNull(obj)
    return nil == obj or obj:Equals(nil)
end

function xcall(func)
    local flag, traceMsg = true, nil
    xpcall(func, function (args)
        flag = false
        traceMsg = args.."\n"..debug.traceback()
    end)
    return flag, traceMsg
end