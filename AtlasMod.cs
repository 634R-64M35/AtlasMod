using Terraria.ModLoader;

namespace AtlasMod
{
    public class AtlasMod : Mod
    {
        public const string AssetPath = "AtlasMod/Assets/";

        public static AtlasMod Instance => ModContent.GetInstance<AtlasMod>();
    }
}