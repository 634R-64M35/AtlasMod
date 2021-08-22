using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions {
    public class BulwarkTonic : ModItem {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Bulwark Tonic");

        public override void SetDefaults() {
            item.consumable = true;

            item.maxStack = 30;

            item.width = 30;
            item.height = 42;

            item.useTime = item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.EatingUsing;

            item.buffType = ModContent.BuffType<Buffs.Potions.BulwarkTonic>();
            item.buffTime = 18000;

            item.UseSound = SoundID.Item3;
        }
    }
}
