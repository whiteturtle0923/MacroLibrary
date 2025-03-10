using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MacroLibrary.Content.UI
{
    public class SaveButton: UIElement 
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = new(50, 255, 153);

            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay"), new Vector2(Main.screenWidth + 20, Main.screenHeight -20) / 2f, color);
        }
    }
}