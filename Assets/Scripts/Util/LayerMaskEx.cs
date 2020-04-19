using UnityEngine;

namespace Util
{
    public static class LayerMaskEx
    {
        public static LayerMask LayerIndexToLayerMask(int index)
        {
            return 1 << index;
        }

        public static int LayerMaskToLayerIndex(LayerMask mask)
        {
            int idx = -1;
            int bmap = mask.value;
            while (bmap != 0)
            {
                bmap >>= 1;
                ++idx;
            }

            return idx;
        }

        public static bool Contains(this LayerMask lhs, LayerMask rhs)
        {
            return (lhs.value & rhs.value) == rhs.value;
        }

        public static bool Contains(this LayerMask self, int layerId)
        {
            return self.Contains(LayerIndexToLayerMask(layerId));
        }
    }
}