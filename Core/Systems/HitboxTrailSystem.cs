using System.Collections.Generic;
using MacroLibrary.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MacroLibrary.Core.Systems
{
    public class HitboxTrailSystem: ModSystem
    {
        internal HitboxTrailUI hitboxTrailUIState;
        private UserInterface hitboxTrailUIInterface;

        public override void Load()
        {
            hitboxTrailUIState = new();
            hitboxTrailUIState.Activate();
            hitboxTrailUIInterface = new();
            hitboxTrailUIInterface.SetState(hitboxTrailUIState);
        }

        public override void Unload()
        {
            hitboxTrailUIState = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (hitboxTrailUIInterface?.CurrentState != null)
                hitboxTrailUIInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: MP Player Names"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Macro Library: Hitbox Trail",
                    delegate
                    {
                        if (hitboxTrailUIInterface?.CurrentState != null)
                            hitboxTrailUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}