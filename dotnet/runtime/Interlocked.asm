global RhpLockCmpXchg32
global RhpLockCmpXchg32AVLocation
global RhpLockCmpXchg64
global RhpLockCmpXchg64AVLocation

section .text

RhpLockCmpXchg32:
	mov rax, rdx
RhpLockCmpXchg32AVLocation:
	lock cmpxchg [rdi], rsi
	ret

RhpLockCmpXchg64:
	mov rax, rdx
RhpLockCmpXchg64AVLocation:
	lock cmpxchg [rdi], rsi
	ret
