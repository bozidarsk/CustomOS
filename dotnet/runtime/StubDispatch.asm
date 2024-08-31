global RhpInitialInterfaceDispatch
global RhpInitialDynamicInterfaceDispatch

extern RhpCidResolve

RhpInitialInterfaceDispatch:
RhpInitialDynamicInterfaceDispatch:
	cmp byte [rdi], 0
	jmp RhpInterfaceDispatchSlow

RhpInterfaceDispatchSlow:
	mov rsi, r10
	jmp RhpCidResolve
