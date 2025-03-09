using System;
using System.Reflection;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MacroLibrary
{
	public class MacroLibrary : Mod
	{
		public static ModKeybind ToggleMacroKeybind {get; private set; }
		public static ModKeybind ToggleRecordingKeybind {get; private set; }
		public static ModKeybind SaveMacroMenuKeybind {get; private set; }

		Hook hook;
        public override void Load()
        {
            ToggleMacroKeybind = KeybindLoader.RegisterKeybind(this, "ToggleMacro", Keys.None);
			ToggleRecordingKeybind = KeybindLoader.RegisterKeybind(this, "ToggleRecording", Keys.None);
			SaveMacroMenuKeybind = KeybindLoader.RegisterKeybind(this, "SaveMacroMenu", Keys.None);
			//hook = new(typeof(RecipeLoader).GetMethod("ConsumeIngredient", BindingFlags.Public | BindingFlags.Static), FullyDecraftPotions);
        }

        public override void Unload()
        {
            ToggleMacroKeybind = null;
			ToggleRecordingKeybind = null;
			SaveMacroMenuKeybind = null;
			//hook.Dispose();
        }
		/*
		private delegate void ConsumeIngredientDelegate(Recipe recipe, int type, ref int amount, bool isDecrafting);
		private static void FullyDecraftPotions(ConsumeIngredientDelegate orig, Recipe recipe, int type, ref int amount, bool isDecrafting)
		{
			if (recipe.requiredTile.Contains(TileID.Bottles))
				isDecrafting = false;
			orig(recipe, type, ref amount, isDecrafting);
		}*/
    }
}
