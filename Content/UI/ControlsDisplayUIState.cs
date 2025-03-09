using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MacroLibrary.Content.UI
{
    internal class ControlsDisplayUIState: UIState
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            base.DrawSelf(spriteBatch);
			Color color = Color.Black;
			DynamicSpriteFont font = FontAssets.MouseText.Value; 
			Vector2 selfPos = new(10, Main.screenHeight / 2 - 100);
			if (Main.LocalPlayer.controlUp)
				spriteBatch.DrawString(font, "Up", new Vector2(0, 20) + selfPos, color);
			if (Main.LocalPlayer.controlDown)
				spriteBatch.DrawString(font, "Down", new Vector2(0, 40) + selfPos, color);
			if (Main.LocalPlayer.controlLeft)
				spriteBatch.DrawString(font, "Left", new Vector2(0, 60) + selfPos, color);
			if (Main.LocalPlayer.controlRight)
				spriteBatch.DrawString(font, "Right", new Vector2(0, 80) + selfPos, color);
			if (Main.LocalPlayer.controlJump)
				spriteBatch.DrawString(font, "Jump", new Vector2(0, 100) + selfPos, color);
        }
    }
}