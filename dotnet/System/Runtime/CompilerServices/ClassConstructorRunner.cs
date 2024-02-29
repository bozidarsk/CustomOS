// using System;

// namespace System.Runtime.CompilerServices;

// internal static class ClassConstructorRunner 
// {
// 	private static unsafe object CheckStaticClassConstructionReturnGCStaticBase(StaticClassConstructionContext* context, object gcStaticBase) 
// 	{
// 		EnsureClassConstructorRun(context);
// 		return gcStaticBase;
// 	}

// 	private static unsafe IntPtr CheckStaticClassConstructionReturnNonGCStaticBase(StaticClassConstructionContext* context, IntPtr nonGcStaticBase) 
// 	{
// 		EnsureClassConstructorRun(context);
// 		return nonGcStaticBase;
// 	}

// 	public static unsafe void EnsureClassConstructorRun(StaticClassConstructionContext* context) {}
// 	// {
// 	// 	if (context->cctorMethodAddress == IntPtr.Zero)
// 	// 		return;

// 	// 	((delegate*<void>)(context->cctorMethodAddress))();
// 	// 	context->cctorMethodAddress = IntPtr.Zero;
// 	// }
// }
