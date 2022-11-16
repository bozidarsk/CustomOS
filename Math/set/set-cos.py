from math import factorial

power = 2
sign = 0
output = "1"

def p():
	out = "x"
	i = power - 1
	while (i > 0):
		out += "*x"
		i -= 1
	return out

i = 0
while (i < 10):
	if (sign):
		output += " + " + "(" + p() + " / " + str(factorial(power)) + ")"
		sign = 0
	else:
		output += " - " + "(" + p() + " / " + str(factorial(power)) + ")"
		sign = 1
	power += 2
	i += 1

print(output)