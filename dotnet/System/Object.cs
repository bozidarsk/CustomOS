using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Internal.Runtime;

namespace System;

[StructLayout(LayoutKind.Sequential)]
internal class RawData 
{
	public byte Data;
}

public unsafe class Object 
{
	private MethodTable* m_pEEType;

	public virtual string ToString() => GetType().ToString();
	public virtual bool Equals(object? other) => this == other;
	public virtual int GetHashCode() => this.ToString().GetHashCode();
	public virtual Type GetType() => Type.GetTypeFromMethodTable(m_pEEType);

	public Object() {}
	~Object() {}

	/// <summary>
	/// Return beginning of all data (excluding ObjHeader and MethodTable*) within this object.
	/// Note that for strings/arrays this would include the Length as well.
	/// </summary>
	internal ref byte GetRawData() => ref Unsafe.As<RawData>(this).Data;

	/// <summary>
	/// Return size of all data (excluding ObjHeader and MethodTable*).
	/// Note that for strings/arrays this would include the Length as well.
	/// </summary>
	internal uint GetRawDataSize() => GetMethodTable()->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(MethodTable*);

	internal unsafe MethodTable* GetMethodTable() => m_pEEType;
	internal unsafe ref MethodTable* GetMethodTableRef() => ref m_pEEType;
	internal unsafe EETypePtr GetEETypePtr() => new EETypePtr(m_pEEType);

	[Intrinsic]
	protected internal object MemberwiseClone() 
	{
		object clone = 
			GetEETypePtr().IsArray
			? InternalCalls.RhpNewArray(GetMethodTable(), Unsafe.As<Array>(this).Length)
			: InternalCalls.RhpNewFast(GetMethodTable())
		;

		// copy contents of "this" to the clone

		uint byteCount = GetMethodTable()->BaseSize - (uint)(2 * sizeof(nint));
		if (GetMethodTable()->HasComponentSize)
			byteCount += (uint)Unsafe.As<RawArrayData>(this).Length * (uint)GetMethodTable()->ComponentSize;

		// nuint byteCount = RuntimeHelpers.GetRawObjectDataSize(this);
		ref byte src = ref this.GetRawData();
		ref byte dst = ref clone.GetRawData();

		Platform.CopyMemory(ref dst, ref src, byteCount);

		return clone;
	}
}
