global RhpGetNumThunksPerBlock
global RhpGetThunkSize
global RhpGetNumThunkBlocksPerMapping
global RhpGetThunkBlockSize
global RhpGetThunkDataBlockAddress
global RhpGetThunkStubsBlockAddress

section .data

THUNK_CODESIZE: equ 0x10 ; 3 instructions, 4 bytes each (and we also have 4 bytes of padding)
THUNK_DATASIZE: equ 0x10 ; 2 qwords

POINTER_SIZE: equ 0x08

THUNKS_MAP_SIZE: equ 0x8000

PAGE_SIZE: equ 0x1000
PAGE_SIZE_LOG2: equ 12

THUNK_POOL_NUM_THUNKS_PER_PAGE: equ 0xff

section .text

; int RhpGetNumThunksPerBlock()
RhpGetNumThunksPerBlock:
	mov rax, THUNK_POOL_NUM_THUNKS_PER_PAGE
	ret

; int RhpGetThunkSize()
RhpGetThunkSize:
	mov rax, THUNK_CODESIZE
	ret

; int RhpGetNumThunkBlocksPerMapping()
RhpGetNumThunkBlocksPerMapping:
	mov rax, (THUNKS_MAP_SIZE / PAGE_SIZE)
	ret

; int RhpGetThunkBlockSize()
RhpGetThunkBlockSize:
	mov rax, PAGE_SIZE
	ret

; nint RhpGetThunkDataBlockAddress(nint thunkStubAddress)
RhpGetThunkDataBlockAddress:
	mov rax, rdi
	mov rdi, PAGE_SIZE - 1
	not rdi
	and rax, rdi
	add rax, THUNKS_MAP_SIZE
	ret

; nint RhpGetThunkStubsBlockAddress(nint thunkDataAddress)
RhpGetThunkStubsBlockAddress:
	mov rax, rdi
	mov rdi, PAGE_SIZE - 1
	not rdi
	and rax, rdi
	sub rax, THUNKS_MAP_SIZE
	ret
