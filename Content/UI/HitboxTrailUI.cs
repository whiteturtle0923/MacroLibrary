using MacroLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace MacroLibrary.Content.UI
{
    internal class HitboxTrailUI: UIState
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            Texture2D tex = TextureAssets.MagicPixel.Value;
            Player player = Main.LocalPlayer;
            Rectangle hitbox = player.Hitbox;
            foreach (Vector2 pos in player.GetModPlayer<MacroPlayer>().MacroPositions)
            {
                //spriteBatch.Draw(tex, new Rectangle((int)(pos - Main.screenPosition).X, (int)(pos - Main.screenPosition).Y, hitbox.Width, hitbox.Height), Color.Red * 0.5f);
            }
            foreach (Vector2 pos in player.GetModPlayer<MacroPlayer>().RecordingPositions)
            {
                //spriteBatch.Draw(tex, new Rectangle((int)(pos - Main.screenPosition).X, (int)(pos - Main.screenPosition).Y, hitbox.Width, hitbox.Height), Color.Blue * 0.5f);
            }
        }
    }
}