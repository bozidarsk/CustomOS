#ifndef KEYBOARD_H

#define KEYBOARD_H
#include <definitions.h>
#include <string.h>

#define KEY_LSHIFT 0x2a
#define KEY_RSHIFT 0x36
#define KEY_ESCAPE 0x01
#define KEY_CONTROL 0x1d
#define KEY_TAB 0x0f
#define KEY_ALT 0x38
#define KEY_SPACE 0x39
#define KEY_CAPSLOCK 0x3a
#define KEY_NUMLOCK 0x45
#define KEY_ENTER 0x1c
#define KEY_BACKSPACE 0x0e
#define KEY_CONTEXT 0x5b
#define KEY_WIN 0x5b

#define KEY_A 0x1e
#define KEY_B 0x30
#define KEY_C 0x2e
#define KEY_D 0x20
#define KEY_E 0x12
#define KEY_F 0x21
#define KEY_G 0x22
#define KEY_H 0x23
#define KEY_I 0x17
#define KEY_J 0x24
#define KEY_K 0x25
#define KEY_L 0x26
#define KEY_M 0x32
#define KEY_N 0x31
#define KEY_O 0x18
#define KEY_P 0x19
#define KEY_Q 0x10
#define KEY_R 0x13
#define KEY_S 0x1f
#define KEY_T 0x14
#define KEY_U 0x16
#define KEY_V 0x2f
#define KEY_W 0x11
#define KEY_X 0x2d
#define KEY_Y 0x15
#define KEY_Z 0x2c

#define KEY_0 0xb
#define KEY_1 0x2
#define KEY_2 0x3
#define KEY_3 0x4
#define KEY_4 0x5
#define KEY_5 0x6
#define KEY_6 0x7
#define KEY_7 0x8
#define KEY_8 0x9
#define KEY_9 0xa
#define KEY_NUM0 0x52
#define KEY_NUM1 0x4f
#define KEY_NUM2 0x50
#define KEY_NUM3 0x51
#define KEY_NUM4 0x4b
#define KEY_NUM5 0x4c
#define KEY_NUM6 0x4d
#define KEY_NUM7 0x47
#define KEY_NUM8 0x48
#define KEY_NUM9 0x49

#define KEY_F1 0x3b
#define KEY_F2 0x3c
#define KEY_F3 0x3d
#define KEY_F4 0x3e
#define KEY_F5 0x3f
#define KEY_F6 0x40
#define KEY_F7 0x41
#define KEY_F8 0x42
#define KEY_F9 0x43
#define KEY_F10 0x44
#define KEY_F11 0x57
#define KEY_F12 0x58

uint16 GetKeyboardState();
bool GetKeyUp(uint8 key);
bool GetKeyDown(uint8 key);
void UpdateKeyboard();
void ClearKeyboard();
bool GetKeyboardChar(wchar x);
wchar GetKeyboardChar();
wchar ReadKey();
// string ReadLine();

#endif