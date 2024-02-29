// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System;

public abstract class Delegate 
{
	internal object? m_firstParameter;
	internal object? m_helperObject;
	internal nint m_extraFunctionPointerOrData;
	internal IntPtr m_functionPointer;

	// WARNING: These constants are also declared in System.Private.TypeLoader\Internal\Runtime\TypeLoader\CallConverterThunk.cs
	// Do not change their values without updating the values in the calling convention converter component
	private protected const int MulticastThunk = 0;
	private protected const int ClosedStaticThunk = 1;
	private protected const int OpenStaticThunk = 2;
	private protected const int ClosedInstanceThunkOverGenericMethod = 3; // This may not exist
	private protected const int OpenInstanceThunk = 4; // This may not exist
	private protected const int ObjectArrayThunk = 5; // This may not exist

	// This function is known to the compiler backend.
	private void InitializeClosedStaticThunk(object firstParameter, IntPtr functionPointer, IntPtr functionPointerThunk) 
	{
		m_extraFunctionPointerOrData = functionPointer;
		m_helperObject = firstParameter;
		m_functionPointer = functionPointerThunk;
		m_firstParameter = this;
	}

	// This function is known to the compiler backend.
	private void InitializeOpenStaticThunk(object _ /*firstParameter*/, IntPtr functionPointer, IntPtr functionPointerThunk) 
	{
		// This sort of delegate is invoked by calling the thunk function pointer with the arguments to the delegate + a reference to the delegate object itself.
		m_firstParameter = this;
		m_functionPointer = functionPointerThunk;
		m_extraFunctionPointerOrData = functionPointer;
	}

	private void InitializeClosedInstance(object firstParameter, IntPtr functionPointer) 
	{
		if (firstParameter == null)
			throw new ArgumentNullException();

		m_firstParameter = firstParameter;
		m_functionPointer = functionPointer;
	}
}
