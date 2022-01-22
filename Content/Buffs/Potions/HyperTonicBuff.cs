using Terraria;

namespace AtlasMod.Content.Buffs.Potions
{
    public class HyperTonicBuff : TonicBuff
    {
        public HyperTonicBuff() : base(
            displayName: "Hypertonic",
            tooltip: "Increases movement speed by 50%, but reduces defense by 20"
        )
        { }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.50f;
            player.statDefense -= 20;
        }
    }
}