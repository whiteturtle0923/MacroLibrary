using System;
using System.Runtime.ExceptionServices;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace MacroLibrary
{
	public class MacroLibrary : Mod
	{
		public static ModKeybind ToggleMacroKeybind {get; private set; }
		public static ModKeybind ToggleRecordingKeybind {get; private set; }
		public static ModKeybind SaveMacroKeybind {get; private set; }
		public static ModKeybind LoadMacroKeybind {get; private set; }

        public override void Load()
        {
            ToggleMacroKeybind = KeybindLoader.RegisterKeybind(this, "ToggleMacro", Keys.None);
			ToggleRecordingKeybind = KeybindLoader.RegisterKeybind(this, "ToggleRecording", Keys.None);
			SaveMacroKeybind = KeybindLoader.RegisterKeybind(this, "SaveMacro", Keys.None);
			LoadMacroKeybind = KeybindLoader.RegisterKeybind(this, "LoadMacro", Keys.None);
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
