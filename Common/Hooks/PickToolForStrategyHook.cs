using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Common.Hooks
{
    public class PickToolForStrategyHook : ModHook
    {
        public override void Load() => On.Terraria.Player.SmartSelect_PickToolForStrategy += PickToolForStrategy;
        public override void Unload() => On.Terraria.Player.SmartSelect_PickToolForStrategy -= PickToolForStrategy;

        private static void PickToolForStrategy(On.Terraria.Player.orig_SmartSelect_PickToolForStrategy orig, Player player, int tX, int tY, int toolStrategy, bool wetTile)
        {
            // I hope it won't break anything :l

            if ((toolStrategy == 4 && wetTile) || toolStrategy == 5)
            {
                for (int i = 0; i < 50; i++)
                {
                    int type = player.inventory[i].type;
                    if (type != ModContent.ItemType<Content.Items.Tools.Misc.HyperlightFlareGun>()) continue;

                    bool flag = false;
                    for (int num = 57; num >= 0; num--)
                    {
                        if (player.inventory[num].ammo == player.inventory[i].useAmmo && player.inventory[num].stack > 0)
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        SmartSelect_SelectItem_Method?.Invoke(player, new object[] { i });
                        return;
                    }
                }
            }

            orig(player, tX, tY, toolStrategy, wetTile);
        }

        private static readonly MethodInfo SmartSelect_SelectItem_Method = typeof(Player).GetMethod("SmartSelect_SelectItem", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}