using MacroLibrary.Core;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace MacroLibrary.Content.UI
{
    internal class SaveUIState: UIState
    {
        public SaveUIPanel saveUIPanel;
        public SaveButton saveButton;
        public override void OnInitialize()
        {
            saveUIPanel = new();
            saveUIPanel.SetPadding(20);
            SetRectangle(saveUIPanel, -100, -135, 200, 270, 0.5f, 0.5f);
            saveUIPanel.BackgroundColor = new(73, 94, 171);

            UIText saveUIText = new("Save/Load Macro");
            saveUIText.DynamicallyScaleDownToWidth = true;
            SetRectangle(saveUIText, -75, 0, 100, 50, 0.5f);
            saveUIPanel.Append(saveUIText);

            BetterUITextBox saveFileName = new("File Name");
            SetRectangle(saveFileName, 0, -190, 0, 50, 0, 1, 1);
            saveUIPanel.Append(saveFileName);

            UIButton<string> saveButton = new("Save Macro");
            SetRectangle(saveButton, 0, -120, 0, 50, 0, 1, 1);
            saveButton.OnLeftClick += new((evt, element) => Main.LocalPlayer.GetModPlayer<MacroPlayer>().SaveMacro(saveFileName.currentString == "" ? "Macro" : saveFileName.currentString));
            saveUIPanel.Append(saveButton);

            UIButton<string> loadButton = new("Load Macro");
            SetRectangle(loadButton, 0, -50, 0, 50, 0, 1, 1);
            loadButton.OnLeftClick += new((evt, element) => Main.LocalPlayer.GetModPlayer<MacroPlayer>().LoadMacro(saveFileName.currentString == "" ? "Macro" : saveFileName.currentString));
            saveUIPanel.Append(loadButton);

            Append(saveUIPanel);
        }

        internal static void SetRectangle
            (UIElement uiElement, float left, float top, float width, float height,
            float leftPercent = 0, float topPercent = 0, float widthPercent = 0, float heightPercent = 0) 
        {
			uiElement.Left.Set(left, leftPercent);
			uiElement.Top.Set(top, topPercent);
			uiElement.Width.Set(width, widthPercent);
			uiElement.Height.Set(height, heightPercent);
		}
    }
}