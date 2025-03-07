using MacroLibrary.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MacroLibrary.Content.Items
{
    public class SaveMacro: ModItem
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.HandOfCreation}";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            MacroPlayer mp = player.GetModPlayer<MacroPlayer>();
            if (player.altFunctionUse == 2)
            {
                mp.LoadMacro();
                Main.NewText("Macro Loaded"); 
            }
            else
            {
                mp.SaveMacro();
                Main.NewText("Macro Saved");
            }
            return true;
        }
    }
}