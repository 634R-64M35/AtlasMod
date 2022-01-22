using Terraria;

namespace AtlasMod.Content.Buffs.Potions
{
    public class BulwarkTonicBuff : TonicBuff
    {
        public BulwarkTonicBuff() : base(
            displayName: "Bulwark Tonic",
            tooltip: "Increases defense by 20, but reduces movement speed by 50%"
        )
        { }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 20;
            player.moveSpeed -= 0.50f;
        }
    }
}
