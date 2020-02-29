using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace HalfbornMod
{
    public class HalfbornItem : GlobalItem
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        // Player player = Main.player[Main.myPlayer];
        Vector2 offset = new Vector2(10, -10);
        public override void UseStyle(Item item, Player player)
        {
            if (player.GetModPlayer<HalfbornPlayer>().demonForm)
            {
                if (player.HeldItem.useStyle != 1) HalfbornPlayer.modifyPlayerItemLocation(player, -5, 10);
                else
                {
                    if (player.itemTime == 0)
                    {
                       // player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
                    }
                    else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 3)
                    {
                        if (player.bodyFrame.Y >= 1 * player.bodyFrame.Height && player.bodyFrame.Y <= 3 * player.bodyFrame.Height) HalfbornPlayer.modifyPlayerItemLocation(player, -12, -14);

                    }
                }
            }
        }
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.GetModPlayer<HalfbornPlayer>().demonForm)
            {
                position += offset;
            }
            return true;
        }
    }
}