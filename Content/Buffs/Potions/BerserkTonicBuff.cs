using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Potions
{
    public class BerserkTonicBuff : TonicBuff
    {
        public BerserkTonicBuff() : base(
            displayName: "Berserk Tonic",
            tooltip: "Increases damage by 30%, but reduces defense by 20"
        )
        { }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += 0.30f;
            player.statDefense -= 20;
        }
    }
}
