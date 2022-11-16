#include <keyboard.h>
#include <display.h>
#include <io.h>
#include <string.h>

uint16 GetKeyboardState() { return (inw(0x60) >> 8) & 0xff; }
bool GetKeyUp(uint8 key) { return (inw(0x60) >> 8) == key + 0x80; }
bool GetKeyDown(uint8 key) { return (inw(0x60) >> 8) == key; }
void ClearKeyboard() { outb(0x60, 0x00); }

#define codesLength 47
static uint8 codes[codesLength] = { 0x27, 0x0c, 0x0d, 0x1a, 0x1b, 0x28, 0x29, 0x2b, 0x33, 0x34, 0x35, 0x0b, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x1e, 0x30, 0x2e, 0x20, 0x12, 0x21, 0x22, 0x23, 0x17, 0x24, 0x25, 0x26, 0x32, 0x31, 0x18, 0x19, 0x10, 0x13, 0x1f, 0x14, 0x16, 0x2f, 0x11, 0x2d, 0x15, 0x2c };
static wchar chars[codesLength * 2] = { 0x3b, 0x2d, 0x3d, 0x5b, 0x5d, 0x27, 0x60, 0x5c, 0x2c, 0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x3a, 0x5f, 0x2b, 0x7b, 0x7d, 0x22, 0x7e, 0x7c, 0x3c, 0x3e, 0x3f, 0x29, 0x21, 0x40, 0x23, 0x24, 0x25, 0x5e, 0x26, 0x2a, 0x28, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a };

static bool control = false;
static bool alt = false;
static bool shift = false;

static bool capslk = false;
static bool capslkHold = false;
static bool numlk = true;
static bool numlkHold = false;

void UpdateKeyboard() 
{
	if (GetKeyDown(KEY_CONTROL)) { control = true; }
	if (GetKeyUp(KEY_CONTROL)) { control = false; }

	if (GetKeyDown(KEY_ALT)) { alt = true; }
	if (GetKeyUp(KEY_ALT)) { alt = false; }

	if (GetKeyDown(KEY_LSHIFT) || GetKeyDown(KEY_RSHIFT)) { shift = true; }
	if (GetKeyUp(KEY_LSHIFT) || GetKeyUp(KEY_RSHIFT)) { shift = false; }

	if (GetKeyUp(KEY_CAPSLOCK) && capslkHold) { capslk = !capslk; }
	if (GetKeyDown(KEY_CAPSLOCK)) { capslkHold = true; }
	if (GetKeyUp(KEY_CAPSLOCK)) { capslkHold = false; }

	if (GetKeyUp(KEY_NUMLOCK) && numlkHold) { numlk = !numlk; }
	if (GetKeyDown(KEY_NUMLOCK)) { numlkHold = true; }
	if (GetKeyUp(KEY_NUMLOCK)) { numlkHold = false; }
}

bool GetKeyboardChar(wchar x) { return GetKeyboardChar() == x; }
wchar GetKeyboardChar() 
{
	UpdateKeyboard();
	if (GetKeyDown(KEY_ENTER)) { return 0x0a; }
	if (GetKeyDown(KEY_SPACE)) { return 0x20; }
	if (GetKeyDown(KEY_BACKSPACE)) { return 0x08; }
	if (GetKeyDown(KEY_NUM0) && numlk) { return 0x30; }
	if (GetKeyDown(KEY_NUM1) && numlk) { return 0x31; }
	if (GetKeyDown(KEY_NUM2) && numlk) { return 0x32; }
	if (GetKeyDown(KEY_NUM3) && numlk) { return 0x33; }
	if (GetKeyDown(KEY_NUM4) && numlk) { return 0x34; }
	if (GetKeyDown(KEY_NUM5) && numlk) { return 0x35; }
	if (GetKeyDown(KEY_NUM6) && numlk) { return 0x36; }
	if (GetKeyDown(KEY_NUM7) && numlk) { return 0x37; }
	if (GetKeyDown(KEY_NUM8) && numlk) { return 0x38; }
	if (GetKeyDown(KEY_NUM9) && numlk) { return 0x39; }
	if (GetKeyDown(0x56) && shift) { return 0x7c; } // :
	if (GetKeyDown(0x56)) { return 0x5c; } // ;
	if (GetKeyDown(0x37)) { return 0x2a; } // keypad *
	if (GetKeyDown(0x4e)) { return 0x2b; } // keypad +
	if (GetKeyDown(0x4a)) { return 0x2d; } // keypad -
	if (GetKeyDown(0x53)) { return 0x2e; } // keypad .

	uint8 i = 0;
	while (i < codesLength) { UpdateKeyboard(); if (GetKeyDown(codes[i])) { break; } i++; }
	if (i >= codesLength) { return 0; }
	bool next = (capslk && i >= 20) ^ shift;
	return chars[i + (codesLength * next)];
}

wchar ReadKey() 
{
	ClearKeyboard();
	wchar result = GetKeyboardChar();
	while (result == 0) { result = GetKeyboardChar(); }
	return result;
}

// string ReadLine() 
// {
// 	uint16 i = 0;
// 	char input = 0x00;
// 	while (input != 0x0a) 
// 	{
// 		if (i >= bufferLength) { i = bufferLength - 1; break; }
// 		input = ReadKey();
// 		buffer[i] = input;
// 		i++;
// 	}

// 	buffer[i] = '\0';
// 	return buffer;
// }