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
        //----------------------------------------------------------------------
        // Game pad to handle the input for both players and lighting up the dance floor
        //----------------------------------------------------------------------
        public class Dancepad
        {
            GameInput leftPlayer;
            GameInput rightPlayer;
            Dictionary<char, FloorArrow> floorArrowsLeft = new Dictionary<char, FloorArrow>();
            Dictionary<char, FloorArrow> floorArrowsRight = new Dictionary<char, FloorArrow>();
            public static Dictionary<char,Color> arrowColors = new Dictionary<char, Color>();
            public Dancepad(GameInput leftPlayer, GameInput rightPlayer)
            {
                arrowColors.Clear();
                this.leftPlayer = leftPlayer;
                this.rightPlayer = rightPlayer;
                arrowColors.Add('w',Color.Yellow);
                arrowColors.Add('a',Color.Blue);
                arrowColors.Add('s',Color.Green);
                arrowColors.Add('d',Color.Red);
                foreach (KeyValuePair<char,Color> arrow in arrowColors)
                {
                    floorArrowsLeft.Add(arrow.Key,new FloorArrow(GridBlocks.GetTextPanel("Arrow Left " + arrow.Key), arrow.Value, GameSprites.arrows[arrow.Key]));
                    floorArrowsRight.Add(arrow.Key,new FloorArrow(GridBlocks.GetTextPanel("Arrow Right " + arrow.Key), arrow.Value, GameSprites.arrows[arrow.Key]));
                }
            }
            public void Update()
            {
                //GridInfo.Echo("Dancepad Update");
                if (leftPlayer.PlayerPresent || leftPlayer.PlayerJoining)
                {
                    floorArrowsLeft['w'].Active = leftPlayer.Wpad;
                    floorArrowsLeft['a'].Active = leftPlayer.Apad;
                    floorArrowsLeft['s'].Active = leftPlayer.Spad;
                    floorArrowsLeft['d'].Active = leftPlayer.Dpad;
                    foreach (KeyValuePair<char, FloorArrow> arrow in floorArrowsLeft)
                    {
                        arrow.Value.Draw();
                    }
                }
                else
                {
                    foreach (KeyValuePair<char, FloorArrow> arrow in floorArrowsLeft)
                    {
                        arrow.Value.Active = false;
                        arrow.Value.Draw();
                    }
                }
                if (rightPlayer.PlayerPresent || rightPlayer.PlayerJoining)
                {
                    floorArrowsRight['w'].Active = rightPlayer.Wpad;
                    floorArrowsRight['a'].Active = rightPlayer.Apad;
                    floorArrowsRight['s'].Active = rightPlayer.Spad;
                    floorArrowsRight['d'].Active = rightPlayer.Dpad;
                    foreach (KeyValuePair<char, FloorArrow> arrow in floorArrowsRight)
                    {
                        arrow.Value.Draw();
                    }
                }
                else
                {
                    foreach (KeyValuePair<char, FloorArrow> arrow in floorArrowsRight)
                    {
                        arrow.Value.Active = false;
                        arrow.Value.Draw();
                    }
                }
            }
        }
        //----------------------------------------------------------------------
    }
}
