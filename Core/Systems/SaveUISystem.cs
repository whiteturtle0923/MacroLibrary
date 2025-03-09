using System.Collections.Generic;
using MacroLibrary.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MacroLibrary.Core.Systems
{
    public class SaveUISystem: ModSystem
    {
        internal SaveUIState saveUIState;
        private UserInterface saveUIInterface;

        public override void Load()
        {
            saveUIState = new();
            saveUIState.Activate();
            saveUIInterface = new();
        }

        public override void Unload()
        {
            saveUIState = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (saveUIInterface?.CurrentState != null)
                saveUIInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Macro Library: Save UI",
                    delegate
                    {
                        if (saveUIInterface?.CurrentState != null)
                            saveUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void ToggleUI() 
        {
            if (saveUIInterface?.CurrentState == null)
			    saveUIInterface?.SetState(saveUIState);
            else 
                saveUIInterface?.SetState(null);
		}
    }
}