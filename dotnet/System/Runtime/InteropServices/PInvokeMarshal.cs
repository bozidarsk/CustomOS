using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

public static class PInvokeMarshal 
{
	/// <summary>
    /// Retrieves the function pointer for the current open static delegate that is being called
    /// </summary>
    public static IntPtr GetCurrentCalleeOpenStaticDelegateFunctionPointer()
    {
        //
        // RH keeps track of the current thunk that is being called through a secret argument / thread
        // statics. No matter how that's implemented, we get the current thunk which we can use for
        // look up later
        //
        IntPtr pContext = RuntimeImports.GetCurrentInteropThunkContext();
        Debug.Assert(pContext != IntPtr.Zero);

        IntPtr fnPtr;
        unsafe
        {
            // Pull out function pointer for open static delegate
            fnPtr = ((ThunkContextData*)pContext)->FunctionPtr;
        }
        Debug.Assert(fnPtr != IntPtr.Zero);

        return fnPtr;
    }
	
	/// <summary>
	/// Retrieves the current delegate that is being called
	/// </summary>
	public static T GetCurrentCalleeDelegate<T>() where T : class // constraint can't be System.Delegate
	{
		//
		// RH keeps track of the current thunk that is being called through a secret argument / thread
		// statics. No matter how that's implemented, we get the current thunk which we can use for
		// look up later
		//
		IntPtr pContext = RuntimeImports.GetCurrentInteropThunkContext();

		Debug.Assert(pContext != IntPtr.Zero);

		GCHandle handle;
		unsafe
		{
			// Pull out Handle from context
			#pragma warning disable CS8500
			handle = ((ThunkContextData*)pContext)->Handle;
			#pragma warning restore
		}

		T target = Unsafe.As<T>(handle.Target);

		//
		// The delegate might already been garbage collected
		// User should use GC.KeepAlive or whatever ways necessary to keep the delegate alive
		// until they are done with the native function pointer
		//
		if (target == null)
		{
			Environment.FailFast("SR.Delegate_GarbageCollected");
		}
		return target;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct ThunkContextData
    {
        public GCHandle Handle;        //  A weak GCHandle to the delegate
        public IntPtr FunctionPtr;     // Function pointer for open static delegates
    }
}
