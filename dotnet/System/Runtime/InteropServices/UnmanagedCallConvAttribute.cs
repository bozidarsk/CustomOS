namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class UnmanagedCallConvAttribute : Attribute
{
    public CallingConvention CallingConvention;

    public UnmanagedCallConvAttribute(CallingConvention callingConvention) 
    {
        this.CallingConvention = callingConvention;
    }
}
