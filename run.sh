mkdir -p iso/boot/grub/grub 2> /dev/null

cp grub.cfg iso/boot/grub
cp bin/kernel.bin iso/boot

grub-mkrescue iso -o bin/kernel.iso
qemu-system-x86_64 -display gtk -hda bin/kernel.iso -d int -no-reboot
