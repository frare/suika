using System.Collections.Generic;
using UnityEngine;

namespace frare.CollectionsExtensions
{
    public static class CollectionsExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            return list[Random.Range(0, list.Count)];
        }

        public static T GetRandom<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return default;

            return array[Random.Range(0, array.Length)];
        }

        public static List<T> GetRandomMultiple<T>(this List<T> list, int count)
        {
            if (list == null || list.Count == 0)
                return default;

            List<T> result = new();
            for (int i = 0; i < count; i++)
            {
                result.Add(list.GetRandom());
            }
            return result;
        }

        public static T[] GetRandomMultiple<T>(this T[] array, int count)
        {
            if (array == null || array.Length == 0)
                return default;

            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = array.GetRandom();
            }
            return result;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            List<T> result = list;
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);

                (result[i], result[j]) = (list[j], list[i]);
            }

            return result;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return default;

            T[] result = new T[array.Length];
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);

                (result[i], result[j]) = (array[j], array[i]);
            }

            return result;
        }
    }
}