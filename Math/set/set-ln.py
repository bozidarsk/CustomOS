from math import factorial

power = 2
sign = 0
output = "x"

def p():
	out = "n"
	i = power - 1
	while (i > 0):
		out += "*n"
		i -= 1
	return out

i = 0
while (i < 15):
	if (sign):
		output += " + " + "(" + p() + " / " + str(i + 2) + ")"
		sign = 0
	else:
		output += " - " + "(" + p() + " / " + str(i + 2) + ")"
		sign = 1
	power += 1
	i += 1

print(output)
