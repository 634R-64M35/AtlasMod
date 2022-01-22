using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions
{
    public class HyperTonic : TonicItem
    {
        public HyperTonic() : base(
            displayName: "Hypertonic",
            tooltip: "Increases movement speed by 50%, but reduces defense by 20",
            buffType: ModContent.BuffType<Buffs.Potions.HyperTonicBuff>(),
            buffTime: 300 * 60,
            particleColors: new Color[]
            {
                new Color(10, 184, 223),
                new Color(41, 227, 89),
                new Color(108, 60, 130)
            }
        )
        { }

        public override void TonicSetDefaults()
        {
            Item.width = 26;
            Item.height = 46;
        }
    }
}
