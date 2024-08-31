using System;
using System.Collections;

namespace System.Collections.Generic;

public interface IEnumerator<out T> : IDisposable, IEnumerator
{
	new T? Current { get; }
}
