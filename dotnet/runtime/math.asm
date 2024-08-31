global $abs
global floor
global ceil
global frac
global fmod
global sqrt
global cbrt
global log2
global log10
global sin
global cos
global tan
global asin
global acos
global atan
global atan2
global sinh
global cosh
global tanh
global asinh
global acosh
global atanh
global fma
global modf

extern scalbn
extern exp
extern log
extern pow

section .rodata

align 16
zero: dq 0x0000000000000000, 0
one: dq 0x3ff0000000000000, 0
negone: dq 0xbff0000000000000, 0
half: dq 0x3fe0000000000000, 0
pi: dq 0x400921fb54442d18, 0
tau: dq 0x401921fb54442d18, 0
nan: dq 0x7ff8000000000000, 0

section .text

retnan:
	movsd xmm0, qword [nan]
	ret

; double modf(double x, out double interger)
modf:
	cvttsd2si rax, xmm0
	cvtsi2sd xmm1, rax
	movsd qword [rdi], xmm1
	subsd xmm0, xmm1
	ret

; double fma(double x, double y, double z)
fma:
	vfmadd132sd xmm0, xmm2, xmm1
	ret

; double $abs(double x)
$abs:
	movq rax, xmm0
	mov rbx, 0x7fffffffffffffff
	and rax, rbx
	movq xmm0, rax
	ret

; double floor(double x)
floor:
	movsd xmm1, xmm0
	cvttsd2si rax, xmm0
	cvtsi2sd xmm0, rax
	comisd xmm1, qword [zero]
	jae .ret
	subsd xmm0, qword [one]
	.ret:
	ret

; double ceil(double x)
ceil:
	movsd xmm1, xmm0
	call floor
	comisd xmm0, xmm1
	je .ret
	addsd xmm0, qword [one]
	comisd xmm0, qword [zero]
	jne .ret
	mov rax, 0x8000000000000000
	movq xmm0, rax
	.ret:
	ret

; double frac(double x)
frac:
	push rbp
	mov rbp, rsp
	sub rsp, 8

	movsd qword [rbp - 8], xmm0
	call floor
	movsd xmm1, qword [rbp - 8]
	subsd xmm1, xmm0
	movsd xmm0, xmm1

	add rsp, 8
	pop rbp
	ret

; double fmod(double a, double b)
fmod:
	push rbp
	mov rbp, rsp
	sub rsp, 24

	movsd qword [rbp - 8], xmm0
	movsd qword [rbp - 16], xmm1

	divsd xmm0, xmm1
	call $abs
	call frac
	movsd qword [rbp - 24], xmm0
	movsd xmm0, qword [rbp - 16]
	call $abs
	mulsd xmm0, qword [rbp - 24]
	movsd xmm1, qword [rbp - 8]
	comisd xmm1, qword [zero]
	jae .ret
	mulsd xmm0, qword [negone]
	.ret:

	add rsp, 24
	pop rbp
	ret

; double sqrt(double x)
sqrt:
	sqrtsd xmm0, xmm0
	ret

; double cbrt(double x)
cbrt:
	call log
	mov rax, 0x4008000000000000
	movq xmm1, rax
	divsd xmm0, xmm1
	call exp
	ret

; double log2(double x)
log2:
	call log
	mov rax, 0x3fe62e42fefa39ef
	movq xmm1, rax
	divsd xmm0, xmm1
	ret

; double log10(double x)
log10:
	call log
	mov rax, 0x40026bb1bbb55516
	movq xmm1, rax
	divsd xmm0, xmm1
	ret

; double normtau(double x)
normtau:
	divsd xmm0, qword [tau]
	call frac
	mulsd xmm0, qword [tau]
	ret

; double sin(double x)
sin:
	comisd xmm0, xmm0
	jp retnan

	call normtau

	mov rdx, 3

	; rdi == max iterrations
	mov rdi, 20

	; rsi == i
	mov rsi, 1

	; xmm0 == x
	; xmm1 == sum
	movsd xmm1, xmm0

	; xmm2 == up
	movsd xmm2, xmm0
	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	; xmm3 == down
	mov rax, 6
	cvtsi2sd xmm3, rax

	; xmm4 == sign
	movsd xmm4, qword [negone]

	.loop:
	movsd xmm7, xmm4
	mulsd xmm7, xmm2
	divsd xmm7, xmm3
	addsd xmm1, xmm7

	mulsd xmm4, qword [negone]

	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	inc rdx
	cvtsi2sd xmm7, rdx
	mulsd xmm3, xmm7
	inc rdx
	cvtsi2sd xmm7, rdx
	mulsd xmm3, xmm7

	inc rsi
	cmp rsi, rdi
	jbe .loop

	movsd xmm0, xmm1
	ret

