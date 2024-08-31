using System.Collections.Generic;

namespace System.Linq;

#pragma warning disable CS8600

public static partial class Enumerable 
{
	public static TSource Single<TSource>(this IEnumerable<TSource> source) 
	{
		if (source == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);

		TSource value = default;
		bool hasValue = false;

		foreach (var x in source) 
		{
			if (hasValue)
				ThrowHelper.ThrowMoreThanOneElementException();

			if (!hasValue) 
			{
				hasValue = true;
				value = x;
			}
		}

		if (!hasValue)
			ThrowHelper.ThrowNoElementsException();

		return value;
	}

	public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) 
	{
		if (source == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
		if (predicate == null)
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.predicate);

		TSource value = default;
		bool hasValue = false;

		foreach (var x in source) 
		{
			if (hasValue && predicate(x))
				ThrowHelper.ThrowMoreThanOneElementException();

			if (!hasValue && predicate(x)) 
			{
				hasValue = true;
				value = x;
			}
		}

		if (!hasValue)
			ThrowHelper.ThrowNoElementsException();

		return value;
	}
}
