using System;
using System.Collections.Generic;

namespace Util
{
    public static class CollecitonEx
    {
        public static void DeleteRange<T>(ref T[] self, int fromInclusive, int toExclusive)
        {
            var diff = toExclusive - fromInclusive;

            for (int i = fromInclusive; i < self.Length - diff; i++)
            {
                // moving elements downwards, to fill the gap at [index]
                self[i] = self[i + diff];
            }
            Array.Resize(ref self, self.Length - diff);
        }

        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
        {
            TV value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}