; double cos(double x)
cos:
	comisd xmm0, xmm0
	jp retnan

	call normtau

	mov rdx, 2

	; rdi == max iterrations
	mov rdi, 20

	; rsi == i
	mov rsi, 1

	; xmm0 == x
	; xmm1 == sum
	movsd xmm1, qword [one]

	; xmm2 == up
	movsd xmm2, xmm0
	mulsd xmm2, xmm0

	; xmm3 == down
	mov rax, 2
	cvtsi2sd xmm3, rax

	; xmm4 == sign
	movsd xmm4, qword [negone]

	.loop:
	movsd xmm7, xmm4
	mulsd xmm7, xmm2
	divsd xmm7, xmm3
	addsd xmm1, xmm7

	mulsd xmm4, qword [negone]

	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	inc rdx
	cvtsi2sd xmm7, rdx
	mulsd xmm3, xmm7
	inc rdx
	cvtsi2sd xmm7, rdx
	mulsd xmm3, xmm7

	inc rsi
	cmp rsi, rdi
	jbe .loop

	movsd xmm0, xmm1
	ret

; double tan(double x)
tan:
	push rbp
	mov rbp, rsp
	sub rsp, 16

	movsd qword [rbp - 8], xmm0
	call cos
	movsd qword [rbp - 16], xmm0
	movsd xmm0, qword [rbp - 8]
	call sin
	divsd xmm0, qword [rbp - 16]

	add rsp, 16
	pop rbp
	ret

; double asin(double x)
asin:
	comisd xmm0, qword [one]
	jp retnan
	ja retnan
	comisd xmm0, qword [negone]
	jb retnan

	mov rdx, 2

	; rdi == max iterrations
	mov rdi, 20

	; rsi == i
	mov rsi, 1

	; xmm0 == x
	; xmm1 == sum
	movsd xmm1, xmm0

	; xmm2 == xterm
	movsd xmm2, xmm0
	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	; xmm3 == 2factterm
	mov rax, 2
	cvtsi2sd xmm3, rax

	; xmm4 == 4term
	mov rax, 4
	cvtsi2sd xmm4, rax

	; xmm5 == factterm
	mov rax, 1
	cvtsi2sd xmm5, rax

	; xmm6 == constterm
	mov rax, 3
	cvtsi2sd xmm6, rax

	.loop:
	movsd xmm7, xmm2
	mulsd xmm7, xmm3
	divsd xmm7, xmm4
	divsd xmm7, xmm5
	divsd xmm7, xmm5
	divsd xmm7, xmm6
	addsd xmm1, xmm7

	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	mov rax, 4
	cvtsi2sd xmm7, rax
	mulsd xmm4, xmm7 

	mov rax, 2
	cvtsi2sd xmm7, rax
	addsd xmm6, xmm7

	inc rdx
	cvtsi2sd xmm7, rdx
	mulsd xmm3, xmm7
	inc rdx
	cvtsi2sd xmm7, rdx
	mulsd xmm3, xmm7

	inc rsi
	cvtsi2sd xmm7, rsi
	mulsd xmm5, xmm7

	cmp rsi, rdi
	jbe .loop

	movsd xmm0, xmm1
	ret

; double acos(double x)
acos:
	call asin
	movsd xmm1, qword [pi]
	mulsd xmm1, qword [half]
	subsd xmm1, xmm0
	movsd xmm0, xmm1
	ret

