from PIL import Image

img = Image.open("colorPalett.png", "r").convert('RGB')
p = img.load()

def FormatValue(color):
	r = str(hex(color[0])).replace("0x", "")
	if (len(r) == 1):
		r = "0" + r
	g = str(hex(color[1])).replace("0x", "")
	if (len(g) == 1):
		g = "0" + g
	b = str(hex(color[2])).replace("0x", "")
	if (len(b) == 1):
		b = "0" + b
	return r + g + b

x = 0
y = 0
width = img.size[0]
height = img.size[1]
color = (1, 1, 1)
blockCount = 0
blockSize = 75
array = "[ "
string = ""

while (y < height):
	x = 0
	while (x < width):
		if (color != p[x, y]):
			color = p[x, y]
			array += "[" + str(color[0]) + ", " + str(color[1]) + ", " + str(color[2]) + "]" + ", "
			string += FormatValue(color) + " "
			blockCount += 1

		x += blockSize

	string += "\n"
	y += blockSize

array += " ]"
array = array.replace(",  ]", " ]")
print(string)
print(array)
print(blockCount)