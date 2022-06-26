using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AtlasMod {
    public class AtlasMod : Mod {
        public const string AssetPath = "AtlasMod/Assets/";

        public static AtlasMod Instance => ModContent.GetInstance<AtlasMod>();

        public override void AddRecipeGroups() {
            RecipeGroup group = new RecipeGroup(() => "Any Cloud Type", new int[] {
                ItemID.RainCloud,
                ItemID.Cloud,
            });
            RecipeGroup.RegisterGroup("AnyCloudType", group);
        }
    }
}