using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Materials
{
    public class MeticuliteCompound : ModItem
    {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Materials/MeticuliteCompound";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meticulite Compound");

            this.SacrificeTotal = 25;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(platinum: 0, gold: 0, silver: 0, copper: 0);
        }
    }
}