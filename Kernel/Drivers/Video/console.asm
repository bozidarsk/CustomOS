global getconsolebuffer

extern CONSOLEBUFFER

section .framebuffer

section .text

getconsolebuffer:
	mov rax, CONSOLEBUFFER
	ret
