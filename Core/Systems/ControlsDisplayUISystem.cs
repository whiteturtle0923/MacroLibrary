using System.Collections.Generic;
using MacroLibrary.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MacroLibrary.Core.Systems
{
    public class ControlsDisplayUISystem: ModSystem
    {
        internal ControlsDisplayUIState controlsDisplayUIState;
        private UserInterface controlsDisplayUIInterface;

        public override void Load()
        {
            controlsDisplayUIState = new();
            controlsDisplayUIState.Activate();
            controlsDisplayUIInterface = new();
            controlsDisplayUIInterface.SetState(controlsDisplayUIState);
        }

        public override void Unload()
        {
            controlsDisplayUIState = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (controlsDisplayUIInterface?.CurrentState != null)
                controlsDisplayUIInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Macro Library: Controls Display",
                    delegate
                    {
                        if (controlsDisplayUIInterface?.CurrentState != null)
                            controlsDisplayUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}