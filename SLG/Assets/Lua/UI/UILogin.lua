require("UI.UIBase")

local UILogin = class("UILogin", UIBase)

-- 只调用一次，用于初始化
function UILogin:InitOnce()
    UILogin.super:InitOnce()
    print("UITipsBox InitOnce")
end

-- 打开
function UILogin:Open()
    UILogin.super:Open()
    print("UILogin.Open")
end

-- 关闭
function UILogin:Close()
    print("UILogin.Close")
end

-- 销毁
function UILogin:Destroy()
    print("UILogin.Destroy")
end

return UILogin