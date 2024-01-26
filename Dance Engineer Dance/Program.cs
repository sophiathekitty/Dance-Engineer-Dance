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
    partial class Program : MyGridProgram
    {
        //=======================================================================
        GameInput leftPlayer;
        GameInput rightPlayer;
        Dancepad dancepad;
        DanceGame game;
        List<CoinTray> coinTrays = new List<CoinTray>();
        public Program()
        {
            GridInfo.Init("Dance Engineer Dance",GridTerminalSystem,IGC,Me,Echo);
            GridBlocks.RefreshGridBlocks();
            GridInfo.Load(Storage);
            GameSprites.LoadSprites();
            leftPlayer = new GameInput(GridBlocks.GetController("left"),GridBlocks.GetSensor("Player Sensor Left"),GridBlocks.GetSensor("Left W"),GridBlocks.GetSensor("left A"),GridBlocks.GetSensor("left S"),GridBlocks.GetSensor("left D"));
            rightPlayer = new GameInput(GridBlocks.GetController("right"),GridBlocks.GetSensor("Player Sensor Right"),GridBlocks.GetSensor("Right W"),GridBlocks.GetSensor("right A"),GridBlocks.GetSensor("right S"),GridBlocks.GetSensor("right D"));
            dancepad = new Dancepad(leftPlayer,rightPlayer);
            game = new DanceGame(GridBlocks.GetTextPanel("Main"),leftPlayer,rightPlayer);
            coinTrays.Add(new CoinTray(leftPlayer.GetSurface(0),leftPlayer));
            coinTrays.Add(new CoinTray(rightPlayer.GetSurface(0),rightPlayer));
            IMyTextSurfaceProvider coinTray = GridBlocks.GetTurretControlBlock("Coin Tray") as IMyTextSurfaceProvider;
            if (coinTray != null)
            {
                coinTrays.Add(new CoinTray(coinTray.GetSurface(0)));
            }
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save()
        {
            Storage = GridInfo.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            dancepad.Update();
            game.Draw();
            foreach (CoinTray coinTray in coinTrays)
            {
                coinTray.Draw();
            }
        }
        //=======================================================================
    }
}
