using AtlasMod.Common.Interfaces;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Common.PlayerDrawLayers
{
    public class HeldItemLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(Terraria.DataStructures.PlayerDrawLayers.HeldItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            var player = drawInfo.drawPlayer;
            var item = drawInfo.heldItem;

            bool usingItem = player.itemAnimation > 0 && item.useStyle != ItemUseStyleID.None;
            bool holdingSuitableItem = item.holdStyle != 0 && !player.pulley;

            if (!player.CanVisuallyHoldItem(item))
            {
                holdingSuitableItem = false;
            }

            bool flag = drawInfo.shadow != 0 || player.JustDroppedAnItem || player.frozen;
            flag |= !(usingItem || holdingSuitableItem) || item.type <= ItemID.None || player.dead;
            flag |= item.noUseGraphic || player.wet && item.noWet;
            flag |= player.happyFunTorchTime && player.HeldItem.createTile == TileID.Torches && player.itemAnimation == 0;

            return !flag;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            var player = drawInfo.drawPlayer;

            if (player.HeldItem.ModItem is IGlowingItem item)
            {
                item.DrawItemGlowmaskOnPlayer(ref drawInfo);
            }
        }
    }
}
