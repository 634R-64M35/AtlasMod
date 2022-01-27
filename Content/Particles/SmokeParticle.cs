using AtlasMod.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Particles
{
    public class SmokeParticle : Particle
    {
        public SmokeParticle(int timeLeft, Vector2 position, Vector2? velocity = null, Color? color = null, float rotation = 1f, float scale = 1f) :
        base(ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Particles/SmokeParticle"), color ?? Color.LightGray, timeLeft, position, velocity, rotation, scale)
        { }

        public override void Update()
        {
            oldPosition = position;
            position += velocity;
            rotation += Main.rand.NextFloat(-0.5f, 0.5f);
            velocity *= 0.975f;
            scale += 0.03f;

            if (--timeLeft <= 0 || scale <= 0.2f) this.Kill();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var rect = new Rectangle((int)Main.screenPosition.X - 25, (int)Main.screenPosition.Y - 25, Main.screenWidth + 25, Main.screenHeight + 25);
            if (!rect.Contains((int)position.X, (int)position.Y)) return;

            Color color = Lighting.GetColor(position.ToTileCoordinates(), this.color) * (timeLeft / (float)InitTimeLeft) * 0.3f;
            spriteBatch.Draw(Texture.Value, position - Main.screenPosition, null, color, rotation, Texture.Size() * 0.5f, scale * 0.15f, SpriteEffects.None, 0f);
        }
    }
}
