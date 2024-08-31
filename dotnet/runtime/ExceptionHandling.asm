global RhpFallbackFailFast
global RhpThrowEx

extern GetStringForFailFastReason
extern panic

section .text

; void RhpFallbackFailFast(string message, Exception? e)
RhpFallbackFailFast:
	int3
	; jmp panic ; if panic is not defined comment this line and use int3

; void RhpThrowEx(Exception e)
RhpThrowEx:
	push rdi
	mov rdi, 2
	call GetStringForFailFastReason
	mov rdi, rax
	pop rsi
	jmp RhpFallbackFailFast
