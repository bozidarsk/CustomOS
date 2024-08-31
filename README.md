# Custom OS Kernel
The build, run and clean scripts are made for linux. (for Windows see below)<br/>

## Dependencies
### Make
- binutils
- dotnet-sdk
- lttng-ust
- nasm
### Run
- qemu-system-x86
- qemu-ui-gtk
- grub-mkrescue

## Working in Windows
Based on [this](https://www.youtube.com/watch?v=4SZXbl9KVsw) tutorial.<br/>

First you will need Windows Subsystem for Linux installed. (wsl)<br/>
Then download [VcXsrv](https://sourceforge.net/projects/vcxsrv).<br/>
Run xlaunch.exe<br/>
Go to your linux instance and run this command:
``export DISPLAY=127.0.0.1:0``<br/>

Now you can run linux GUI apps in Windows using wsl. (in this case qemu)
