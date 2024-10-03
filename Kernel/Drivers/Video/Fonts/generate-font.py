from PIL import Image

# settings
charw = int(input("charw: "))
charh = int(input("charh: "))
file = input("file: ")

image = Image.open(file, 'r').convert('RGB')
pixels = image.load()

if image.width % charw != 0 or image.height % charh != 0:
	print("Invalid dimentions.")
	exit(1)

for char in range(0x80):
	cx = int(char % (image.width / charw)) * charw
	cy = int(char / (image.width / charw)) * charh

	data = []

	for y in range(charh):
		for x in range(charw):
			data.append(1 if pixels[cx + x, cy + y] != (0, 0, 0) else 0);

	while len(data) % 8 != 0:
		data.append(0)

	print("db ", end="")
	for i in range(int(len(data) / 8)):
		b = 0
		b |= data[(i * charw) + 0] << 0
		b |= data[(i * charw) + 1] << 1
		b |= data[(i * charw) + 2] << 2
		b |= data[(i * charw) + 3] << 3
		b |= data[(i * charw) + 4] << 4
		b |= data[(i * charw) + 5] << 5
		b |= data[(i * charw) + 6] << 6
		b |= data[(i * charw) + 7] << 7
		print(f" 0x{b:02x}, ", end="")
	print("")
