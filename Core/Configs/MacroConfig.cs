using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MacroLibrary.Core.Configs
{
    public class MacroConfig: ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        public bool UncappedMacroFileNameLength;
        [DefaultValue(true)]
        public bool DisplayCurrentControls;
        [DefaultValue(true)]
        public bool CloseUIOnStartMacro;
        [DefaultValue(true)]
        public bool CloseUIOnRecordMacro;
        [DefaultValue(false)]
        public bool DrawHitboxTrail;
    }
}