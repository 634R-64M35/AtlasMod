using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions {
    public class HyperTonic : ModItem {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Hyper Tonic");

        public override void SetDefaults() {
            item.consumable = true;

            item.maxStack = 30;

            item.width = 26;
            item.height = 46;

            item.useTime = item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.EatingUsing;

            item.buffType = ModContent.BuffType<Buffs.Potions.HyperTonic>();
            item.buffTime = 18000;

            item.UseSound = SoundID.Item3;
        }
    }
}
