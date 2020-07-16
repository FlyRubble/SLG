LogLevel = {
    Log = Global.Debug.LogLevel.Log,
    Warning = Global.Debug.LogLevel.Warning,
    Exception = Global.Debug.LogLevel.Exception,
    Error = Global.Debug.LogLevel.Error,
    None = Global.Debug.LogLevel.None,
}

function debug.LogEnabled(enabled)
    Global.Debug.logEnabled = enabled and true or false
end

function debug.WebLogEnabled(enabled)
    Global.Debug.webLogEnabled = enabled and true or false
end

function debug.LogLevel(logLevel)
    logLevel = logLevel or LogLevel.None
    Global.Debug.logLevel = logLevel
end

function debug.Log(message)
    Global.Debug.Log(message)
end

function debug.LogFormat(format, ...)
    Global.Debug.Log(string.format(format, ...))
end

function debug.LogError(message)
    Global.Debug.LogError(message)
end

function debug.LogErrorFormat(format, ...)
    Global.Debug.LogErrorFormat(string.format(format, ...))
end

function debug.LogWarning(message)
    Global.Debug.LogWarning(message)
end

function debug.LogWarningFormat(format, ...)
    Global.Debug.LogWarningFormat(string.format(format, ...))
end