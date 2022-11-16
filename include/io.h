#ifndef IO_H

#define IO_H
#include <definitions.h>

void outb(uint16 port, uint8 value);
void outw(uint16 port, uint16 value);
uint8 inb(uint16 port);
uint16 inw(uint16 port);

#endif