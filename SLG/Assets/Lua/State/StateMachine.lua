
StateMachine = {}

function StateMachine:OnEnter(state)
    if nil ~= self.current then
        self.current:OnExit()
    end
    self.current = state
    self.current:OnEnter()
end

function StateMachine:OnUpdate()
    self.current:OnUpdate()
end

function StateMachine:OnExit()
    self.current:OnExit()
end