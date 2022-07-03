using AtlasMod.Common;
using AtlasMod.Common.Interfaces;
using AtlasMod.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Weapons.Ranged {
    public class HurricaneGRS : ModItem {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Weapons/HurricaneGRS";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hurricane GRS");
            Tooltip.SetDefault("'Management would be pleased'");
        }

        public override void SetDefaults() {
            Item.width = 68;
            Item.height = 38;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 50;
            Item.crit = 0;
            Item.knockBack = 4;

            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileID.RocketI;
            Item.shootSpeed = 15f;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(platinum: 0, gold: 12, silver: 20, copper: 50);

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.RocketLauncher)
                .AddIngredient(ItemID.ChlorophyteBar, 15)
                .AddIngredient(ItemID.Nanites, 50)
                .AddIngredient(ItemID.Wire, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UseItemFrame(Player player) {
            player.bodyFrame.Y = player.bodyFrame.Height * 3;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            velocity = GetShootVector(player.MountedCenter + player.gfxOffY * Vector2.UnitY) * velocity.Length();
            type = ModContent.ProjectileType<HurricaneGRSHeldProjectile>();

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanUseItem(Player player) {
            int count = player.ownedProjectileCounts[ModContent.ProjectileType<HurricaneGRSRocketProjectile>()];
            return count <= 6;
        }

        public static Vector2 GetShootVector(Vector2 playerCenter) {
            const float ANGLE = 0.25f;

            Vector2 vecToMouse = Vector2.Normalize(Main.MouseWorld - playerCenter);
            int direction = Math.Sign(vecToMouse.X);

            if (float.IsNaN(vecToMouse.X) || float.IsNaN(vecToMouse.Y)) {
                vecToMouse = -Vector2.UnitY;
            }

            Vector2 ret = new Vector2(1, 0);
            if (direction < 0) {
                vecToMouse = -vecToMouse;
                ret = new Vector2(-1, 0);
            }

            float rot = vecToMouse.ToRotation();
            return ret.RotatedBy(MathHelper.Clamp(rot, -ANGLE, ANGLE));
        }
    }

    public class HurricaneGRSHeldProjectile : ModProjectile {
        public int ShootCounter { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }

        public bool CanShoot => ShootTimer == 4;

        public ref float ShootTimer => ref Projectile.ai[1];

        public override string Texture => AtlasMod.AssetPath + "Textures/Projectiles/HurricaneGRSHeldProjectile";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hurricane GRS");
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults() {
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];

            ShootTimer++;
            Projectile.frame = (ShootTimer >= 2 && ShootTimer <= 10) ? (int)(ShootTimer - 2) : 0;

            if (CanShoot)
            {
                var spinningpoint = Vector2.UnitX * 24f;
                spinningpoint = spinningpoint.RotatedBy((double)(Projectile.rotation - 1.57079637f), default(Vector2));
                var value = Projectile.Center + spinningpoint;

                for (int i = 0; i < 2; i++)
                {
                    int num = Dust.NewDust(value - Vector2.One * 8f, 16, 16, 6, Projectile.velocity.X / 2f, Projectile.velocity.Y / 2f, 100, default(Color), 1f);
                    var dust = Main.dust[num];

                    dust.velocity *= 0.66f;
                    dust.noGravity = true;
                    dust.scale = 0.7f;
                }

                for (int i = 0; i < 5; i++)
                {
                    var particle = new SmokeParticle(
                    timeLeft: 60,
                    position: value,
                    velocity: new Vector2(Projectile.velocity.X / 2f, Projectile.velocity.Y / 2f) * 0.66f,
                    rotation: Main.rand.NextFloat(MathHelper.TwoPi),
                    scale: Main.rand.NextFloat(0.3f, 0.6f));
                    ParticleSystem.NewParticle(particle);
                }
            }

            var center = player.MountedCenter + player.gfxOffY * Vector2.UnitY;
            var shootSpeed = player.HeldItem.shootSpeed;

            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel && ItemLoader.CanUseItem(player.HeldItem, player) && !player.noItems && !player.CCed)
                {
                    var velocity = HurricaneGRS.GetShootVector(center) * shootSpeed;

                    if (velocity.X != Projectile.velocity.X || velocity.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }

                    Projectile.velocity = velocity;
                }
                else if (ShootTimer > 0 || ShootCounter != 0)
                {
                    // ...
                }
                else Projectile.Kill();
            }

            if (CanShoot)
            {
                SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);

                var rocketType = ModContent.ProjectileType<HurricaneGRSRocketProjectile>();
                var spawnPos = center + new Vector2(10 * player.direction, 14 * player.gravDir).RotatedBy(player.fullRotation);
                var index = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos.X, spawnPos.Y, Projectile.velocity.X, Projectile.velocity.Y, rocketType, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);

                (Main.projectile[index].ModProjectile as HurricaneGRSRocketProjectile).Speed = shootSpeed;
                ShootCounter++;

                float speed = shootSpeed, knockBack = Projectile.knockBack;
                //bool canShoot = true;
                int damage = Projectile.damage;

                if (ShootCounter > 1)
                {
                    player.PickAmmo(player.HeldItem, out rocketType, out speed, out damage, out knockBack, out int usedAmmoItemId);
                }
            }

            if (ShootTimer > player.HeldItem.useTime) ShootTimer = -1;
            if (ShootCounter >= 3) ShootCounter = 0;

            var shakeProgress = (float)Math.Sin(Projectile.frame / (float)Main.projFrames[Type] * MathHelper.Pi);
            var shakeOffset = new Vector2(0, 0.5f).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * shakeProgress;

            Projectile.velocity.Normalize();
            Projectile.Center = center + shakeOffset + new Vector2(10 * player.direction - shakeProgress, 10 * player.gravDir).RotatedBy(player.fullRotation);
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;
            Projectile.spriteDirection = Projectile.direction;

            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type];
            var player = Main.player[Projectile.owner];
            var rotation = Projectile.rotation + MathHelper.PiOver2;
            var position = Projectile.Center - Main.screenPosition;
            var effect = SpriteEffects.FlipHorizontally;

            if (player.gravDir == 1 && player.direction == 1 || player.gravDir != 1 && player.direction != 1)
            {
                effect |= SpriteEffects.FlipVertically;
            }

            Main.EntitySpriteDraw(texture.Value, position, null, lightColor, rotation, texture.Size() * 0.5f, Projectile.scale, effect, 0);

            texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Projectiles/HurricaneGRSHeldProjectile_Extra");
            position += new Vector2(4 * player.gravDir * player.direction, -47).RotatedBy(Projectile.rotation);
            var height = texture.Height() / Main.projFrames[Projectile.type];

            Main.EntitySpriteDraw(texture.Value, position, new Rectangle(0, height * Projectile.frame, texture.Width(), height), Color.White, rotation, new Vector2(texture.Width(), height) * 0.5f, Projectile.scale, effect, 0);

            return false;
        }

        public override bool? CanCutTiles() => false;
        public override bool? CanDamage() => false;
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }

    public class HurricaneGRSRocketProjectile : ModProjectile, IDrawAdditive
    {
        private const int EXPLOSION_RADIUS_MAX = 16 * 8;
        private const int EXPLOSION_RADIUS_MIN = 16 * 2;
        private const int LIFETIME = 60 * 4;

        public float Speed { get; set; }
        public float Progress { get => _timer / (float)LIFETIME; }
        public bool CanBeDrawn { get => Projectile.timeLeft > 3; }
        public Vector2 TargetPos
        {
            get => new(Projectile.ai[0], Projectile.ai[1]);
            set
            {
                Projectile.ai[0] = value.X;
                Projectile.ai[1] = value.Y;
            }
        }

        private int _timer = 0;

        public override string Texture => AtlasMod.AssetPath + "Textures/Projectiles/HurricaneGRSRocketProjectile";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hurricane GRS");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = LIFETIME;
        }

        public override bool PreAI()
        {
            _timer++;

            var player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.HeldItem?.type != ModContent.ItemType<HurricaneGRS>())
            {
                Projectile.Kill();
                return false;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                TargetPos = Main.MouseWorld;
            }

            MoveTowards(TargetPos, Speed, 20f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.friendly = true;

            if (Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[1] = 0f;
                Projectile.alpha = 255;

                int value = (int)MathHelper.Lerp(EXPLOSION_RADIUS_MIN, EXPLOSION_RADIUS_MAX, Progress);
                Projectile.Resize(value, value);

                value = player.GetWeaponDamage(player.HeldItem);
                Projectile.damage = (int)MathHelper.Lerp(value, value * 2.5f, Progress);

                Projectile.knockBack = 10f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Lighting.AddLight(Projectile.Center, new Color(255, 211, 85).ToVector3() * 0.35f);
            SpawnTrail();
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0f;
            Projectile.alpha = 255;
            Projectile.timeLeft = 3;

            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int num = 0; num < 15; num++)
            {
                int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[index].velocity *= 1.4f;
            }

            for (int num = 0; num < 10; num++)
            {
                int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3.5f);
                var dust = Main.dust[index];
                dust.noGravity = true;
                dust.velocity *= 7f;

                index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[index].velocity *= 3f;
            }

            for (int i = 0; i < 7; i++)
            {
                var particle = new SmokeParticle(
                    timeLeft: 60,
                    position: Projectile.Center + new Vector2(0, Main.rand.NextFloat(50)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)),
                    velocity: Vector2.Zero,
                    rotation: Main.rand.NextFloat(MathHelper.TwoPi),
                    scale: Main.rand.NextFloat(1.5f, 2.5f));
                ParticleSystem.NewParticle(particle);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (CanBeDrawn)
            {
                var drawPos = Projectile.Center + Projectile.gfxOffY * Vector2.UnitY - Main.screenPosition;
                drawPos -= Vector2.Normalize(Projectile.velocity) * 8;

                var texture = TextureAssets.Projectile[Type];
                var height = texture.Height() / Main.projFrames[Projectile.type];
                var rect = new Rectangle(0, height * Projectile.frame, texture.Width(), height);
                var origin = new Vector2(texture.Width(), height) * 0.5f;
                Main.EntitySpriteDraw(texture.Value, drawPos, rect, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

                texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Projectiles/HurricaneGRSRocketProjectile_Extra");
                Main.EntitySpriteDraw(texture.Value, drawPos, rect, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }

        private void MoveTowards(Vector2 target, float speed, float resistance)
        {
            var move = target - Projectile.Center;
            var length = move.Length();

            if (length > speed) move *= speed / length;

            move = (Projectile.velocity * resistance + move) / (resistance + 1f);
            move = Vector2.Normalize(move) * MathHelper.Max(move.Length(), 4f);
            length = move.Length();

            if (length > speed) move *= speed / length;

            Projectile.velocity = move;
        }

        public void SpawnTrail()
        {
            var particle = new SmokeParticle(
                timeLeft: 60,
                position: Projectile.Center + new Vector2(Main.rand.NextFloat(2)).RotatedByRandom(MathHelper.TwoPi),
                velocity: Vector2.Zero,
                rotation: Main.rand.NextFloat(MathHelper.TwoPi),
                scale: Main.rand.NextFloat(0.6f, 1f));
            ParticleSystem.NewParticle(particle);

            if (Main.rand.NextBool(3))
            {
                particle = new SmokeParticle(
                timeLeft: 20,
                position: Projectile.Center + new Vector2(Main.rand.NextFloat(2)).RotatedByRandom(MathHelper.TwoPi),
                velocity: Vector2.Zero,
                color: Color.Orange,
                rotation: Main.rand.NextFloat(MathHelper.TwoPi),
                scale: Main.rand.NextFloat(0.4f, 0.8f));
                ParticleSystem.NewParticle(particle);
            }
        }

        void IDrawAdditive.DrawAdditive()
        {
            if (!CanBeDrawn) return;

            var drawPos = Projectile.Center + Projectile.gfxOffY * Vector2.UnitY - Main.screenPosition;
            drawPos -= Vector2.Normalize(Projectile.velocity) * 12;
            var texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_1");
            var color = new Color(255, 211, 85) * 0.35f;

            Main.EntitySpriteDraw(texture.Value, drawPos, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.45f, SpriteEffects.None, 0);
        }
    }
}