using System.Collections.Generic;

namespace System.Linq;

public static partial class Enumerable 
{
	public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) 
	{
		if (source == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
		if (predicate == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.predicate);

		foreach (var x in source)
			if (predicate(x))
				yield return x;
	}

	public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) 
	{
		if (source == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
		if (predicate == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.predicate);

		int index = 0;
		foreach (var x in source)
			if (predicate(x, index))
				yield return x;
	}
}
