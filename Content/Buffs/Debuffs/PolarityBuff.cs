using AtlasMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Debuffs
{
    public class PolarityBuff : ModBuff
    {
        public override string Texture => AtlasMod.AssetPath + "Textures/Buffs/" + this.Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polarity");
            Description.SetDefault("Suffering from a lethal tonic reaction!");

            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.LightPurple;
        }

        public override void Update(Player player, ref int buffIndex) => player.GetModPlayer<BuffPlayer>().polarity = true;
    }
}