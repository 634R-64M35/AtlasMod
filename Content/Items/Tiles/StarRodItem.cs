using AtlasMod.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Tiles {
    public class StarRodItem : ModItem {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Tiles/StarRodItem";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Star Rod");
            Tooltip.SetDefault("Attracts the products of ancient celestial clashes. Most useful at moderate altitudes\n" +
                "Will attract a heavenly asteroid into atmosphere upon placement\n" +
                "Will continue to scan and attract asteroids as long as its placed");
        }

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 40;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<StarRod>();
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 15)
                .AddIngredient(ItemID.IronBar, 15)
                .AddIngredient(ItemID.Diamond, 1)
                .AddIngredient(ItemID.Amber, 1)
                .AddIngredient(ItemID.Ruby, 1)
                .AddIngredient(ItemID.Emerald, 1)
                .AddIngredient(ItemID.Sapphire, 1)
                .AddIngredient(ItemID.Topaz, 1)
                .AddIngredient(ItemID.Amethyst, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}