using System.Runtime.CompilerServices;

namespace Internal.Runtime;

internal static class WellKnownEETypes 
{
	// Returns true if the passed in MethodTable is the MethodTable for System.Object
	// This is recognized by the fact that System.Object and interfaces are the only ones without a base type
	internal static unsafe bool IsSystemObject(MethodTable* pEEType) 
	{
		if (pEEType->IsArray)
			return false;
		return (pEEType->NonArrayBaseType == null) && !pEEType->IsInterface;
	}

	// Returns true if the passed in MethodTable is the MethodTable for System.Array or System.Object.
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static unsafe bool IsValidArrayBaseType(MethodTable* pEEType) 
	{
		EETypeElementType elementType = pEEType->ElementType;
		return elementType == EETypeElementType.SystemArray
			|| (elementType == EETypeElementType.Class && pEEType->NonArrayBaseType == null);
	}
}
