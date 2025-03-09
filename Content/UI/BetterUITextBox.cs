using System;
using MacroLibrary.Core.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace MacroLibrary.Content.UI
{
    // borrowed from https://github.com/direwolf420/PetRenamer/blob/1.4/UI/RenamePetUI/UIBetterTextBox.cs
	public class BetterUITextBox : UIPanel
	{
		internal string currentString = string.Empty;

		internal bool focused = false;

		private readonly int maxLength = 13;
		private readonly string hintText;
		private int textBlinkerCount;
		private int textBlinkerState;

		public event Action OnFocus;

		public event Action OnUnfocus;

		public event Action OnTextChanged;

		public event Action OnTabPressed;

		public event Action OnEnterPressed;

		internal bool unfocusOnEnter = true;

		internal bool unfocusOnTab = true;

		internal BetterUITextBox(string hintText, string text = "")
		{
			this.hintText = hintText;
			currentString = text;
			SetPadding(0);
			BackgroundColor = Color.White;
			BorderColor = Color.Black;
		}

		public override void LeftClick(UIMouseEvent evt)
		{
			Focus();
			base.LeftClick(evt);
		}

		internal void Unfocus()
		{
			if (focused)
			{
				focused = false;
				Main.blockInput = false;

				OnUnfocus?.Invoke();
			}
		}

		internal void Focus()
		{
			if (!focused)
			{
				Main.clrInput();
				focused = true;
				Main.blockInput = true;

				OnFocus?.Invoke();
			}
		}

		public override void Update(GameTime gameTime)
		{
			Vector2 MousePosition = new Vector2(Main.mouseX, Main.mouseY);
			if (!ContainsPoint(MousePosition) && (Main.mouseLeft || Main.mouseRight)) //This solution is fine, but we need a way to cleanly "unload" a UIElement
			{
				//TODO, figure out how to refocus without triggering unfocus while clicking enable button.
				Unfocus();
			}
			base.Update(gameTime);
		}


        /*
		internal void SetText(string text)
		{
			if (text.Length > maxLength)
			{
				text = text.Substring(0, maxLength);
			}
			if (currentString != text)
			{
				currentString = text;
				OnTextChanged?.Invoke();
			}
		}
        */

		private static bool JustPressed(Keys key)
		{
			return Main.inputText.IsKeyDown(key) && !Main.oldInputText.IsKeyDown(key);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Rectangle hitbox = GetInnerDimensions().ToRectangle();

			//Draw panel
			base.DrawSelf(spriteBatch);

			if (focused)
			{
				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();
				string newString = Main.GetInputText(currentString);
                if (newString.Length > maxLength && !ModContent.GetInstance<MacroConfig>().UncappedMacroFileNameLength)
                    newString = newString.Substring(0, maxLength);
				if (!newString.Equals(currentString))
				{
					currentString = newString;
					OnTextChanged?.Invoke();
				}
				else
				{
					currentString = newString;
				}

				if (JustPressed(Keys.Tab))
				{
					if (unfocusOnTab) Unfocus();
					OnTabPressed?.Invoke();
				}
				if (JustPressed(Keys.Enter))
				{
					Main.drawingPlayerChat = false;
					if (unfocusOnEnter) Unfocus();
					OnEnterPressed?.Invoke();
				}
				if (++textBlinkerCount >= 20)
				{
					textBlinkerState = (textBlinkerState + 1) % 2;
					textBlinkerCount = 0;
				}
				Main.instance.DrawWindowsIMEPanel(new Vector2(98f, Main.screenHeight - 36), 0f);
			}
			string displayString = currentString;
			if (textBlinkerState == 1 && focused)
			{
				displayString += "|";
			}
			CalculatedStyle space = GetDimensions();
			Color color = Color.Black;
			DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 drawPos = space.Center();
			if (currentString.Length == 0 && !focused)
			{
				color *= 0.5f;
				spriteBatch.DrawString(font, hintText, drawPos - font.MeasureString(hintText) / 2, color);
			}
			else
			{
				spriteBatch.DrawString(font, displayString, drawPos - new Vector2(font.MeasureString(currentString).X, font.MeasureString(displayString).Y) / 2, color);
			}
		}
	}
}