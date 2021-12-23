namespace System.Collections.Generic;

public static class CollectionExtensions
{
	public static void Deconstruct<T>(this IEnumerable<T> items, out T first, out T second)
	{
		using var enumerator = items.GetEnumerator();
		enumerator.MoveNext();
		first = enumerator.Current;
		enumerator.MoveNext();
		second = enumerator.Current;
	}
}
