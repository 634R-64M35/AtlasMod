using AtlasMod.Common;
using AtlasMod.Common.Interfaces;
using AtlasMod.Content.Items.Materials;
using AtlasMod.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Weapons.Ranged {
    public class MedalOfHonor : ModItem {
        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Weapons/MedalOfHonor";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Medal Of Honor");
            Tooltip.SetDefault("Shoots a homing soaring eagle, releases a bullet cartridge upon discharge\n" +
                "'Fly free, soaring eagle'");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            foreach (TooltipLine tooltipLine in tooltips) {
                List<Color> flagColors = new List<Color>() {
                    new Color(98, 164, 239), //Old Glory Blue
                    new Color(237, 87, 154), //Old Glory Red
                    new Color(253, 247, 236), //White
                    new Color(155, 227, 245), //Neon Glory Blue
                    new Color(245, 182, 203), //Neon Glory Red
                    new Color(253, 247, 236), //White
                };
                int colorIndex = (int)(Main.GlobalTimeWrappedHourly / 2 % flagColors.Count);
                Color currentColor = flagColors[colorIndex];
                Color nextColor = flagColors[(colorIndex + 1) % flagColors.Count];
                if (tooltipLine.Mod == "Terraria" && tooltipLine.Name == "ItemName") {
                    tooltipLine.OverrideColor = Color.Lerp(currentColor, nextColor, Main.GlobalTimeWrappedHourly % 2f > 1f ? 1f : Main.GlobalTimeWrappedHourly % 1f);
                }
            }
        }

        public override void SetDefaults() {
            Item.width = 26;
            Item.height = 26;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 64;
            Item.crit = 4;
            Item.knockBack = 0;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(platinum: 6, gold: 27, silver: 20, copper: 22);

            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.noMelee = true;
            Item.autoReuse = false;
            //Item.channel = true;

            Item.scale = 0.8f;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-35, 6);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            var cartridge = ModContent.ProjectileType<MedalOfHonorCartridge>();
            Projectile.NewProjectile(source, position - new Vector2(-8, 2), velocity, cartridge, damage, knockback, player.whoAmI);
            return true;
        }

        public override bool CanUseItem(Player player) {
            int count = player.ownedProjectileCounts[ModContent.ProjectileType<MedalOfHonorCartridge>()];
            return count <= 2;
        }

        /*public override bool? CanAutoReuseItem(Player player) {
            return false;
        }*/

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Musket)
                .AddIngredient(ItemID.Revolver)
                .AddIngredient(ItemID.Handgun)
                .AddIngredient(ItemID.ClockworkAssaultRifle)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class SoaringEagleProjectile : ModProjectile {
        public override string Texture => AtlasMod.AssetPath + "Textures/Projectiles/SoaringEagleProjectile";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Medal Of Honor");
        }

        public override void SetDefaults() {
            Projectile.width = 168;
            Projectile.height = 74;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.maxPenetrate = 3;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
        }
    }

    public class MedalOfHonorCartridge : ModProjectile {
        public override string Texture => AtlasMod.AssetPath + "Textures/Projectiles/MedalOfHonorCartridge";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Medal Of Honor");
        }

        public override void SetDefaults() {
            Projectile.width = 8;
            Projectile.height = 18;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.maxPenetrate = 3;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.Bullet;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.WeaponImbueGold, 18000, false);
        }
    }
}
