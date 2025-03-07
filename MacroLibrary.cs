using System;
using System.Runtime.ExceptionServices;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace MacroLibrary
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class MacroLibrary : Mod
	{
		public static ModKeybind ToggleMacroKeybind {get; private set; }
		public static ModKeybind ToggleRecordingKeybind {get; private set; }
		public static ModKeybind SaveMacroKeybind {get; private set; }
		public static ModKeybind LoadMacroKeybind {get; private set; }

        public override void Load()
        {
            ToggleMacroKeybind = KeybindLoader.RegisterKeybind(this, "Toggle Macro", Keys.None);
			ToggleRecordingKeybind = KeybindLoader.RegisterKeybind(this, "Toggle Recording", Keys.None);
			SaveMacroKeybind = KeybindLoader.RegisterKeybind(this, "Save Macro", Keys.None);
			LoadMacroKeybind = KeybindLoader.RegisterKeybind(this, "Load Macro", Keys.None);
        }

        public override void Unload()
        {
            ToggleMacroKeybind = null;
			ToggleRecordingKeybind = null;
			SaveMacroKeybind = null;
			LoadMacroKeybind = null;
        }
    }
}
