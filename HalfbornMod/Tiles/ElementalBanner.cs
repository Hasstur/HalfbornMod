using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace HalfbornMod.Tiles
{

    public class ElementalBanner : ModTile
    {

        public override void SetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile((int)base.Type);
            this.dustType = -1;
            this.disableSmartCursor = true;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("");
            base.AddMapEntry(new Color(13, 88, 130), modTranslation);
        }


        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            string text;
            switch (frameX / 18)
            {
                case 0:
                    text = "WhiteElemental";
                    break;
                case 1:
                    text = "RedElemental";
                    break;
                case 2:
                    text = "GreenElemental";
                    break;
                case 3:
                    text = "BlueElemental";
                    break;
                case 4:
                    text = "YellowElemental";
                    break;
                case 5:
                    text = "OrangeElemental";
                    break;
                case 6:
                    text = "BlackElemental";
                    break;

                default:
                    return;
            }
            Item.NewItem(i * 16, j * 16, 16, 48, base.mod.ItemType(text), 1, false, 0, false, false);
        }


        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player localPlayer = Main.LocalPlayer;
                string text;
                switch (Main.tile[i, j].frameX / 18)
                {
                    case 0:
                        text = "WhiteElemental";
                        break;
                    case 1:
                        text = "RedElemental";
                        break;
                    case 2:
                        text = "GreenElemental";
                        break;
                    case 3:
                        text = "BlueElemental";
                        break;
                    case 4:
                        text = "YellowElemental";
                        break;
                    case 5:
                        text = "OrangeElemental";
                        break;
                    case 6:
                        text = "BlackElemental";
                        break;
                    default:
                        return;
                }
                localPlayer.NPCBannerBuff[base.mod.NPCType(text)] = true;
                localPlayer.hasBanner = true;
            }
        }


        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}
