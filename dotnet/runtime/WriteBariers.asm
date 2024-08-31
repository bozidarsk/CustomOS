global RhpAssignRef
global RhpByRefAssignRef
global RhpCheckedAssignRef
global RhpCheckedLockCmpXchg
global RhpCheckedLockCmpXchgAVLocation
global RhpCheckedXchg
global RhpCheckedXchgAVLocation
global RhBulkMoveWithWriteBarrier

extern memcpy

section .text

; void RhpAssignRef(void** address, void* obj)
RhpAssignRef:
	mov [rdi], rsi
	ret

; void RhpByRefAssignRef(void** address, void* obj)
RhpByRefAssignRef:
	mov [rdi], rsi
	ret

; void RhpCheckedAssignRef(void** address, void* obj)
RhpCheckedAssignRef:
	mov [rdi], rsi
	ret

RhpCheckedLockCmpXchg:
	mov rax, rdx
RhpCheckedLockCmpXchgAVLocation:
	lock cmpxchg [rdi], rsi
    ret

RhpCheckedXchg:
	mov rax, rsi
RhpCheckedXchgAVLocation:
	xchg [rdi], rax
	ret

RhBulkMoveWithWriteBarrier:
	jmp memcpy
