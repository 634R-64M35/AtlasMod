using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Potions {
    public class HyperTonic : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Hyper Tonic");
            Description.SetDefault("Grants +50% movement speed but reduces -20 defense");
        }

        public override void Update(Player player, ref int buffIndex) {
            player.moveSpeed += 0.50f;
            player.statDefense -= 20;
        }
    }
}
