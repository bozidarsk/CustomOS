global getrax
global getrbx
global getrcx
global getrdx
global getrsi
global getrdi
global getrbp
global getrsp
global getr8
global getr9
global getr10
global getr11
global getr12
global getr13
global getr14
global getr15
global getrip
global getrflags

section .text

; ulong getrax()
getrax:
	mov rax, rax
	ret

; ulong getrbx()
getrbx:
	mov rax, rbx
	ret

; ulong getrcx()
getrcx:
	mov rax, rcx
	ret

; ulong getrdx()
getrdx:
	mov rax, rdx
	ret

; ulong getrsi()
getrsi:
	mov rax, rsi
	ret

; ulong getrdi()
getrdi:
	mov rax, rdi
	ret

; ulong getrbp()
getrbp:
	mov rax, rbp
	ret

; ulong getrsp()
getrsp:
	lea rax, [rsp + 8] ; return address
	ret

; ulong getr8()
getr8:
	mov rax, r8
	ret

; ulong getr9()
getr9:
	mov rax, r9
	ret

; ulong getr10()
getr10:
	mov rax, r10
	ret

; ulong getr11()
getr11:
	mov rax, r11
	ret

; ulong getr12()
getr12:
	mov rax, r12
	ret

; ulong getr13()
getr13:
	mov rax, r13
	ret

; ulong getr14()
getr14:
	mov rax, r14
	ret

; ulong getr15()
getr15:
	mov rax, r15
	ret

; ulong getrip()
getrip:
	pop rax
	push rax
	ret

; ulong getrflags()
getrflags:
	pushfq
	pop rax
	ret
