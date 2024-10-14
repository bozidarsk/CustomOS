global loadidt
global getisr
global setisrhandler
global getisrhandler
global loadpanic

extern sendeoi
extern memcpy
extern panic
extern Int64__ToString
extern String__Concat
extern _ZTV6String

section .rodata

string_failed:
	dq _ZTV6String
	dd 39
	dw 0x49, 0x6e, 0x74, 0x65, 0x72, 0x72, 0x75, 0x70, 0x74, 0x20, 0x68, 0x61, 0x6e, 0x64, 0x6c, 0x65, 0x72, 0x20, 0x66, 0x61, 0x69, 0x6c, 0x65, 0x64, 0x20, 0x66, 0x6f, 0x72, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x72, 0x75, 0x70, 0x74, 0x20, 0x00
string_notfound:
	dq _ZTV6String
	dd 42
	dw 0x49, 0x6e, 0x74, 0x65, 0x72, 0x72, 0x75, 0x70, 0x74, 0x20, 0x68, 0x61, 0x6e, 0x64, 0x6c, 0x65, 0x72, 0x20, 0x6e, 0x6f, 0x74, 0x20, 0x66, 0x6f, 0x75, 0x6e, 0x64, 0x20, 0x66, 0x6f, 0x72, 0x20, 0x69, 0x6e, 0x74, 0x65, 0x72, 0x72, 0x75, 0x70, 0x74, 0x20, 0x00
string_dot:
	dq _ZTV6String
	dd 1
	dw 0x2e, 0x00

isr_table:
	%assign i 0
	%rep 256 
		dq isr%+i
	%assign i i+1 
	%endrep

section .bss

align 16
; keep in sync with Kernel/Registers.cs
registers:
	.rax: resb 8
	.rbx: resb 8
	.rcx: resb 8
	.rdx: resb 8
	.rsi: resb 8
	.rdi: resb 8
	.rbp: resb 8
	.rsp: resb 8
	.r8: resb 8
	.r9: resb 8
	.r10: resb 8
	.r11: resb 8
	.r12: resb 8
	.r13: resb 8
	.r14: resb 8
	.r15: resb 8
	.rip: resb 8
	.rflags: resb 8
	.cs: resb 2
	.ds: resb 2
	.ss: resb 2
	.es: resb 2
	.fs: resb 2
	.gs: resb 2
	.cr0: resb 8
	.cr2: resb 8
	.cr3: resb 8
	.cr4: resb 8
	.cr8: resb 8
	.msr: resb 8
	.xmm0: resb 16
	.xmm1: resb 16
	.xmm2: resb 16
	.xmm3: resb 16
	.xmm4: resb 16
	.xmm5: resb 16
	.xmm6: resb 16
	.xmm7: resb 16
	.xmm8: resb 16
	.xmm9: resb 16
	.xmm10: resb 16
	.xmm11: resb 16
	.xmm12: resb 16
	.xmm13: resb 16
	.xmm14: resb 16
	.xmm15: resb 16

registers_size: equ ($ - registers) + (16 - (($ - registers) % 16)) ; for alignment

rsp_alignment: resq 1

section .data

isrhandler_table:
	%assign i 0
	%rep 256 
		dq 0
	%assign i i+1 
	%endrep

section .text

; void loadidt(InterruptDescriptorTable* table)
loadidt:
	lidt [rdi]
	sti
	ret

; void* getisr(int index)
getisr:
	mov rax, qword [isr_table + edi * 8]
	ret

; void getisrhandler(int index, void* handler)
setisrhandler:
	mov qword [isrhandler_table + edi * 8], rsi
	ret

; void* getisrhandler(int index)
getisrhandler:
	mov rax, qword [isrhandler_table + edi * 8]
	ret

; void void loadpanic(ulong rdi, ulong rsi, ulong rbp, ulong rsp)
loadpanic:
	mov rbp, rdx
	mov rsp, rcx
	jmp panic

