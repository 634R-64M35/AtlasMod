using AtlasMod.Content.Buffs.Potions;
using Terraria.ModLoader;

namespace AtlasMod.Common.Players {
    public class BuffPlayer : ModPlayer {
        public bool Polarity;

        public override void ResetEffects() => Polarity = false;

        public override void UpdateBadLifeRegen() {
            if (Polarity) {
                player.lifeRegen -= 50;

                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
            }
        }

        public override void PostUpdateBuffs() {
            bool firstMix = (player.HasBuff(ModContent.BuffType<HyperTonic>()) || player.HasBuff(ModContent.BuffType<BerserkTonic>())) && player.HasBuff(ModContent.BuffType<BulwarkTonic>());
            // bool secondMix = player.HasBuff(ModContent.BuffType<BerserkTonic>()) && player.HasBuff(ModContent.BuffType<MirrorTonic>());
            // bool thirdMix = player.HasBuff(ModContent.BuffType<IronBloodTonic>()) && player.HasBuff(ModContent.BuffType<MedicatedTonic>());

            if (firstMix) {
                
            }
        }
    }
}
