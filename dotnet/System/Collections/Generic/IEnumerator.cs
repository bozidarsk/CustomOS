using System.Collections;

namespace System.Collections.Generic;

public interface IEnumerator<out T> : IEnumerator
{
	new T? Current { get; }
}
