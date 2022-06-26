using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Materials {
    public class MeticuliteCompound : ModItem {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Materials/MeticuliteCompound";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Meticulite Compound");
            Tooltip.SetDefault("A heavenly alloy, handle with care");
            this.SacrificeTotal = 25;
        }

        public override void SetDefaults() {
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(platinum: 0, gold: 0, silver: 55, copper: 60);
        }
        public override void AddRecipes() {
            ModContent.GetInstance<MeticuliteCompound>().CreateRecipe()
                .AddIngredient<MeticuliteOre>(3)
                .AddIngredient(ItemID.FallenStar)
                .AddRecipeGroup("AnyCloudType", 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}