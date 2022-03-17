using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CrowlerLib
{
    public static class Extensions
    {
        public static T[] ToArray<T>(this ConcurrentHashSet<T> set)
        {
            if (set == null)
                return null;

            T[] array = new T[set.Count];


            int idx = 0;

            foreach (T item in set)
            {
                array[idx++] = item;
            }

            return array;
        }

        public static void AddRange<T>(this ConcurrentQueue<T> queue, IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                queue.Enqueue(item);
            }
        }

        public static string[] Split(this string text, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] separators = { separator };

            return text.Split(separators, options);
        }

        public static bool StartsWithAny(this string text, params string[] args)
        {
            if (args == null)
                return false;

            foreach (string arg in args)
            {
                if (text.StartsWith(arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EndsWithAny(this string text, params string[] args)
        {
            if (args == null)
                return false;

            foreach (string arg in args)
            {
                if (text.EndsWith(arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool StartsWithAll(this string text, params string[] args)
        {
            if (args == null)
                return false;

            bool condition = false;

            foreach (string arg in args)
            {
                condition = true;

                if (!text.StartsWith(arg))
                {
                    return false;
                }
            }

            return condition;
        }
        
        public static string SkipLeft(this string text, int count)
        {
            if (count < 1)
                return text;

            if (count > text.Length)
                return string.Empty;

            return text.Substring(count);
        }

        public static string SkipRight(this string text, int count)
        {
            if (count < 1)
                return text;

            if (count > text.Length)
                return string.Empty;

            return text.Substring(0, text.Length - count);
        }

        public static string TakeLeft(this string text, int count)
        {
            if (count < 1)
                return string.Empty;

            if (count >= text.Length)
                return text;

            return text.Substring(0, count);
        }

        public static string TakeRight(this string text, int count)
        {
            // 0123456
            if (count < 1)
                return string.Empty;

            if (count >= text.Length)
                return text;

            return text.Substring(text.Length - count);
        }

        public static string TakeTextAfter(this string text, string searchFor, int ocurrences = 1)
        {
            string processedText = text;

            for (int ocurrence = 1; ocurrence <= ocurrences; ocurrence++)
            {
                int idxFound = processedText.IndexOf(searchFor, 0);

                if (idxFound < 0)
                    break;

                int idxCut = idxFound + searchFor.Length;
                processedText = processedText.SkipLeft(idxCut);
            }

            return processedText;
        }

        public static string TakeTextAfter(this string text, string[] searchForArgs)
        {
            string returnValue = text;

            if (searchForArgs == null)
                return text;

            foreach (string value in searchForArgs)
            {
                returnValue = returnValue.TakeTextAfter(value);
            }

            return returnValue;
        }

        public static string TakeTextBefore(this string text, string searchFor)
        {
            if (searchFor == null)
                return text;

            int idxOcurrence = text.IndexOf(searchFor);

            if (idxOcurrence == -1)
                return text;

            return text.Substring(0, idxOcurrence);
        }

        public static string TakeTextBefore(this string text, string[] searchForArgs)
        {
            if (searchForArgs == null)
                return text;

            int idxOcurrence = -1;

            foreach (string search in searchForArgs)
            {
                int idxNewOcurrence = text.IndexOf(search);

                if (idxOcurrence == -1 || idxNewOcurrence < idxOcurrence)
                {
                    idxOcurrence = idxNewOcurrence;
                }
            }

            if (idxOcurrence == -1)
                return text;

            return text.Substring(0, idxOcurrence);
        }

        public static string TakeTextBetween(this string text, string[] afterText, string[] beforeText)
        {
            string middle = text.TakeTextAfter(afterText);
            middle = middle.TakeTextBefore(beforeText);
            return middle;
        }

        
    }
}