using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Items.Potions
{
    public abstract class TonicItem : ModItem
    {
        private readonly Color[] _particleColors;

        private readonly string _displayName;
        private readonly string _tooltip;

        private readonly int _buffType;
        private readonly int _buffTime;

        public TonicItem(string displayName, string tooltip, int buffType, int buffTime, Color[] particleColors = null)
        {
            _particleColors = particleColors;

            _displayName = displayName;
            _tooltip = tooltip;

            _buffType = buffType;
            _buffTime = buffTime;
        }

        public override string Texture => AtlasMod.AssetPath + "Textures/Items/Potions/" + this.Name;

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(_displayName);
            Tooltip.SetDefault(_tooltip);

            if (_particleColors != null && _particleColors.Length == 3)
            {
                ItemID.Sets.DrinkParticleColors[Type] = _particleColors;
            }

            this.TonicSetStaticDefaults();
        }

        public sealed override void SetDefaults()
        {
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Blue;

            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.consumable = true;

            Item.buffType = _buffType;
            Item.buffTime = _buffTime;

            Item.UseSound = SoundID.Item3;

            this.TonicSetDefaults();
        }

        public virtual void TonicSetStaticDefaults() { }
        public virtual void TonicSetDefaults() { }
    }
}
