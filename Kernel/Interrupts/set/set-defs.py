output = ""

for i in range(0, 256):
	output += "ASMCALL void ISR" + str(i) + "(void);\n"

file = open("defs.inc", "w")
file.write(output)
file.close()