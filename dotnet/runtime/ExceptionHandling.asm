global RhpFallbackFailFast
global RhpThrowEx

; Unhandled exception: a managed exception was not handled before reaching unmanaged code.
; from System.RuntimeExceptionHelpers.GetStringForFailFastReason(RhFailFastReason) (dotnet/System/RuntimeExceptionHelpers.cs)
extern dotnet__Str_Unhandled_exception__a_managed_B0005D445338AB7FAB5B8E872459DDA98B8055D781EFAB667B896550D100A072
extern panic

section .text

; void RhpFallbackFailFast(string message, Exception? e)
RhpFallbackFailFast:
	int3
	cli
	hlt

; void RhpThrowEx(Exception e)
RhpThrowEx:
	mov rsi, rdi
	mov rdi, dotnet__Str_Unhandled_exception__a_managed_B0005D445338AB7FAB5B8E872459DDA98B8055D781EFAB667B896550D100A072
	jmp RhpFallbackFailFast
