using AtlasMod.Common.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using SPladisonsYoyoMod.Common;
using System;
using System.Linq;
using Terraria;

namespace AtlasMod.Common.Hooks
{
    public class DrawDustHook : ModHook
    {
        public override void Load() => On.Terraria.Main.DrawDust += DrawDust;
        public override void Unload() => On.Terraria.Main.DrawDust -= DrawDust;

        private static void DrawDust(On.Terraria.Main.orig_DrawDust orig, Main main)
        {
            orig(main);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            {
                foreach (var proj in Main.projectile.ToList().FindAll(i => i.active && i.ModProjectile is IDrawAdditive))
                {
                    try
                    {
                        (proj.ModProjectile as IDrawAdditive).DrawAdditive();
                    }
                    catch (Exception e)
                    {
                        TimeLogger.DrawException(e);
                        proj.active = false;
                    }
                }

                foreach (var dust in ParticleSystem.Particles)
                {
                    dust.Draw(Main.spriteBatch);
                }
            }
            Main.spriteBatch.End();
        }
    }
}