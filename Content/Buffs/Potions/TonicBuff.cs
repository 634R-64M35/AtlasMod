using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod.Content.Buffs.Potions
{
    public abstract class TonicBuff : ModBuff
    {
        private readonly string _displayName;
        private readonly string _tooltip;

        public TonicBuff(string displayName, string tooltip)
        {
            _displayName = displayName;
            _tooltip = tooltip;
        }

        public override string Texture => AtlasMod.AssetPath + "Textures/Buffs/" + this.Name;

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(_displayName);
            Description.SetDefault(_tooltip);

            this.TonicSetStaticDefaults();
        }

        public sealed override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.LightPurple;

            this.TonicModifyBuffTip(ref tip, ref rare);
        }

        public virtual void TonicSetStaticDefaults() { }
        public virtual void TonicModifyBuffTip(ref string tip, ref int rare) { }
    }
}
