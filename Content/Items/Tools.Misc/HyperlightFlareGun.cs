using AtlasMod.Common;
using AtlasMod.Common.Interfaces;
using AtlasMod.Content.Particles;
using AtlasMod.Content.Trails;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Tools.Misc
{
    public class HyperlightFlareGun : ModItem
    {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Tools/HyperlightFlareGun";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperlight Flare Gun");
            Tooltip.SetDefault("66% chance to not consume flare\n" +
                "'Afraid of the dark? No need, you got me!'");
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 28;

            Item.damage = 12;
            Item.crit = 0;
            Item.knockBack = 0;

            Item.useAmmo = AmmoID.Flare;
            Item.shoot = ProjectileID.Flare;
            Item.shootSpeed = 8f;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(platinum: 0, gold: 0, silver: 0, copper: 0);

            Item.autoReuse = true;
            Item.useStyle = 5;
            Item.useTime = Item.useAnimation = 13;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
        }

        public override bool CanConsumeAmmo(Player player) => Main.rand.NextBool(3);

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var flareType = type == ProjectileID.BlueFlare ? 1 : 0;
            type = ModContent.ProjectileType<HyperlightFlareGunProjectile>();

            var proj = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
            (proj.ModProjectile as HyperlightFlareGunProjectile).OnSpawn(flareType);

            for (int i = 0; i < 5; i++)
            {
                var particle = new HyperlightFlareGunParticle(
                    color: HyperlightFlareGunProjectile.Colors[flareType],
                    timeLeft: 60,
                    position: position + Vector2.Normalize(velocity) * 45,
                    velocity: velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.15f, 0.3f),
                    scale: Main.rand.NextFloat(0.7f, 0.9f)
                );
                ParticleSystem.NewParticle(particle);
            }

            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 2);

        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FlareGun);
            recipe.AddIngredient<Materials.MeticuliteCompound>(3);
            recipe.AddIngredient(ItemID.Topaz, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    public class HyperlightFlareGunProjectile : ModProjectile, IDrawAdditive
    {
        public static readonly Color[] Colors = new Color[] { new Color(213, 46, 17), new Color(66, 188, 239) };

        private int _flareType;
        private Trail _trail;

        public override string Texture => AtlasMod.AssetPath + "Textures/Projectiles/HyperlightFlareGunProjectile";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hyperlight Flare Gun");
        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;

            Projectile.netImportant = true;
            Projectile.aiStyle = 33;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 36000 * 2;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
        }

        public override bool PreAI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.ai[0] = Projectile.velocity.X;
                Projectile.ai[1] = Projectile.velocity.Y;
                Projectile.localAI[1] += 1f;

                if (Projectile.localAI[1] >= 30f)
                {
                    Projectile.velocity.Y += 0.09f;
                    Projectile.localAI[1] = 30f;
                }
            }
            else
            {
                if (!Collision.SolidCollision(Projectile.position - Vector2.One * 4, Projectile.width + 8, Projectile.height + 8))
                {
                    Projectile.localAI[0] = 0f;
                    Projectile.localAI[1] = 30f;
                }

                Projectile.damage = 0;
            }

            Projectile.rotation = (float)Math.Atan2(Projectile.ai[1], Projectile.ai[0]) - 1.57f;
            Lighting.AddLight(Projectile.Center, Colors[_flareType].ToVector3() * 1.3f);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0f)
            {
                if (Projectile.wet) Projectile.position += oldVelocity / 2f;
                else Projectile.position += oldVelocity;

                Projectile.velocity = Vector2.Zero;
                Projectile.localAI[0] = 1f;

                _trail?.Kill();
            }

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type];
            var width = texture.Width() / 2;
            var rotation = Projectile.rotation;
            var rect = new Rectangle(width * _flareType, 0, width, texture.Height());
            var origin = new Vector2(width, texture.Height()) * 0.5f;
            var position = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(texture.Value, position, rect, lightColor, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            var texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Projectiles/HyperlightFlareGunProjectile_Extra");
            var width = texture.Width() / 2;
            var rotation = Projectile.rotation;
            var rect = new Rectangle(width * _flareType, 0, width, texture.Height());
            var origin = new Vector2(width, texture.Height()) * 0.5f;
            var position = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(texture.Value, position, rect, Color.White * 0.85f, rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public void OnSpawn(int flareType)
        {
            _flareType = flareType;

            _trail = new RoundedTrail(
                target: Projectile,
                length: 16 * 7,
                width: (progress) => 8,
                color: (progress) => Colors[_flareType] * (1 - progress)
            );
            _trail.SetCustomPositionMethod((proj) =>
            {
                return proj.Center - Vector2.UnitY.RotatedBy(Projectile.rotation) * 11;
            });

            PrimitiveTrailSystem.NewTrail(_trail);
        }

        void IDrawAdditive.DrawAdditive()
        {
            var position = Projectile.Center - Main.screenPosition - Vector2.UnitY.RotatedBy(Projectile.rotation) * 11;
            var texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_1");
            var color = Colors[_flareType] * 0.5f;
            Main.EntitySpriteDraw(texture.Value, position, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.45f, SpriteEffects.None, 0);

            if (Projectile.localAI[0] != 1f) return;

            /*texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_1");
            color = Colors[_flareType] * 0.5f;
            Main.EntitySpriteDraw(texture.Value, position, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.45f, SpriteEffects.None, 0);*/
        }
    }
}