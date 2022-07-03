using AtlasMod.Common;
using AtlasMod.Common.Interfaces;
using AtlasMod.Content.Particles;
using AtlasMod.Content.Trails;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Weapons.Ranged {
    public class MeticuliteBow : ModItem {
        public static readonly Color[] StarColors = new Color[] { new Color(248, 131, 80), new Color(80, 182, 255), new Color(255, 75, 130), Color.White };

        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Weapons/MeticuliteBow";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Meticulite Bow");
            Tooltip.SetDefault("Tags an enemy with a mark.\nWhen an enemy with this mark is hit by another weapon,\nthe mark will explode, dealing 5x the damage dealt.");
        }

        public override void SetDefaults() {
            Item.width = 36;
            Item.height = 62;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 24;
            Item.crit = 0;
            Item.knockBack = 3;

            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 15f;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(platinum: 0, gold: 0, silver: 80, copper: 40);

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.useTime = Item.useAnimation = 16;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.scale = 0.8f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ModContent.ProjectileType<MeticuliteBowProjectile>();
        public override void UseStyle(Player player, Rectangle heldItemFrame) => Lighting.AddLight(player.itemLocation, new Color(233, 75, 61).ToVector3() * 0.3f);
        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);
        public override Color? GetAlpha(Color lightColor) => new Color(240, 240, 240, 240);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var proj = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
            (proj.ModProjectile as MeticuliteBowProjectile).OnSpawn();

            for (int i = 0; i < 5; i++) {
                var particle = new MeticuliteStarParticle(
                    color: StarColors[Main.rand.Next(StarColors.Length)],
                    timeLeft: 60,
                    position: position + Vector2.Normalize(velocity) * 45,
                    velocity: velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.15f, 0.3f),
                    rotation: Main.rand.NextFloat(MathHelper.TwoPi),
                    scale: Main.rand.NextFloat(0.7f, 0.9f)
                );
                ParticleSystem.NewParticle(particle);
            }

            return false;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.DemonBow)
                .AddIngredient<Materials.MeticuliteCompound>(8)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.Cloud, 8)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.TendonBow)
                .AddIngredient<Materials.MeticuliteCompound>(8)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.Cloud, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class MeticuliteBowProjectile : ModProjectile, IDrawAdditive {
        public override string Texture => AtlasMod.AssetPath + "Textures/Projectiles/MeticuliteBowProjectile";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Meticulite Bow");
        }

        public override void SetDefaults() {
            Projectile.arrow = true;

            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1200;
        }

        public override void AI() {
            Lighting.AddLight(Projectile.Center, new Color(233, 75, 61).ToVector3() * 0.3f);
        }

        public override void Kill(int timeLeft) {
            for (int i = 0; i < 12; i++) {
                var vector = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                var particle = new MeticuliteStarParticle(
                    color: MeticuliteBow.StarColors[Main.rand.Next(MeticuliteBow.StarColors.Length)],
                    timeLeft: 60,
                    position: Projectile.Center + vector * Main.rand.NextFloat(4.0f),
                    velocity: vector * Main.rand.NextFloat(0.3f, 3f),
                    rotation: Main.rand.NextFloat(MathHelper.TwoPi),
                    scale: Main.rand.NextFloat(0.7f, 0.9f)
                );
                ParticleSystem.NewParticle(particle);
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            var texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Projectiles/MeticuliteBowProjectile");
            var rotation = Projectile.rotation;
            var position = Projectile.Center - Main.screenPosition - Vector2.UnitY.RotatedBy(Projectile.rotation) * 4;

            var color = new Color(221, 21, 82) * 0.35f;
            var progress = Main.GlobalTimeWrappedHourly + Projectile.whoAmI * 0.55f;

            for (int i = 0; i < 5; i++) {
                Main.EntitySpriteDraw(texture.Value, position + new Vector2(0, (float)Math.Sin(progress) * 6).RotatedBy(progress + MathHelper.TwoPi / 5f * i), null, 
                    color, rotation, new Vector2(texture.Width() * 0.5f, 0), Projectile.scale * 1.1f, SpriteEffects.None, 0); ;
            }
            Main.EntitySpriteDraw(texture.Value, position, null, Color.White * 0.95f, rotation, new Vector2(texture.Width() * 0.5f, 0), Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public void OnSpawn() {
            var trail = new SimpleTrail(
                target: Projectile,
                length: 16 * 5,
                width: (progress) => 10 * (1 - progress * 0.3f),
                color: (progress) => new Color(47, 163, 247) * MathHelper.Lerp(0.3f, 0f, progress),
                additive: true
            );
            trail.SetDissolveSpeed(1f);
            trail.SetCustomPositionMethod((proj) => {
                return proj.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * 30;
            });
            PrimitiveTrailSystem.NewTrail(trail);
        }

        void IDrawAdditive.DrawAdditive() {
            var position = Projectile.Center - Main.screenPosition + Vector2.UnitY.RotatedBy(Projectile.rotation) * 3;
            var texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_1");
            var color = new Color(233, 75, 61) * 0.35f;
            Main.EntitySpriteDraw(texture.Value, position, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.6f, SpriteEffects.None, 0);

            var progress = (float)Main.GlobalTimeWrappedHourly + Projectile.whoAmI * 0.55f;
            color = new Color(47, 163, 247) * 0.2f;
            texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_3");
            Main.EntitySpriteDraw(texture.Value, position, null, color, progress, texture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture.Value, position, null, color * 0.5f, -progress, texture.Size() * 0.5f, Projectile.scale * 0.3f, SpriteEffects.None, 0);
        }
    }

    public class MeticuliteBowMarkGlobalNPC : GlobalNPC {
        public static readonly Color EffectColor = new(47, 163, 247);

        public bool IsMarked => Timer > 0;
        public int Timer;

        public override bool InstancePerEntity => true;

        public override void PostAI(NPC npc) {
            if (IsMarked) Timer--;
        }


        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
            => this.ModifyHitByPlayer(npc, ref damage);
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
            => this.ModifyHitByPlayer(npc, ref damage, projectile.type == ModContent.ProjectileType<MeticuliteBowProjectile>());

        private void ModifyHitByPlayer(NPC npc, ref int damage, bool meticuliteArrow = false) {
            if (meticuliteArrow) {
                Timer = 60 * 7;
            } else if (IsMarked) {
                damage = (int)(damage * 1.25f);
            }
        }

        public override bool PreAI(NPC npc) {
            if (IsMarked) {
                Lighting.AddLight(npc.Center, EffectColor.ToVector3() * 0.3f);
            }
            return true;
        }

        public void DrawMeticuliteMark(NPC npc) {
            if (!npc.active || !IsMarked) return;

            var rotation = (float)Main.GlobalTimeWrappedHourly + npc.whoAmI * 0.4f;
            var texture = ModContent.Request<Texture2D>(AtlasMod.AssetPath + "Textures/Misc/Extra_3");
            var position = npc.Center + Vector2.UnitY * npc.gfxOffY - Main.screenPosition;
            var scale = npc.scale * 2f * (new Vector2(npc.Hitbox.Width, npc.Hitbox.Height).Length() / texture.Size().Length());
            var colorProgress = Timer > 20 ? 1f : (Timer / 20f);

            Main.EntitySpriteDraw(texture.Value, position, null, EffectColor * 0.4f * colorProgress, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture.Value, position, null, EffectColor * 0.6f * colorProgress, -rotation, texture.Size() * 0.5f, scale * 0.8f, SpriteEffects.None, 0);
        }
    }
}