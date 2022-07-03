using AtlasMod.Common;
using AtlasMod.Common.Interfaces;
using AtlasMod.Content.Items.Materials;
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

namespace AtlasMod.Content.Items.Weapons.Thrown {
    public class AstralMine : ModItem {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Weapons/AstralMineArmed";
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Astral Mine");
            Tooltip.SetDefault("A high damage sticky proximity mine\nReleases a 4 star burst based on direction\nTakes 5 seconds to arm");
        }

        public override void SetDefaults() {
            Item.width = 26;
            Item.height = 26;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 150;
            Item.crit = 0;
            Item.knockBack = 0;

            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(platinum: 0, gold: 0, silver: 0, copper: 75);

            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 10;
            Item.UseSound = SoundID.Item15;
            Item.noMelee = true;
            Item.consumable = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            type = ModContent.ProjectileType<AstralMineProjectile>();

            var proj = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
            //(proj.ModProjectile as HyperlightFlareGunProjectile).OnSpawn(flareType);

            /*for (int i = 0; i < 5; i++)
            {
                var particle = new HyperlightFlareGunParticle(
                    color: HyperlightFlareGunProjectile.Colors[flareType],
                    timeLeft: 60,
                    position: position,
                    velocity: velocity.RotatedBy(Main.rand.NextFloat(-0.67f, 0.65f)) * Main.rand.NextFloat(0.01f, 0.1f),
                    scale: Main.rand.NextFloat(0.7f, 0.9f)
                );
                ParticleSystem.NewParticle(particle);
            }*/
            return false;
        }

        public override void AddRecipes() {
            ModContent.GetInstance<AstralMine>().CreateRecipe(15)
                .AddIngredient(ItemID.Grenade, 15)
                .AddIngredient<MeticuliteCompound>(3)
                .AddIngredient(ItemID.MeteoriteBar)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class AstralMineProjectile : ModProjectile {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Weapons/AstralMineUnarmed";
    }
}