isr_common:
	mov [registers.rax], rax
	mov [registers.rbx], rbx
	mov [registers.rcx], rcx
	mov [registers.rdx], rdx
	mov [registers.rsi], rsi
	mov [registers.rdi], rdi
	mov [registers.rbp], rbp
	mov [registers.r8], r8
	mov [registers.r9], r9
	mov [registers.r10], r10
	mov [registers.r11], r11
	mov [registers.r12], r12
	mov [registers.r13], r13
	mov [registers.r14], r14
	mov [registers.r15], r15

	sub rsp, 8
	mov rbp, rsp
	; rbp == handler
	; rbp + 8 == index
	; rbp + 16 == error
	; rbp + 24 == rip
	; rbp + 32 == cs
	; rbp + 40 == rflags
	; rbp + 48 == rsp
	; rbp + 56 == ss

	; align stack to 16 bytes
	mov rax, rsp
	and rax, 0xf
	sub rsp, rax
	mov qword [rsp_alignment], rax

	mov rax, qword [rbp + 48]
	mov qword [registers.rsp], rax
	mov rax, qword [rbp + 24]
	mov qword [registers.rip], rax
	mov rax, qword [rbp + 40]
	mov qword [registers.rflags], rax
	mov rax, qword [rbp + 32]
	mov word [registers.cs], ax
	mov rax, qword [rbp + 56]
	mov word [registers.ss], ax
	mov word [registers.ds], ds
	mov word [registers.es], es
	mov word [registers.fs], fs
	mov word [registers.gs], gs
	mov rax, cr0
	mov qword [registers.cr0], rax
	mov rax, cr2
	mov qword [registers.cr2], rax
	mov rax, cr3
	mov qword [registers.cr3], rax
	mov rax, cr4
	mov qword [registers.cr4], rax
	mov rax, cr8
	mov qword [registers.cr8], rax

	mov rcx, 0xc0000080
	rdmsr
	mov qword [registers.msr], rax

	movlpd [registers.xmm0], xmm0
	movhpd [registers.xmm0 + 8], xmm0
	movlpd [registers.xmm1], xmm1
	movhpd [registers.xmm1 + 8], xmm1
	movlpd [registers.xmm2], xmm2
	movhpd [registers.xmm2 + 8], xmm2
	movlpd [registers.xmm3], xmm3
	movhpd [registers.xmm3 + 8], xmm3
	movlpd [registers.xmm4], xmm4
	movhpd [registers.xmm4 + 8], xmm4
	movlpd [registers.xmm5], xmm5
	movhpd [registers.xmm5 + 8], xmm5
	movlpd [registers.xmm6], xmm6
	movhpd [registers.xmm6 + 8], xmm6
	movlpd [registers.xmm7], xmm7
	movhpd [registers.xmm7 + 8], xmm7
	movlpd [registers.xmm8], xmm8
	movhpd [registers.xmm8 + 8], xmm8
	movlpd [registers.xmm9], xmm9
	movhpd [registers.xmm9 + 8], xmm9
	movlpd [registers.xmm10], xmm10
	movhpd [registers.xmm10 + 8], xmm10
	movlpd [registers.xmm11], xmm11
	movhpd [registers.xmm11 + 8], xmm11
	movlpd [registers.xmm12], xmm12
	movhpd [registers.xmm12 + 8], xmm12
	movlpd [registers.xmm13], xmm13
	movhpd [registers.xmm13 + 8], xmm13
	movlpd [registers.xmm14], xmm14
	movhpd [registers.xmm14 + 8], xmm14
	movlpd [registers.xmm15], xmm15
	movhpd [registers.xmm15 + 8], xmm15


	mov rdi, qword [rbp + 8]
	call getisrhandler
	mov qword [rbp], rax
	cmp rax, 0
	jnz .callhandler

	.handlernotfound:
	lea rdi, [rbp + 8]
	call Int64__ToString
	mov rdi, rax
	mov rsi, string_dot
	xor rdx, rdx
	call String__Concat
	mov rdi, string_notfound
	mov rsi, rax
	xor rdx, rdx
	call String__Concat
	mov rdi, rax
	xor rsi, rsi
	mov rsp, [registers.rsp]
	mov rbp, [registers.rbp]
	jmp panic


	.callhandler:
	sub rsp, registers_size
	mov rdi, rsp
	mov rsi, registers
	mov rdx, registers_size
	call memcpy

	mov rdi, rsp
	mov rsi, qword [rbp + 16]
	call qword [rbp]

	mov rdi, qword [rbp + 8]
	call sendeoi


	mov rax, [registers.rax]
	mov rbx, [registers.rbx]
	mov rcx, [registers.rcx]
	mov rdx, [registers.rdx]
	mov rsi, [registers.rsi]
	mov rdi, [registers.rdi]
	mov rbp, [registers.rbp]
	mov r8, [registers.r8]
	mov r9, [registers.r9]
	mov r10, [registers.r10]
	mov r11, [registers.r11]
	mov r12, [registers.r12]
	mov r13, [registers.r13]
	mov r14, [registers.r14]
	mov r15, [registers.r15]

	add rsp, registers_size + 8 + 8 + 8 ; registers, handler, index, error
	add rsp, qword [rsp_alignment]
	iretq

%macro isr_noerror 1
isr%1:
	push 0
	push %1
	jmp isr_common
%endmacro

%macro isr_error 1
isr%1:
	push %1
	jmp isr_common
%endmacro

