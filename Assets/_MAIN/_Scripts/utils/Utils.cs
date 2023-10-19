using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace THJ
{
    public static class Utils
    {
        public static void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + Random.Range(0, n - i);
                T temp = list[i];
                list[i] = list[r];
                list[r] = temp;
            }
        }
    }

}
