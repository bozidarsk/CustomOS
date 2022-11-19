[bits 32]

section .text
	global LoadIDT

LoadIDT:
	push ebp
	mov ebp, esp

	mov eax, [ebp + 8]
	lidt [eax]

	mov esp, ebp
	pop ebp
	ret