isr_noerror 0
isr_noerror 1
isr_noerror 2
isr_noerror 3
isr_noerror 4
isr_noerror 5
isr_noerror 6
isr_noerror 7
isr_error   8
isr_noerror 9
isr_error   10
isr_error   11
isr_error   12
isr_error   13
isr_error   14
isr_noerror 15
isr_noerror 16
isr_error   17
isr_noerror 18
isr_noerror 19
isr_noerror 20
isr_noerror 21
isr_noerror 22
isr_noerror 23
isr_noerror 24
isr_noerror 25
isr_noerror 26
isr_noerror 27
isr_noerror 28
isr_noerror 29
isr_error   30
isr_noerror 31
isr_noerror 32
isr_noerror 33
isr_noerror 34
isr_noerror 35
isr_noerror 36
isr_noerror 37
isr_noerror 38
isr_noerror 39
isr_noerror 40
isr_noerror 41
isr_noerror 42
isr_noerror 43
isr_noerror 44
isr_noerror 45
isr_noerror 46
isr_noerror 47
isr_noerror 48
isr_noerror 49
isr_noerror 50
isr_noerror 51
isr_noerror 52
isr_noerror 53
isr_noerror 54
isr_noerror 55
isr_noerror 56
isr_noerror 57
isr_noerror 58
isr_noerror 59
isr_noerror 60
isr_noerror 61
isr_noerror 62
isr_noerror 63
isr_noerror 64
isr_noerror 65
isr_noerror 66
isr_noerror 67
isr_noerror 68
isr_noerror 69
isr_noerror 70
isr_noerror 71
isr_noerror 72
isr_noerror 73
isr_noerror 74
isr_noerror 75
isr_noerror 76
isr_noerror 77
isr_noerror 78
isr_noerror 79
isr_noerror 80
isr_noerror 81
isr_noerror 82
isr_noerror 83
isr_noerror 84
isr_noerror 85
isr_noerror 86
isr_noerror 87
isr_noerror 88
isr_noerror 89
isr_noerror 90
isr_noerror 91
isr_noerror 92
isr_noerror 93
isr_noerror 94
isr_noerror 95
isr_noerror 96
isr_noerror 97
isr_noerror 98
isr_noerror 99
isr_noerror 100
isr_noerror 101
isr_noerror 102
isr_noerror 103
isr_noerror 104
isr_noerror 105
isr_noerror 106
isr_noerror 107
isr_noerror 108
isr_noerror 109
isr_noerror 110
isr_noerror 111
isr_noerror 112
isr_noerror 113
isr_noerror 114
isr_noerror 115
isr_noerror 116
isr_noerror 117
isr_noerror 118
isr_noerror 119
isr_noerror 120
isr_noerror 121
isr_noerror 122
isr_noerror 123
isr_noerror 124
isr_noerror 125
isr_noerror 126
isr_noerror 127
isr_noerror 128
isr_noerror 129
isr_noerror 130
isr_noerror 131
isr_noerror 132
isr_noerror 133
isr_noerror 134
isr_noerror 135
isr_noerror 136
isr_noerror 137
isr_noerror 138
isr_noerror 139
isr_noerror 140
isr_noerror 141
isr_noerror 142
isr_noerror 143
isr_noerror 144
isr_noerror 145
isr_noerror 146
isr_noerror 147
isr_noerror 148
isr_noerror 149
isr_noerror 150
isr_noerror 151
isr_noerror 152
isr_noerror 153
isr_noerror 154
isr_noerror 155
isr_noerror 156
isr_noerror 157
isr_noerror 158
isr_noerror 159
isr_noerror 160
isr_noerror 161
isr_noerror 162
isr_noerror 163
isr_noerror 164
isr_noerror 165
isr_noerror 166
isr_noerror 167
isr_noerror 168
isr_noerror 169
isr_noerror 170
isr_noerror 171
isr_noerror 172
isr_noerror 173
isr_noerror 174
isr_noerror 175
isr_noerror 176
isr_noerror 177
isr_noerror 178
isr_noerror 179
isr_noerror 180
isr_noerror 181
isr_noerror 182
isr_noerror 183
isr_noerror 184
isr_noerror 185
isr_noerror 186
isr_noerror 187
isr_noerror 188
isr_noerror 189
isr_noerror 190
isr_noerror 191
isr_noerror 192
isr_noerror 193
isr_noerror 194
isr_noerror 195
isr_noerror 196
isr_noerror 197
isr_noerror 198
isr_noerror 199
isr_noerror 200
isr_noerror 201
isr_noerror 202
isr_noerror 203
isr_noerror 204
isr_noerror 205
isr_noerror 206
isr_noerror 207
isr_noerror 208
isr_noerror 209
isr_noerror 210
isr_noerror 211
isr_noerror 212
isr_noerror 213
isr_noerror 214
isr_noerror 215
isr_noerror 216
isr_noerror 217
isr_noerror 218
isr_noerror 219
isr_noerror 220
isr_noerror 221
isr_noerror 222
isr_noerror 223
isr_noerror 224
isr_noerror 225
isr_noerror 226
isr_noerror 227
isr_noerror 228
isr_noerror 229
isr_noerror 230
isr_noerror 231
isr_noerror 232
isr_noerror 233
isr_noerror 234
isr_noerror 235
isr_noerror 236
isr_noerror 237
isr_noerror 238
isr_noerror 239
isr_noerror 240
isr_noerror 241
isr_noerror 242
isr_noerror 243
isr_noerror 244
isr_noerror 245
isr_noerror 246
isr_noerror 247
isr_noerror 248
isr_noerror 249
isr_noerror 250
isr_noerror 251
isr_noerror 252
isr_noerror 253
isr_noerror 254
isr_noerror 255
