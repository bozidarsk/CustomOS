errors = [ 8, 10, 11, 12, 13, 14, 17, 21, 24, 25 ]
output = "\
%macro isr_noerror 1\n\
global ISR%1:\n\
ISR%1:\n\
	push 0\n\
	push %1\n\
	jmp isr_common\n\
%endmacro\n\
\n\
%macro isr_error 1\n\
global ISR%1:\n\
ISR%1:\n\
	push %1\n\
	jmp isr_common\n\
%endmacro\n\n\
"

def Error(i):
	for t in range(0, len(errors)):
		if (errors[t] == i):
			return True
	return False

for i in range(0, 256):
	if (Error(i)):
		output += "isr_error   " + str(i) + "\n"
	else:
		output += "isr_noerror " + str(i) + "\n"

file = open("isrs.inc", "w")
file.write(output)
file.close()