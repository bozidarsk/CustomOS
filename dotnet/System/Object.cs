using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System;

[StructLayout(LayoutKind.Sequential)]
internal class RawData 
{
	public byte Data;
}

public unsafe class Object 
{
	#pragma warning disable CS0649
	private MethodTable* pEEType;
	#pragma warning restore

	public virtual string ToString() => "System.Object";
	public virtual bool Equals(object? other) => this == other;
	public virtual int GetHashCode() => this.ToString().GetHashCode();
	// [Intrinsic] public virtual Type GetType() => Type.GetTypeFromMethodTable(pEEType);

	public Object() {}
	~Object() {}

	internal MethodTable* MethodTable => pEEType;
	internal EETypePtr EETypePtr => new EETypePtr(pEEType);

	[Intrinsic]
	protected internal object MemberwiseClone() 
	{
		object clone = 
			this.EETypePtr.IsArray
			? RuntimeHelpers.RhpNewArray(this.MethodTable, Unsafe.As<Array>(this).Length)
			: RuntimeHelpers.RhpNewFast(this.MethodTable)
		;

		// copy contents of "this" to the clone

		ulong byteCount = MethodTable->BaseSize - (ulong)(2 * sizeof(IntPtr));
		if (MethodTable->HasComponentSize)
			byteCount += (ulong)Unsafe.As<RawArrayData>(this).Length * (ulong)MethodTable->ComponentSize;

		// nuint byteCount = RuntimeHelpers.GetRawObjectDataSize(this);
		ref byte src = ref this.GetRawData();
		ref byte dst = ref clone.GetRawData();

		Platform.CopyMemory((IntPtr)Unsafe.AsPointer<byte>(ref dst), (IntPtr)Unsafe.AsPointer<byte>(ref src), byteCount);

		return clone;
	}

	/// <summary>
	/// Return beginning of all data (excluding ObjHeader and MethodTable*) within this object.
	/// Note that for strings/arrays this would include the Length as well.
	/// </summary>
	internal ref byte GetRawData() => ref Unsafe.As<RawData>(this).Data;

	// public void Finalize() 
	// {
	// 	var obj = this;
	// 	Platform.Free(Unsafe.As<object, IntPtr>(ref obj));
	// }
}
