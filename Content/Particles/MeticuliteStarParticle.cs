using AtlasMod.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AtlasMod.Content.Particles {
    public class MeticuliteStarParticle : Particle {
        public MeticuliteStarParticle(Color color, int timeLeft, Vector2 position, Vector2? velocity = null, float rotation = 1f, float scale = 1f) :
        base(ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_3"), color, timeLeft, position, velocity, rotation, scale)
        { }

        public override void Update() {
            oldPosition = position;
            position += velocity;
            velocity *= 0.975f;
            scale -= 0.05f;
            rotation += 0.05f;

            if (--timeLeft <= 0 || scale <= 0.2f) this.Kill();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            var rect = new Rectangle((int)Main.screenPosition.X - 25, (int)Main.screenPosition.Y - 25, Main.screenWidth + 25, Main.screenHeight + 25);
            if (!rect.Contains((int)position.X, (int)position.Y)) return;

            Color color = this.color * (timeLeft / (float)InitTimeLeft);
            spriteBatch.Draw(Texture.Value, position - Main.screenPosition, null, color * 0.5f, rotation, Texture.Size() * 0.5f, scale * 0.3f, SpriteEffects.None, 0f);
        }
    }
}
