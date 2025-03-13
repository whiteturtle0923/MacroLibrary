#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MacroLibrary.Core.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
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

    public struct ControlSet
    {
        public Controls ControlType;
        public int StartTime;

    }

    public class MacroPlayer: ModPlayer
    {

        public List<(Controls Control, int StartTime, int EndTime)> Instructions = 
        [
            (Controls.Left, 60, 120),
            (Controls.Right, 120, 180),
        ];
        internal List<bool> InstructionsEnabled = [.. Enumerable.Repeat(true, 2)];

        // these fields are for the current controls in the instructions
        private bool Up = false;
        private bool Down = false;
        private bool Left = false;
        private bool Right = false;
        private bool Jump = false;

        public bool MacroOn {get; internal set;} = false;
        public int MacroTimer {get; internal set;} = 0;
        public bool Recording {get; internal set;} = false;
        public int RecordingTimer {get; internal set;} = 0;

        public static readonly string SaveDir = Path.Join(Main.SavePath, "Macros");

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
                    Instructions.Add((control, RecordingTimer, -1));
                }
                else if (controls[i] is null && previousControls[i] is Controls previousControl)
                {
                    int lastControlIndex = Instructions.FindLastIndex(x => x.Control == previousControl);
                    int lastControlStart = Instructions[lastControlIndex].StartTime;
                    Instructions[lastControlIndex] = (previousControl, lastControlStart, RecordingTimer);
                }
                else
                {
                    Main.NewText("what the sigma (this shouldn't happen)", Color.Gold);
                }
            }
            RecordingTimer++;
            previousControls = controls;
        }

        public override void SetControls()
        {
            if (!MacroOn) 
            {
                MacroTimer = 0;
                return;
            }
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            Jump = false;
            foreach ((Controls Control, int StartTime, int EndTime) in Instructions)
            {
                if (StartTime <= MacroTimer && EndTime > MacroTimer)
                {
                    Up = Control == Controls.Up || Up;
                    Down = Control == Controls.Down || Down;
                    Left = Control == Controls.Left || Left;
                    Right = Control == Controls.Right || Right;
                    Jump = Control == Controls.Jump || Jump;
                }
                else if (EndTime <= MacroTimer)
                {
                    InstructionsEnabled[Instructions.FindIndex(x => x.Control == Control && x.StartTime == StartTime && x.EndTime == EndTime)] = false;
                }
            }
            MacroTimer++;
            if (!InstructionsEnabled.Contains(true))
            {
                MacroOn = false;
                Main.NewText("Macro Stopped");
                MacroTimer = 0;
                return;
            }
            Player.controlUp = Up;
            Player.controlDown = Down;
            Player.controlLeft = Left;
            Player.controlRight = Right;
            Player.controlJump = Jump;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (MacroLibrary.MacroMenuKeybind.JustPressed)
                ModContent.GetInstance<MacroUISystem>().ToggleUI();
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Recording)
                ToggleRecordingMacro();
            if (MacroOn)
                ToggleMacro();
        }

        public List<Vector2> RecordingPositions = [];
        public List<Vector2> MacroPositions = [];
        public override void PostUpdate()
        {
            if (Recording)
            {
                RecordingPositions.Add(Player.position);
            }
            else if (MacroOn)
            {
                MacroPositions.Add(Player.position);
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

        private Vector2 pos = new(Main.spawnTileX, Main.spawnTileY);
        public void ToggleMacro()
        {
            if (Recording)
            {
                Main.NewText("Cannot Start Macro While Recording");
                return;
            }
            if (MacroOn)
            {
                MacroOn = false;
                InstructionsEnabled = [.. Enumerable.Repeat(true, Instructions.Capacity)];
                Main.NewText("Macro Stopped");
            }
            else
            {
                MacroOn = true;
                InstructionsEnabled = [.. Enumerable.Repeat(true, Instructions.Capacity)];
                Main.NewText("Macro Started");

                MacroPositions.Clear();
                Player.Teleport(pos);
                //NPC.NewNPC(Player.GetSource_FromThis(), (int)Player.position.X, (int)Player.position.Y + 200, NPCID.HallowBoss);
            }
        }

        public void ToggleRecordingMacro()
        {
            if (MacroOn)
            {
                Main.NewText("Cannot Start Recording While Playing Macro");
                return;
            }
            if (Recording)
            {
                for (int i = 0; i < Instructions.Count; i++)
                {
                    (Controls Control, int StartTime, int EndTime) = Instructions[i];
                    if (EndTime == -1)
                    {
                        Instructions[i] = (Control, StartTime, RecordingTimer);
                    }
                }
                Recording = false;
                RecordingTimer = 0;
                Array.Clear(previousControls);
                Instructions.Capacity = Instructions.Count;
                InstructionsEnabled = [.. Enumerable.Repeat(true, Instructions.Capacity)];
                Main.NewText("Macro Recording Stopped");
            }
            else
            {
                Recording = true;
                Instructions.Clear();
                Main.NewText("Macro Recording Started");
                
                RecordingPositions.Clear();
                pos = Player.position;
                //NPC.NewNPC(Player.GetSource_FromThis(), (int)Player.position.X, (int)Player.position.Y + 200, NPCID.HallowBoss);
            }
        }

        private string previousSavePath = "";
        internal void SaveMacro(string fileName)
        {
            string SavePath = Path.Join(SaveDir, string.Join(null, fileName, ".macro"));
            Main.NewText("test");
            if (File.Exists(SavePath) && SavePath != previousSavePath)
            {
                Main.NewText("Warning: File exists at specified path and will be overwritten. Save again to confirm that you want to overwrite");
                previousSavePath = SavePath;
                return;
            }
            List<byte> fileBytes = [];
            foreach ((Controls Control, int StartTime, int EndTime) in Instructions)
            {
                fileBytes.Add((byte)Control);
                fileBytes.AddRange(BitConverter.GetBytes(StartTime));
                fileBytes.AddRange(BitConverter.GetBytes(EndTime));
            }
            if (!Directory.Exists(SaveDir))
                Directory.CreateDirectory(SaveDir);
            using (FileStream stream = File.Create(SavePath))
            {
                stream.Write([.. fileBytes]);
            }
            Main.NewText("Macro Saved"); 
            if (SavePath == previousSavePath)
                previousSavePath = "";
        }

        internal void LoadMacro(string fileName)
        {
            string SavePath = Path.Join(SaveDir, string.Join(null, fileName, ".macro"));
            if (!File.Exists(SavePath))
            {
                Main.NewText("Error: File does not exist at specified path");
                return;
            }
            byte[] saveData = File.ReadAllBytes(SavePath);
            List<byte[]> splitSaveData = [];
            List<(Controls Control, int StartTime, int EndTime)> newInstructions = [];
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
                    (
                        (Controls)controlSet[0],
                        BitConverter.ToInt32(new ArraySegment<byte>(controlSet, 1, 4)),
                        BitConverter.ToInt32(new ArraySegment<byte>(controlSet, 5, 4))
                    )
                );
            }
            Instructions = newInstructions;
            Instructions.Capacity = Instructions.Count; // idk why this is necessary
            Main.NewText("Macro Loaded"); 
        }
    }
}