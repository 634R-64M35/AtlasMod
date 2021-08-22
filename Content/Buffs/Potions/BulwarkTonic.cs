using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Potions {
    public class BulwarkTonic : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Bulwark Tonic");
            Description.SetDefault("Grants +20 defense but reduces -50% movement speed");
        }

        public override void Update(Player player, ref int buffIndex) {
            player.statDefense += 20;
            player.moveSpeed -= 0.50f;
        }
    }
}
