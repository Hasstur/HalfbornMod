﻿using System;
using Terraria;
using Terraria.ModLoader;

namespace HalfbornMod.Items.Banners
{

    public class BlackElementalBanner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Elemental Banner");
            Tooltip.SetDefault("Nearby players get a bonus against: Black Elemental");
        }


        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.createTile = mod.TileType("ElementalBanner");
            item.placeStyle = 6;
        }
    }
}
