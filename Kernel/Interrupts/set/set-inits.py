output = ""

for i in range(0, 256):
	output += "SetGate(" + str(i) + ", (void*)ISR" + str(i) + ", 0x08, IDT_FLAG_RING0 | IDT_FLAG_GATE_32BIT_INT);\n"

file = open("inits.inc", "w")
file.write(output)
file.close()