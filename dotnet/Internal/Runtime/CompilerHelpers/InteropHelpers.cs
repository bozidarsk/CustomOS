using System;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers;

internal static class InteropHelpers 
{
	/// <summary>
    /// Retrieves the function pointer for the current open static delegate that is being called
    /// </summary>
    public static IntPtr GetCurrentCalleeOpenStaticDelegateFunctionPointer()
    {
        return PInvokeMarshal.GetCurrentCalleeOpenStaticDelegateFunctionPointer();
    }
	
	/// <summary>
	/// Retrieves the current delegate that is being called
	/// </summary>
	public static T GetCurrentCalleeDelegate<T>() where T : class // constraint can't be System.Delegate
	{
		return PInvokeMarshal.GetCurrentCalleeDelegate<T>();
	}
}
