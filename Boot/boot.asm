bits 32

global _start32

extern _start64
extern STACKTOP

section .rodata

$gdt:
	dq 0 ; zero entry
	.code_segment: equ $ - $gdt
		dq (1 << 41) | (1 << 43) | (1 << 44) | (1 << 47) | (1 << 53)
	.descriptor:
		dw $ - $gdt - 1
		dq $gdt

section .bss

multiboot_address: resb 8

alignb 4096
page_table_l4: resb 4096
page_table_l3: resb 4096
page_table_l2: resb 4096
page_table_l1: resb 4096

section .stack

section .text

_start32:
	cli

	mov esp, STACKTOP

	mov esi, ebx
	mov ecx, [esi]

	mov eax, ecx
	mov ebx, 16
	div ebx
	mov eax, ebx
	sub eax, edx
	add ecx, eax

	sub esp, ecx
	mov edi, esp
	.save_bootinfo:
	mov al, byte [esi]
	mov byte [edi], al
	inc edi
	inc esi
	dec ecx
	cmp ecx, 0
	ja .save_bootinfo
	mov dword [multiboot_address], esp


	call enable_a20
	jmp enter_long_mode

	jmp error

enable_a20:
	mov edi, 0x112345 ; odd megabyte address
	mov esi, 0x012345 ; even megabyte address
	mov [esi], esi ; making sure that both addresses contain diffrent values
	mov [edi], edi ; (if A20 line is cleared the two pointers would point to the address 0x012345 that would contain 0x112345 (edi)) 
	cmpsd ; compare addresses to see if the're equivalent
	je error

	ret

setup_page_tables:
	mov eax, page_table_l3 
	or eax, 0b11
	mov [page_table_l4], eax

	mov eax, page_table_l2
	or eax, 0b11
	mov [page_table_l3], eax

	mov eax, page_table_l1
	or eax, 0b11
	mov [page_table_l2], eax

	mov ecx, 1
	.loop1G: ; ecx = 1; ecx < 512; ecx++
	mov eax, 1 * 1024 * 1024 * 1024
	mul ecx
	or eax, 0b10000011
	mov [page_table_l3 + ecx * 8], eax
	inc ecx
	cmp ecx, 512
	jb .loop1G

	mov ecx, 1
	.loop2M: ; ecx = 1; ecx < 512; ecx++
	mov eax, 2 * 1024 * 1024
	mul ecx
	or eax, 0b10000011
	mov [page_table_l2 + ecx * 8], eax
	inc ecx
	cmp ecx, 512
	jb .loop2M

	xor ecx, ecx
	.loop4K: ; ecx = 0; ecx < 512; ecx++
	mov eax, 4 * 1024
	mul ecx
	test ecx, ecx
	jz .nopresent2K
	or eax, 1
	.nopresent2K:
	or eax, 0b10000010
	mov [page_table_l1 + ecx * 8], eax
	inc ecx
	cmp ecx, 512
	jb .loop4K

	ret

enter_long_mode:
	; check for long mode
	mov eax, 0x80000001
	cpuid
	and edx, 1 << 29
	cmp edx, 0
	je error

	; setup page tables
	call setup_page_tables

	; pass table pointer to cpu
	mov eax, page_table_l4
	mov cr3, eax

	; enable PAE
	mov eax, cr4
	or eax, 1 << 5
	mov cr4, eax

	; enable long mode
	mov ecx, 0xc0000080
	rdmsr
	or eax, 1 << 8
	wrmsr

	; enable paging
	mov eax, cr0
	or eax, 1 << 31
	mov cr0, eax

	lgdt [$gdt.descriptor]
	mov edi, dword [multiboot_address]
	jmp $gdt.code_segment:_start64

error:
	cli
	hlt
