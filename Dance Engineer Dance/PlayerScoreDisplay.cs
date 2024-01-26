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
using VRageRender.Messages;

namespace IngameScript
{
    partial class Program
    {
        //----------------------------------------------------------------------
        // PlayerScoreDisplay
        //----------------------------------------------------------------------
        public class PlayerScoreDisplay : IScreenSpriteProvider
        {
            List<ScreenSprite> ScoreDigits = new List<ScreenSprite>();
            ScreenSprite ComboBarBorder;
            ScreenSprite ComboBarFill;
            DiscoLight discoLight;
            Dancer dancer;
            long score = 0;
            public bool Visible { get { return ComboBarBorder.Visible; } set { ComboBarBorder.Visible = value; ComboBarFill.Visible = value; foreach (ScreenSprite sprite in ScoreDigits) sprite.Visible = value; } }
            public long Score 
            { 
                get { return score; } 
                set 
                { 
                    score = value;
                    if(score > 999999999999) score = 999999999999;
                    if(score < 0) score = 0;
                    // convert score to string
                    string scoreString = score.ToString();
                    // pad the string with zeros
                    while (scoreString.Length < 12)
                    {
                        scoreString = "0" + scoreString;
                    }
                    // set the sprites
                    for (int i = 0; i < 12; i++)
                    {
                        ScoreDigits[i].Data = GameSprites.scoreNumbers[int.Parse(scoreString[i].ToString())];
                    }
                } 
            }
            float combo = 0f; // percentage
            public float Combo
            {
                get { return combo; }
                set
                {
                    combo = value;
                    if (combo > 1f) combo = 1f;
                    if (combo < 0f) combo = 0f;
                    if (discoLight != null) discoLight.Level = combo;
                    if (dancer != null) dancer.Combo = combo;
                    string[] comboBarLines = GameSprites.comboBarFill.Split('\n');
                    int maxLineLength = comboBarLines[0].Length;
                    int fillLength = (int)(maxLineLength * combo);
                    // fill the combo bar with sub strings of the fill sprite
                    string comboBarFill = "";
                    for (int i = 0; i < comboBarLines.Length; i++)
                    {
                        comboBarFill += comboBarLines[i].Substring(0, fillLength);
                        // now fill the rest of the line with spaces
                        comboBarFill += "\n";
                    }
                    ComboBarFill.Data = comboBarFill;
                }
            }
            int perfects = 0;
            int greats = 0;
            int goods = 0;
            int misses = 0;
            public PlayerScoreDisplay(Vector2 position, DiscoLight discoLight, Dancer dancer)
            {
                ComboBarBorder = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, 0.1f, Vector2.Zero, Color.White, "Monospace", GameSprites.comboBarBorder, TextAlignment.LEFT, SpriteType.TEXT);
                ComboBarFill = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, 0.1f, Vector2.Zero, Color.White, "Monospace", GameSprites.comboBarFill, TextAlignment.LEFT, SpriteType.TEXT);
                position += new Vector2(15f, 45f);
                for (int i = 0; i < 12; i++)
                {
                    ScoreDigits.Add(new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, 0.1f, Vector2.Zero, Color.White, "Monospace", GameSprites.scoreNumbers[0], TextAlignment.CENTER, SpriteType.TEXT));
                    position += new Vector2(30f, 0f);
                }
                Combo = 0f;
                this.discoLight = discoLight;
                this.dancer = dancer;
            }
            public void AddToScreen(Screen screen)
            {
                screen.AddSprite(ComboBarFill);
                screen.AddSprite(ComboBarBorder);
                foreach (ScreenSprite sprite in ScoreDigits)
                {
                    screen.AddSprite(sprite);
                }
            }

            public void RemoveFromScreen(Screen screen)
            {
                GridInfo.Echo("Removing PlayerScoreDisplay from screen");
                screen.RemoveSprite(ComboBarBorder);
                screen.RemoveSprite(ComboBarFill);
                foreach (ScreenSprite sprite in ScoreDigits)
                {
                    screen.RemoveSprite(sprite);
                }
            }
            public void BringToFront(Screen screen) 
            {
                RemoveFromScreen(screen);
                AddToScreen(screen);
            }
        }
    }
}
