using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Materials {
    public class MeticuliteOre : ModItem {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Materials/MeticuliteOre";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Meticulite");
            Tooltip.SetDefault("Created from heavenly stardust colliding with space debris");
            this.SacrificeTotal = 5;
        }

        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(platinum: 0, gold: 0, silver: 25, copper: 20);
        }
    }
}
