using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Kernel;

public static class StackTrace 
{
	[Import] private static extern ulong getrbp();

	public static IEnumerable<StackFrame> GetFrames() 
	{
		StackFrameWrapper wrapper = new((nint)getrbp());

		do 
		{
			yield return wrapper.Frame;
			wrapper = new(wrapper.Frame.BasePointer);
		} while (wrapper.Frame.BasePointer != 0);
	}
}

public unsafe struct StackFrameWrapper 
{
	public StackFrame* frame;
    
    public StackFrame Frame => *frame;
    public nint BasePointer => frame->BasePointer;
    public nint InstructionPointer => frame->InstructionPointer;
    
    public StackFrameWrapper(nint frame) => this.frame = (StackFrame*)frame;
}

public struct StackFrame 
{
	public nint BasePointer;
	public nint InstructionPointer;

	// public string SymbolName => ELF.ELF.GetSymbolName(InstructionPointer, out string file, out string section);

	// public unsafe StackFrame PreviousFrame => *((StackFrame*)this.BasePointer);

	// public StackFrame(uint basepointer) : this((nint)basepointer) {}
	// public StackFrame(ulong basepointer) : this((nint)basepointer) {}
	// public StackFrame(nint basepointer) 
	// {
	// 	this.BasePointer = ((nint*)basepointer)[0];
	// 	this.InstructionPointer = ((nint*)basepointer)[1];
	// }
}
