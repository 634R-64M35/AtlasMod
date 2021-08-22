using AtlasMod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Debuffs {
    public class Polarity : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Polarity");
            Description.SetDefault("Suffering from a lethal tonic reaction!");

            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) => player.GetModPlayer<BuffPlayer>().Polarity = true;
    }
}
