#nullable enable

using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Immutable;
using AutoGenerateXML;

namespace AutoGenerateXML
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Executes an action that modifies the collection on each element (such as removing items from the list).
        /// Creates a temporary list, unless the collection is empty.
        /// </summary>
        public static void ForEachMod<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source.None()) { return; }
            var temp = new List<T>(source);
            temp.ForEach(action);
        }

        /// <summary>
        /// Generic version of List.ForEach.
        /// Performs the specified action on each element of the collection (short hand for a foreach loop).
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Iterates over all elements in a given enumerable and discards the result.
        /// </summary>
        public static void Consume<T>(this IEnumerable<T> enumerable)
        {
            foreach (var _ in enumerable) { /* do nothing */ }
        }

        /// <summary>
        /// Shorthand for !source.Any(predicate) -> i.e. not any.
        /// </summary>
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool>? predicate = null)
        {
            if (predicate == null)
            {
                return !source.Any();
            }
            else
            {
                return !source.Any(predicate);
            }
        }

        public static bool Multiple<T>(this IEnumerable<T> source, Func<T, bool>? predicate = null)
        {
            if (predicate == null)
            {
                return source.Count() > 1;
            }
            else
            {
                return source.Count(predicate) > 1;
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                {
                    yield return item;
                }
            }
        }

        // source: https://stackoverflow.com/questions/19237868/get-all-children-to-one-list-recursive-c-sharp
        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var result = source.SelectMany(selector);
            if (!result.Any())
            {
                return result;
            }
            return result.Concat(result.SelectManyRecursive(selector));
        }

        public static void AddIfNotNull<T>(this IList<T> source, T value)
        {
            if (value != null) { source.Add(value); }
        }

        /// <summary>
        /// Returns whether a given collection has at least a certain amount
        /// of elements for which the predicate returns true.
        /// </summary>
        /// <param name="source">Input collection</param>
        /// <param name="amount">How many elements to match before stopping</param>
        /// <param name="predicate">Predicate used to evaluate the elements</param>
        public static bool AtLeast<T>(this IEnumerable<T> source, int amount, Predicate<T> predicate)
        {
            foreach (T elem in source)
            {
                if (predicate(elem)) { amount--; }
                if (amount <= 0) { return true; }
            }
            return false;
        }
    }
}
