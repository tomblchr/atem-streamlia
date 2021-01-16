using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public static class BlackMagicDesignSdk
    {
        public static double[] ConvertDoubleArray(uint length, ref double values)
        {
            return ConvertArray(length, ref values);
        }

        public static T[] ConvertArray<T>(uint length, ref T values)
        {
            var result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = System.Runtime.CompilerServices.Unsafe.Add(ref values, i);
            }
            return result;
        }
    }
}
