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
    //-----------------------------------------------------------------------
    // GridBlocks
    //-----------------------------------------------------------------------
    partial class Program
    {
        public class GridBlocks
        { 
            public static List<IMyShipController> controllers = new List<IMyShipController>();
            public static List<IMyTextPanel> textPanels = new List<IMyTextPanel>();
            public static List<IMyLightingBlock> lights = new List<IMyLightingBlock>();
            public static List<IMySoundBlock> speakers = new List<IMySoundBlock>();
            public static List<IMySensorBlock> mySensors = new List<IMySensorBlock>();
            public static List<IMyMotorStator> myMotorStators = new List<IMyMotorStator>();
            public static List<IMyTurretControlBlock> myTurretControlBlocks = new List<IMyTurretControlBlock>();
            public static void RefreshGridBlocks()
            {
                controllers.Clear();
                textPanels.Clear();
                lights.Clear();
                speakers.Clear();
                mySensors.Clear();
                myMotorStators.Clear();
                myTurretControlBlocks.Clear();
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMyShipController>(controllers);
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(textPanels);
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMyLightingBlock>(lights);
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMySoundBlock>(speakers);
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(mySensors);
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(myMotorStators);
                GridInfo.GridTerminalSystem.GetBlocksOfType<IMyTurretControlBlock>(myTurretControlBlocks);
            }
            public static IMyShipController GetController(string name)
            {
                return controllers.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMyTextPanel GetTextPanel(string name)
            {
                return textPanels.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMyLightingBlock GetLight(string name)
            {
                return lights.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMySoundBlock GetSpeaker(string name)
            {
                return speakers.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMySensorBlock GetSensor(string name)
            {
                return mySensors.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMyMotorStator GetMotorStator(string name)
            {
                return myMotorStators.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
            public static IMyTurretControlBlock GetTurretControlBlock(string name)
            {
                return myTurretControlBlocks.Find(x => x.CustomName.ToLower().Contains(name.ToLower()));
            }
        }
    }
    //-----------------------------------------------------------------------
}
