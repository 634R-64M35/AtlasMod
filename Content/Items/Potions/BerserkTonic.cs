using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions {
    public class BerserkTonic : ModItem {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Berserk Tonic");

        public override void SetDefaults() {
            item.consumable = true;

            item.maxStack = 30;

            item.width = 30;
            item.height = 52;

            item.useTime = item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.EatingUsing;

            item.buffType = ModContent.BuffType<Buffs.Potions.BerserkTonic>();
            item.buffTime = 18000;

            item.UseSound = SoundID.Item3;
        }
    }
}
