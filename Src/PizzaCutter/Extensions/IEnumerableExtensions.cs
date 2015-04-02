using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaCutter.Extensions
{
	public static class IEnumerableExtensions
	{
		public static T ShuffleFirst<T>(this IEnumerable<T> source)
		{
			return source.Shuffle().FirstOrDefault();
		}

		public static T ShuffleFirst<T>(this IEnumerable<T> source, Random rng)
		{
			return source.Shuffle(rng).FirstOrDefault();
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			var rng = new Random();
			return source.Shuffle(rng);
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (rng == null) throw new ArgumentNullException("rng");

			return source.ShuffleIterator(rng);
		}

		private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
		{
			var buffer = source.ToList();

			for (int i = 0; i < buffer.Count; i++)
			{
				int j = rng.Next(i, buffer.Count);
				yield return buffer[j];

				buffer[j] = buffer[i];
			}
		}
	}
}
