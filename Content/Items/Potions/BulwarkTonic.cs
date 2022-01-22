using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions
{
    public class BulwarkTonic : TonicItem
    {
        public BulwarkTonic() : base(
            displayName: "Bulwark Tonic",
            tooltip: "Increases defense by 20, but reduces movement speed by 50%",
            buffType: ModContent.BuffType<Buffs.Potions.BulwarkTonicBuff>(),
            buffTime: 300 * 60,
            particleColors: new Color[]
            {
                new Color(206, 116, 27),
                new Color(197, 230, 64),
                new Color(120, 21, 111)
            }
        )
        { }

        public override void TonicSetDefaults()
        {
            Item.width = 30;
            Item.height = 42;
        }
    }
}
