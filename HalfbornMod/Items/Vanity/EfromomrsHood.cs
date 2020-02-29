using Terraria.ModLoader;

namespace HalfbornMod.Items.Vanity
{
    [AutoloadEquip(new EquipType[] { EquipType.Head })]
    public class EfromomrsHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Efromomr's Hood");
            Tooltip.SetDefault("'Great for impersonating Halfborn Devs!'");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 0;
            item.rare = 9;
            item.vanity = true;
        }
    }
}
