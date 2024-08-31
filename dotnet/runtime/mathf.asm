global absf
global floorf
global ceilf
global fracf
global fmodf
global sqrtf
global cbrtf
global log2f
global log10f
global sinf
global cosf
global tanf
global asinf
global acosf
global atanf
global atan2f
global sinhf
global coshf
global tanhf
global asinhf
global acoshf
global atanhf
global fmaf
global scalbnf
global expf
global logf
global powf
global modff

extern $abs
extern floor
extern ceil
extern frac
extern fmod
extern sqrt
extern cbrt
extern log2
extern log10
extern sin
extern cos
extern tan
extern asin
extern acos
extern atan
extern atan2
extern sinh
extern cosh
extern tanh
extern asinh
extern acosh
extern atanh
extern fma
extern scalbn
extern exp
extern log
extern pow

section .text

; float modff(float x, out float interger)
modff:
	cvttss2si rax, xmm0
	cvtsi2ss xmm1, rax
	movss dword [rdi], xmm1
	subss xmm0, xmm1
	ret

; float absf(float x)
absf:
	push rbp
	cvtss2sd xmm0, xmm0
	call $abs
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float floorf(float x)
floorf:
	push rbp
	cvtss2sd xmm0, xmm0
	call floor
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float ceilf(float x)
ceilf:
	push rbp
	cvtss2sd xmm0, xmm0
	call ceil
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float fracf(float x)
fracf:
	push rbp
	cvtss2sd xmm0, xmm0
	call frac
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float fmodf(float a, float b)
fmodf:
	push rbp
	cvtss2sd xmm0, xmm0
	cvtss2sd xmm1, xmm1
	call fmod
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float sqrtf(float x)
sqrtf:
	push rbp
	cvtss2sd xmm0, xmm0
	call sqrt
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float cbrtf(float x)
cbrtf:
	push rbp
	cvtss2sd xmm0, xmm0
	call cbrt
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float log2f(float x)
log2f:
	push rbp
	cvtss2sd xmm0, xmm0
	call log2
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float log10f(float x)
log10f:
	push rbp
	cvtss2sd xmm0, xmm0
	call log10
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float sinf(float x)
sinf:
	push rbp
	cvtss2sd xmm0, xmm0
	call sin
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float cosf(float x)
cosf:
	push rbp
	cvtss2sd xmm0, xmm0
	call cos
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float tanf(float x)
tanf:
	push rbp
	cvtss2sd xmm0, xmm0
	call tan
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float asinf(float x)
asinf:
	push rbp
	cvtss2sd xmm0, xmm0
	call asin
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float acosf(float x)
acosf:
	push rbp
	cvtss2sd xmm0, xmm0
	call acos
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float atanf(float x)
atanf:
	push rbp
	cvtss2sd xmm0, xmm0
	call atan
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float atan2f(float x)
atan2f:
	push rbp
	cvtss2sd xmm0, xmm0
	call atan2
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float sinhf(float x)
sinhf:
	push rbp
	cvtss2sd xmm0, xmm0
	call sinh
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float coshf(float x)
coshf:
	push rbp
	cvtss2sd xmm0, xmm0
	call cosh
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float tanhf(float x)
tanhf:
	push rbp
	cvtss2sd xmm0, xmm0
	call tanh
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float asinhf(float x)
asinhf:
	push rbp
	cvtss2sd xmm0, xmm0
	call asinh
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float acoshf(float x)
acoshf:
	push rbp
	cvtss2sd xmm0, xmm0
	call acosh
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float atanhf(float x)
atanhf:
	push rbp
	cvtss2sd xmm0, xmm0
	call atanh
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float fmaf(float x, float y, float z)
fmaf:
	push rbp
	cvtss2sd xmm0, xmm0
	cvtss2sd xmm1, xmm1
	cvtss2sd xmm2, xmm2
	call fma
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float scalbnf(float x, int n)
scalbnf:
	push rbp
	cvtss2sd xmm0, xmm0
	call scalbn
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float expf(float x)
expf:
	push rbp
	cvtss2sd xmm0, xmm0
	call exp
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float logf(float x)
logf:
	push rbp
	cvtss2sd xmm0, xmm0
	call log
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

; float powf(float x, float y)
powf:
	push rbp
	cvtss2sd xmm0, xmm0
	cvtss2sd xmm1, xmm1
	call pow
	cvtsd2ss xmm0, xmm0
	pop rbp
	ret

