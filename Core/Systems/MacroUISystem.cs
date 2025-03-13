using System.Collections.Generic;
using MacroLibrary.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MacroLibrary.Core.Systems
{
    public class MacroUISystem: ModSystem
    {
        internal MacroUIState macroUIState;
        private UserInterface macroUIInterface;

        public override void Load()
        {
            macroUIState = new();
            macroUIState.Activate();
            macroUIInterface = new();
        }

        public override void Unload()
        {
            macroUIState = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (macroUIInterface?.CurrentState != null)
                macroUIInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Macro Library: Macro UI",
                    delegate
                    {
                        if (macroUIInterface?.CurrentState != null)
                            macroUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void ToggleUI() 
        {
            if (macroUIInterface?.CurrentState == null)
			    macroUIInterface?.SetState(macroUIState);
            else 
                macroUIInterface?.SetState(null);
		}
    }
}