using AtlasMod.Content.Buffs.Potions;
using Terraria.ModLoader;

namespace AtlasMod.Common.Players
{
    public class BuffPlayer : ModPlayer
    {
        public bool polarity;

        public override void ResetEffects() => polarity = false;

        public override void UpdateBadLifeRegen()
        {
            if (polarity)
            {
                Player.lifeRegen -= 50;

                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }

                Player.lifeRegenTime = 0;
            }
        }

        public override void PostUpdateBuffs()
        {
            bool polarityFlag = false;
            polarityFlag |= (Player.HasBuff(ModContent.BuffType<HyperTonicBuff>()) || Player.HasBuff(ModContent.BuffType<BerserkTonicBuff>())) && Player.HasBuff(ModContent.BuffType<BulwarkTonicBuff>());
            // polarityFlag |= player.HasBuff(ModContent.BuffType<BerserkTonic>()) && player.HasBuff(ModContent.BuffType<MirrorTonic>());
            // polarityFlag |= player.HasBuff(ModContent.BuffType<IronBloodTonic>()) && player.HasBuff(ModContent.BuffType<MedicatedTonic>());

            if (polarityFlag)
            {
                Player.AddBuff(ModContent.BuffType<Content.Buffs.Debuffs.PolarityBuff>(), 1);
            }
        }
    }
}
