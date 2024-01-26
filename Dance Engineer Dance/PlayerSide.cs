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
        // PlayerSide - wrapper to hold all the stuff for one of the players
        //----------------------------------------------------------------------
        public class PlayerSide
        {
            GameInput input;
            DiscoLight discoLight;
            PlayerScoreDisplay scoreDisplay;
            Dictionary<char, ArrowColumn> columns = new Dictionary<char, ArrowColumn>();
            Dancer dancer;
            Vector2 position;
            Screen screen;
            GameMenu gameMenu;
            public float Combo { get { return scoreDisplay.Combo; } set { scoreDisplay.Combo = value; discoLight.Level = value; } }
            public bool Visible { get { return scoreDisplay.Visible; } set { scoreDisplay.Visible = value; foreach (var column in columns) column.Value.Visible = value; dancer.Visible = value; } }
            public bool inGame = false;
            //----------------------------------------------------------------------
            // PlayerSide constructor
            //----------------------------------------------------------------------
            public PlayerSide(GameInput input, DiscoLight discoLight, Screen screen, Vector2 position, GameMenu gameMenu)
            {
                this.input = input;
                this.discoLight = discoLight;
                this.screen = screen;
                this.position = position;
                dancer = new Dancer(position + new Vector2(200f, 190f));
                screen.AddSprite(dancer);
                float arrowSpace = 100f;
                scoreDisplay = new PlayerScoreDisplay(position, discoLight, dancer);
                position += new Vector2(30f, 100f);
                columns.Add('a', new ArrowColumn('a', position, Dancepad.arrowColors['a'], scoreDisplay));
                columns['a'].AddToScreen(screen);
                position.X += arrowSpace;
                columns.Add('w', new ArrowColumn('w', position, Dancepad.arrowColors['w'], scoreDisplay));
                columns['w'].AddToScreen(screen);
                position.X += arrowSpace;
                columns.Add('s', new ArrowColumn('s', position, Dancepad.arrowColors['s'], scoreDisplay));
                columns['s'].AddToScreen(screen);
                position.X += arrowSpace;
                columns.Add('d', new ArrowColumn('d', position, Dancepad.arrowColors['d'], scoreDisplay));
                columns['d'].AddToScreen(screen);
                // add score to the top of the screen
                scoreDisplay.AddToScreen(screen);
                this.gameMenu = gameMenu;
                gameMenu.AddToScreen(screen);
                gameMenu.player = this;
            }
            public void ClearArrows()
            {
                foreach (ArrowColumn column in columns.Values)
                {
                    column.ClearArrows();
                }
            }
            public void AddArrowOnBeat(char arrow,int beat)
            {
                columns[arrow].AddArrowOnBeat(beat);
            }
            public void BringScoreToFront()
            {
                scoreDisplay.BringToFront(screen);
            }
            public void Update()
            {
                discoLight.Update();
                //dancer.NextSprite();
                if (input.PlayerPresent && inGame)
                {
                    if (input.WPressed) columns['w'].Press();
                    if (input.APressed) columns['a'].Press();
                    if (input.SPressed) columns['s'].Press();
                    if (input.DPressed) columns['d'].Press();
                }
                foreach (ArrowColumn column in columns.Values)
                {
                    column.Scroll(DanceGame.songTime/DanceGame.songLength);
                }
            }
            public void Beat()
            {
                discoLight.Beat();
                dancer.NextSprite();
            }
            public void MidBeat()
            {
                discoLight.MidBeat();
            }
        }
    }
}
