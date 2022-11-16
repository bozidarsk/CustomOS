power = 3
nUp = 1
nDown = 2
output = "x"

def p():
	out = "x"
	i = power - 1
	while (i > 0):
		out += "*x"
		i -= 1
	return out

def up():
	num = 1
	i = nUp
	while (i > 0):
		num *= i
		i -= 2
	return num

def down():
	num = 1
	i = nDown
	while (i > 0):
		num *= i
		i -= 2
	return num * power

i = 0
while (i < 50):
	output += " + (" + p() + "* " + str(up() / down()) + ")"
	power += 2
	nUp += 2
	nDown += 2
	i += 1

print(output)