using Terraria.DataStructures;

namespace AtlasMod.Common.Interfaces
{
    public interface IGlowingItem
    {
        void DrawItemGlowmaskOnPlayer(ref PlayerDrawSet drawInfo);
    }
}
