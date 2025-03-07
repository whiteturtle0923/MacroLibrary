#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net.Repository.Hierarchy;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MacroLibrary.Core
{
    public enum Controls
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
    }
    public class MacroPlayer: ModPlayer
    {
        public List<Tuple<Controls, int, int>> Instructions = 
        [
            Tuple.Create(Controls.Left, 60, 60),
            Tuple.Create(Controls.Right, 120, 60),
        ];
        public List<bool> InstructionsEnabled = [.. Enumerable.Repeat(true, 2)];
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;
        public bool Jump = false;
        public int Timer = 0;
        public bool MacroOn = false;
        public bool Recording = false;
        public int RecordingTime = 0;

        internal static string SaveDir => Path.Join(Main.SavePath, "Macros");

        // Will be removed in the future when I get around to having multiple macros with like ui or smth
        // Currently tho idfk how to do ui
        internal static string SavePath => Path.Join(SaveDir, "Macro.macro");

        internal Controls?[] previousControls = new Controls?[5];
        public override void PreUpdate()
        {
            if (!Recording) return;
            Controls?[] controls = GetControls();
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] == previousControls[i]) continue;
                if (controls[i] is Controls control && previousControls[i] is null)
                {
                    Instructions.Add(new(control, RecordingTime, -1));
                }
                else if (controls[i] is null && previousControls[i] is Controls previousControl)
                {
                    int lastControlIndex = Instructions.FindLastIndex(x => x.Item1 == previousControl);
                    int lastControlStart = Instructions[lastControlIndex].Item2;
                    Instructions[lastControlIndex] = new(previousControl, lastControlStart, RecordingTime - lastControlStart);
                }
                else
                {
                    Main.NewText("what the sigma (this shouldn't happen)", Microsoft.Xna.Framework.Color.Gold);
                }
            }
            RecordingTime++;
            previousControls = controls;
        }

        public override void SetControls()
        {
            if (!MacroOn) 
            {
                Timer = 0;
                return;
            }
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            Jump = false;
            foreach (Tuple<Controls, int, int> controlSet in Instructions)
            {
                if (controlSet.Item2 < Timer && controlSet.Item3 + controlSet.Item2 > Timer)
                {
                    Up = controlSet.Item1 == Controls.Up || Up;
                    Down = controlSet.Item1 == Controls.Down || Down;
                    Left = controlSet.Item1 == Controls.Left || Left;
                    Right = controlSet.Item1 == Controls.Right || Right;
                    Jump = controlSet.Item1 == Controls.Jump || Jump;
                }
                else if (controlSet.Item3 + controlSet.Item2 <= Timer)
                {
                    InstructionsEnabled[Instructions.IndexOf(controlSet)] = false;
                }
            }
            Timer++;
            if (!InstructionsEnabled.Contains(true))
            {
                MacroOn = false;
                Main.NewText("Macro Stopped");
                Timer = 0;
                return;
            }
            Player.controlUp = Up;
            Player.controlDown = Down;
            Player.controlLeft = Left;
            Player.controlRight = Right;
            Player.controlJump = Jump;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Recording)
            {
                Recording = false;
                RecordingTime = 0;
                Array.Clear(previousControls);
                Main.NewText("Macro Recording Stopped");
            }
            if (MacroOn)
            {
                MacroOn = false;
                InstructionsEnabled = [.. Enumerable.Repeat(true, Instructions.Capacity)];
                Main.NewText("Macro Stopped");
            }
        }

        public Controls?[] GetControls() =>
        [
            Player.controlUp ? Controls.Up : null,
            Player.controlDown ? Controls.Down : null,
            Player.controlLeft ? Controls.Left : null,
            Player.controlRight ? Controls.Right : null,
            Player.controlJump ? Controls.Jump : null,
        ];

        internal void SaveMacro()
        {
            List<byte> fileBytes = [];
            foreach (Tuple<Controls, int, int> controlSet in Instructions)
            {
                fileBytes.Add((byte)controlSet.Item1);
                fileBytes.AddRange(BitConverter.GetBytes(controlSet.Item2));
                fileBytes.AddRange(BitConverter.GetBytes(controlSet.Item3));
            }
            if (!Directory.Exists(SaveDir))
                Directory.CreateDirectory(SaveDir);
            if (!File.Exists(SavePath))
                File.Create(SavePath);
            File.WriteAllBytes(SavePath, [.. fileBytes]);
        }

        internal void LoadMacro()
        {
            byte[] saveData = File.ReadAllBytes(SavePath);
            List<byte[]> splitSaveData = [];
            List<Tuple<Controls, int, int>> newInstructions = [];
            byte[] tempByteArray;
            for (int i = 0; i < saveData.Length; i += 9)
            {
                tempByteArray = new byte[9];
                Array.Copy(saveData, i, tempByteArray, 0, 9);
                splitSaveData.Add(tempByteArray);
            }
            foreach (byte[] controlSet in splitSaveData)
            {
                newInstructions.Add(
                    new(
                        (Controls)controlSet[0],
                        BitConverter.ToInt32(new ArraySegment<byte>(controlSet, 1, 4)),
                        BitConverter.ToInt32(new ArraySegment<byte>(controlSet, 5, 4))
                    )
                );
            }
            Instructions = newInstructions;
            Instructions.Capacity = Instructions.Count; // idk why this is necessary
        }
    }
}