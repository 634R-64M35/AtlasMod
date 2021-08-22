using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Potions {
    public class BerserkTonic : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Berserk Tonic");
            Description.SetDefault("Grants +30% damage but reduces -20 defense");
        }

        public override void Update(Player player, ref int buffIndex) {
            player.allDamage += 0.30f;
            player.statDefense -= 20;
        }
    }
}
