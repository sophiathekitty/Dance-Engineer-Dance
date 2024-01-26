using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class DiscoLight
        {
            public static List<Color> Colors = new List<Color>();
            public static void InitColors()
            {
                Colors.Add(Color.Red);
                Colors.Add(Color.Green);
                Colors.Add(Color.Blue);
                Colors.Add(Color.Yellow);
                Colors.Add(Color.Purple);
                Colors.Add(Color.White);
            }
            int colorIndex = 0;
            public Color Color { get { return Colors[colorIndex]; } set { colorIndex = Colors.IndexOf(value); } }
            int nextColorDelay = 0;
            public void NextColor()
            {
                if (Colors.Count == 0) InitColors();
                if (nextColorDelay > 0)
                {
                    nextColorDelay--;
                    return;
                }
                nextColorDelay = 10;
                colorIndex++;
                if (colorIndex >= Colors.Count) colorIndex = 0;
                light.Color = Color;
            }
            IMyLightingBlock light;
            IMyMotorStator motor;
            float level = 0; // how well the player is doing. the more the disco should be bopping
            public float Level { get { return level; } set { level = value; update(); } }
            public DiscoLight(string name)
            {
                List<ITerminalProperty> props = new List<ITerminalProperty>();
                light = GridBlocks.GetLight(name);
                motor = GridBlocks.GetMotorStator(name);
            }
            public void Beat()
            {
                if(light == null || motor == null) return;
                motor.TargetVelocityRPM = level * 25;
            }
            public void MidBeat()
            {
                if (light == null || motor == null) return;
                motor.TargetVelocityRPM = level * -25;
            }
            public void Update()
            {
                if(light == null || motor == null) return;
                if(level > 0)
                {
                    NextColor();
                    light.Enabled = true;
                    light.Intensity = level * 2;
                    light.SetValueFloat("RotationSpeed", level * 90);
                }
                else
                {
                    light.Enabled = false;
                    motor.TargetVelocityRPM = 0;
                }
            }
            void update()
            {
                if (light == null || motor == null) return;
                if (level <= 0)
                {
                    light.Enabled = false;
                    motor.TargetVelocityRPM = 0;
                }
            }
        }
    }
}
