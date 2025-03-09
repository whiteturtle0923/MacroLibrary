using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MacroLibrary.Core.Configs
{
    public class MacroConfig: ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        public bool UncappedMacroFileNameLength;
        
    }
}