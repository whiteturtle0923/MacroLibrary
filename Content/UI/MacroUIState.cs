using System;
using MacroLibrary.Core;
using MacroLibrary.Core.Configs;
using MacroLibrary.Core.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace MacroLibrary.Content.UI
{
    internal class MacroUIState: UIState
    {
        internal MacroUIPanel macroUIPanel;
        internal UIText macroUIText;
        internal UIButton<string> macroButton;
        internal UIButton<string> recordButton;
        public override void OnInitialize()
        {
            macroUIPanel = new();
            macroUIPanel.SetPadding(20);
            SetRectangle(macroUIPanel, -100, -135, 420, 270, 0.5f, 0.5f);
            macroUIPanel.BackgroundColor = new(73, 94, 171);
            
            // Toggle/Recording
            macroUIText = new("Toggle/Record Macro");
            SetRectangle(macroUIText, 5, 0, 100, 50);
            macroUIPanel.Append(macroUIText);

            UIText filler = new("Placeholder");
            SetRectangle(filler, 0, 40, 160, 50);
            macroUIPanel.Append(filler);

            macroButton = new("Start Macro");
            SetRectangle(macroButton, 0, 110, 160, 50);
            macroButton.OnLeftClick += delegate
            {
                MacroPlayer mp = Main.LocalPlayer.GetModPlayer<MacroPlayer>();
                mp.ToggleMacro();
                if (mp.MacroOn && ModContent.GetInstance<MacroConfig>().CloseUIOnStartMacro)
                    ModContent.GetInstance<MacroUISystem>().ToggleUI();
                macroButton.SetText(string.Format("{0} Macro", mp.MacroOn ? "Stop" : "Start"));   
            };
            macroUIPanel.Append(macroButton);

            recordButton = new("Start Recording Macro");
            SetRectangle(recordButton, 0, 180, 160, 50);
            recordButton.OnLeftClick += delegate
            {
                MacroPlayer mp = Main.LocalPlayer.GetModPlayer<MacroPlayer>();
                mp.ToggleRecordingMacro();
                if (mp.Recording && ModContent.GetInstance<MacroConfig>().CloseUIOnRecordMacro)
                    ModContent.GetInstance<MacroUISystem>().ToggleUI();
                recordButton.SetText(string.Format("{0} Recording Macro", mp.MacroOn ? "Stop" : "Start"));
            };;
            macroUIPanel.Append(recordButton);

            // Save/Loading
            UIText saveUIText = new("Save/Load Macro");
            SetRectangle(saveUIText, 225, 0, 100, 50);
            macroUIPanel.Append(saveUIText);

            BetterUITextBox saveFileName = new("File Name");
            SetRectangle(saveFileName, 220, 40, 160, 50);
            macroUIPanel.Append(saveFileName);

            UIButton<string> saveButton = new("Save Macro");
            SetRectangle(saveButton, 220, 110, 160, 50);
            saveButton.OnLeftClick += new((evt, element) => Main.LocalPlayer.GetModPlayer<MacroPlayer>().SaveMacro(saveFileName.currentString == "" ? "Macro" : saveFileName.currentString));
            macroUIPanel.Append(saveButton);

            UIButton<string> loadButton = new("Load Macro");
            SetRectangle(loadButton, 220, 180, 160, 50);
            loadButton.OnLeftClick += new((evt, element) => Main.LocalPlayer.GetModPlayer<MacroPlayer>().LoadMacro(saveFileName.currentString == "" ? "Macro" : saveFileName.currentString));
            macroUIPanel.Append(loadButton);

            Append(macroUIPanel);
        }

        public override void Update(GameTime gameTime)
        {
            MacroPlayer mp = Main.LocalPlayer.GetModPlayer<MacroPlayer>();

            macroButton.SetText(string.Format("{0} Macro", mp.MacroOn ? "Stop" : "Start"));
            recordButton.SetText(string.Format("{0} Recording Macro", mp.Recording ? "Stop" : "Start"));

            base.Update(gameTime);
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

        private void MacroButtonOnClick(UIMouseEvent evt, UIElement element)
        {
            MacroPlayer mp = Main.LocalPlayer.GetModPlayer<MacroPlayer>();
            mp.ToggleMacro();
            if (mp.MacroOn && ModContent.GetInstance<MacroConfig>().CloseUIOnStartMacro)
                ModContent.GetInstance<MacroUISystem>().ToggleUI();
            macroUIText.SetText(string.Format("0/Record Macro", mp.MacroOn ? "Stop" : "Start"));
            macroButton.SetText(string.Format("0 Macro", mp.MacroOn ? "Stop" : "Start"));   
        }

        private void RecordButtonOnClick(UIMouseEvent evt, UIElement element)
        {
            MacroPlayer mp = Main.LocalPlayer.GetModPlayer<MacroPlayer>();
            mp.ToggleRecordingMacro();
            if (mp.Recording && ModContent.GetInstance<MacroConfig>().CloseUIOnRecordMacro)
                ModContent.GetInstance<MacroUISystem>().ToggleUI();
            recordButton.SetText(string.Format("{0} Recording Macro", mp.MacroOn ? "Stop" : "Start"));
        }
    }
}