namespace System;

public sealed class AttributeUsageAttribute : Attribute
{
	public AttributeTargets ValidOn { set; get; }
	public bool AllowMultiple { set; get; }
	public bool Inherited { set; get; }

	public AttributeUsageAttribute(AttributeTargets validOn) => this.ValidOn = validOn;
}
