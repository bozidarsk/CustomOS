power = 3
sign = 0
output = "x"

def p():
	out = "x"
	i = power - 1
	while (i > 0):
		out += "*x"
		i -= 1
	return out

i = 0
while (i < 50):
	if (sign):
		output += " + " + "(" + p() + " / " + str(power) + ")"
		sign = 0
	else:
		output += " - " + "(" + p() + " / " + str(power) + ")"
		sign = 1
	power += 2
	i += 1

print(output)