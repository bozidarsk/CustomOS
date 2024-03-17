mkdir -p iso/boot/grub/grub 2> /dev/null

cp grub.cfg iso/boot/grub
cp bin/kernel.bin iso/boot

if [[ $(ls bin/drive.bin) == '' ]]; then dd if=/dev/zero of=bin/drive.bin bs=1M count=500; fi

grub-mkrescue iso -o bin/kernel.iso
qemu-system-x86_64 -display gtk -drive id=kernel,file=bin/kernel.iso -drive id=drive,file=bin/drive.bin -device ich9-ahci -d int -no-reboot
