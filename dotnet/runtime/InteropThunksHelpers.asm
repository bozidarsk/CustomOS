global RhGetCurrentThunkContext
global RhpAcquireThunkPoolLock
global RhpReleaseThunkPoolLock

section .bss

tls_thunkData: resq 4

section .text

RhGetCurrentThunkContext:
	mov rax, tls_thunkData
	ret

RhpAcquireThunkPoolLock:
	ret

RhpReleaseThunkPoolLock:
	ret
