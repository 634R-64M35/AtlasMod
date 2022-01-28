using AtlasMod.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace AtlasMod.Common.Hooks
{
    public class DrawPlayersBehindNPCsHook : ModHook
    {
        public override void Load() => On.Terraria.Main.DrawPlayers_BehindNPCs += DrawPlayersBehindNPCs;
        public override void Unload() => On.Terraria.Main.DrawPlayers_BehindNPCs -= DrawPlayersBehindNPCs;

        private static void DrawPlayersBehindNPCs(On.Terraria.Main.orig_DrawPlayers_BehindNPCs orig, Main main)
        {
            var spriteBatch = Main.spriteBatch;
            var pts = PrimitiveTrailSystem.Instance;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            foreach (var npc in Main.npc)
            {
                if (!npc.TryGetGlobalNPC(out MeticuliteBowMarkGlobalNPC global)) continue;

                try
                {
                    global.DrawMeticuliteMark(npc);
                }
                catch (Exception e)
                {
                    TimeLogger.DrawException(e);
                    npc.active = false;
                }
            }
            spriteBatch.End();

            if (pts != null)
            {
                pts.UpdateTransformMatrix();

                if (PrimitiveTrailSystem.AlphaBlendTrails.Count > 0)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    pts.DrawTrails(PrimitiveTrailSystem.AlphaBlendTrails);
                    spriteBatch.End();
                }

                if (PrimitiveTrailSystem.AdditiveTrails.Count > 0)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    pts.DrawTrails(PrimitiveTrailSystem.AdditiveTrails);
                    spriteBatch.End();
                }
            }

            orig(main);
        }
    }
}