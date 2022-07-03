using AtlasMod.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AtlasMod.Content.Tiles {
    public class StarRod : ModTile {
        public override string Texture => AtlasMod.AssetPath + "Textures/Tiles/StarRod";

        public override void SetStaticDefaults() {
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(158, 15, 102), Language.GetText("Star Rod"));
            AnimationFrameHeight = 198;
            HitSound = SoundID.Tink;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(0, 2);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
            TileObjectData.addTile(Type);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<StarRodItem>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0) {
                r = 0.98f;
                g = 0.81f;
                b = 0.47f;
            }
        }
    }
}
