using System.Collections.Generic;

namespace System.Linq;

public static partial class Enumerable 
{
	public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) 
	{
		if (source == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
		if (selector == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.selector);

		foreach (var x in source)
			yield return selector(x);
	}

	public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) 
	{
		if (source == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
		if (selector == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.selector);

		int index = 0;
		foreach (var x in source)
			yield return selector(x, index++);
	}
}
