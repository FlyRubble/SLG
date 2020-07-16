function class(className, super)
    super = super or {}
    local superType = type(super)
    local cls = { typeName = className }
    -- 基类限定
    if superType ~= "function" and superType ~= "table" then
        superType = nil
        super = nil
    end
    -- 复制基类方法
    if superType == "function" then
        setmetatable(cls, { __index = super })
        cls.super = super
    elseif superType == "table" then
        cls.super = {}
        for k, v in pairs(super) do
            cls.super[k] = v
        end
        setmetatable(cls, { __index = cls.super })
    end
    -- 克隆
    local function Clone(c)
        local tb = {}
        for k, v in pairs(c) do
            if k == "super" then
                tb[k] = Clone(v)
                setmetatable(tb, { __index = tb[k]})
            else
                tb[k] = v
            end
        end
        return tb
    end
    
    function cls.new(className, ...)
        local instance = Clone(cls)
        if nil ~= instance.Ctor then
            instance:Ctor(instance, ...)
        end
        if className then
            instance.typeName = className
        end
        return instance
    end

    function cls.New(className, ...)
        return cls.new(...)
    end
    
    return cls
end