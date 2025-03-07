using System;
using System.Linq;
using System.Reflection;
using MacroLibrary.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MacroLibrary.Content.Items
{
    public class StartMacro: ModItem
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.PDA}";
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
                if (!mp.Recording)
                {
                    if (mp.MacroOn)
                    {
                        Main.NewText("Cannot Start Recording While Playing Macro");
                        return true;
                    }
                    mp.Instructions.Clear();
                    mp.InstructionsEnabled = [.. Enumerable.Repeat(true,  mp.Instructions.Capacity)];
                    
                    Main.NewText("Macro Recording Started"); 
                }
                else
                {
                    for (int i = 0; i < mp.Instructions.Count; i++)
                    {
                        Tuple<Controls, int, int> instruction = mp.Instructions[i];
                        if (instruction.Item3 == -1)
                        {
                            mp.Instructions[i] = new(instruction.Item1, instruction.Item2, mp.RecordingTime);
                        }
                    }
                    mp.RecordingTime = 0;
                    Array.Clear(mp.previousControls);
                    mp.Instructions.Capacity = mp.Instructions.Count;
                    Main.NewText("Macro Recording Stopped");
                }
                mp.Recording = !mp.Recording;

            }
            else
            {
                if (mp.Recording)
                {
                    Main.NewText("Cannot Start Macro While Recording");
                    return true;
                }
                mp.MacroOn = !mp.MacroOn;
                mp.InstructionsEnabled = [.. Enumerable.Repeat(true, mp.Instructions.Capacity)];
                Main.NewText($"Macro {(mp.MacroOn ? "Started" : "Stopped")}");
            }
            return true;
        }
    }
}