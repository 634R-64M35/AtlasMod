using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions
{
    public class BerserkTonic : TonicItem
    {
        public BerserkTonic() : base(
            displayName: "Berserk Tonic",
            tooltip: "Increases damage by 30%, but reduces defense by 20",
            buffType: ModContent.BuffType<Buffs.Potions.BerserkTonicBuff>(),
            buffTime: 300 * 60,
            particleColors: new Color[]
            {
                new Color(245, 23, 23),
                new Color(231, 128, 72),
                new Color(230, 159, 21)
            }
        )
        { }

        public override void TonicSetDefaults()
        {
            Item.width = 30;
            Item.height = 52;
        }
    }
}