; double atan(double x)
atan:
	comisd xmm0, xmm0
	jp retnan

	comisd xmm0, qword [one]
	jae .above
	comisd xmm0, qword [negone]
	jbe .below

	jmp .series

	.above:
	movsd xmm1, xmm0
	movsd xmm0, qword [one]
	divsd xmm0, xmm1
	call .series
	movsd xmm1, qword [pi]
	mulsd xmm1, qword [half]
	subsd xmm1, xmm0
	movsd xmm0, xmm1
	ret

	.below:
	movsd xmm1, xmm0
	movsd xmm0, qword [one]
	divsd xmm0, xmm1
	call .series
	movsd xmm1, qword [pi]
	mulsd xmm1, qword [half]
	addsd xmm0, xmm1
	mulsd xmm0, qword [negone]
	ret

	.series:
	; rdi == max iterrations
	mov rdi, 70

	; rsi == i
	mov rsi, 1

	; xmm0 == x
	; xmm1 == sum
	movsd xmm1, xmm0

	; xmm2 == up
	movsd xmm2, xmm0
	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	; xmm3 == down
	mov rax, 3
	cvtsi2sd xmm3, rax

	; xmm4 == sign
	movsd xmm4, qword [negone]

	.loop:
	movsd xmm7, xmm4
	mulsd xmm7, xmm2
	divsd xmm7, xmm3
	addsd xmm1, xmm7

	mulsd xmm4, qword [negone]

	mulsd xmm2, xmm0
	mulsd xmm2, xmm0

	mov rax, 2
	cvtsi2sd xmm7, rax
	addsd xmm3, xmm7

	inc rsi
	cmp rsi, rdi
	jbe .loop

	movsd xmm0, xmm1
	ret

; double atan2(double y, double x)
atan2:
	comisd xmm1, qword [zero]
	jbe .continue0
	divsd xmm0, xmm1
	jmp atan
	.continue0:

	comisd xmm1, qword [zero]
	jae .continue1
	comisd xmm0, qword [zero]
	jb .continue1
	divsd xmm0, xmm1
	call atan
	addsd xmm0, qword [pi]
	ret
	.continue1:

	comisd xmm1, qword [zero]
	jae .continue2
	comisd xmm0, qword [zero]
	jae .continue2
	divsd xmm0, xmm1
	call atan
	subsd xmm0, qword [pi]
	ret
	.continue2:

	comisd xmm1, qword [zero]
	jne .continue3
	comisd xmm0, qword [zero]
	jbe .continue3
	movsd xmm0, qword [pi]
	mulsd xmm0, qword [half]
	ret
	.continue3:

	comisd xmm1, qword [zero]
	jne .continue4
	comisd xmm0, qword [zero]
	jae .continue4
	movsd xmm0, qword [pi]
	mulsd xmm0, qword [half]
	mulsd xmm0, qword [negone]
	ret
	.continue4:

	xorps xmm0, xmm0
	ret

; double sinh(double x)
sinh:
	push rbp
	mov rbp, rsp
	sub rsp, 16

	movsd qword [rbp - 8], xmm0
	mulsd xmm0, qword [negone]
	call exp
	movsd qword [rbp - 16], xmm0
	movsd xmm0, qword [rbp - 8]
	call exp
	subsd xmm0, qword [rbp - 16]
	mulsd xmm0, qword [half]

	add rsp, 16
	pop rbp
	ret

; double cosh(double x)
cosh:
	push rbp
	mov rbp, rsp
	sub rsp, 16

	movsd qword [rbp - 8], xmm0
	mulsd xmm0, qword [negone]
	call exp
	movsd qword [rbp - 16], xmm0
	movsd xmm0, qword [rbp - 8]
	call exp
	addsd xmm0, qword [rbp - 16]
	mulsd xmm0, qword [half]

	add rsp, 16
	pop rbp
	ret

; double tanh(double x)
tanh:
	push rbp
	mov rbp, rsp
	sub rsp, 16

	movsd qword [rbp - 8], xmm0
	call cosh
	movsd qword [rbp - 16], xmm0
	movsd xmm0, qword [rbp - 8]
	call sinh
	divsd xmm0, qword [rbp - 16]

	add rsp, 16
	pop rbp
	ret

; double asinh(double x)
asinh:
	push rbp
	mov rbp, rsp
	sub rsp, 8

	movsd qword [rbp - 8], xmm0

	mulsd xmm0, xmm0
	addsd xmm0, qword [one]
	call sqrt
	addsd xmm0, qword [rbp - 8]
	call log

	add rsp, 8
	pop rbp
	ret

; double acosh(double x)
acosh:
	push rbp
	mov rbp, rsp
	sub rsp, 8

	movsd qword [rbp - 8], xmm0

	mulsd xmm0, xmm0
	subsd xmm0, qword [one]
	call sqrt
	addsd xmm0, qword [rbp - 8]
	call log

	add rsp, 8
	pop rbp
	ret

; double atanh(double x)
atanh:
	movsd xmm1, qword [one]
	subsd xmm1, xmm0
	addsd xmm0, qword [one]
	divsd xmm0, xmm1
	call log
	mulsd xmm0, qword [half]
	ret
