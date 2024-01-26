using EmptyKeys.UserInterface.Generated.EditFactionIconView_Bindings;
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
        // GameSprites
        //----------------------------------------------------------------------
        public class GameSprites
        {
            public static Dictionary<char,string> arrows = new Dictionary<char, string>();
            public static Dictionary<char,string> pressed = new Dictionary<char, string>();
            public static Dictionary<char, string[]> hits = new Dictionary<char, string[]>();
            public static List<string> scoreNumbers = new List<string>();
            public static Dictionary<string, string> combo = new Dictionary<string, string>();
            public static string comboBarBorder;
            public static string comboBarFill;
            public static void LoadSprites()
            {
                // load arrow sprites.
                IMyTextPanel arrowDB = GridBlocks.GetTextPanel("Arrow Left W");
                string[] arrowData = arrowDB.CustomData.Split('|');
                foreach (string data in arrowData)
                {
                    string[] arrow = data.Split(':');
                    if (arrow.Length == 2)
                    {
                        switch (arrow[0])
                        {
                            case "arrows":
                                LoadArrows(arrow[1]);
                                break;
                            case "pressed":
                            case "presses":
                                LoadPressed(arrow[1]);
                                break;
                            case "hits":
                            case "hit":
                                LoadHits(arrow[1]);
                                break;
                        }
                    }
                }
                GridInfo.Echo("Loading score sprites");
                IMyTextPanel ScoreSpriteData = GridBlocks.GetTextPanel("Arrow Left A");

                string[] scoreData = ScoreSpriteData.CustomData.Split('|');
                GridInfo.Echo("Score data length " + scoreData.Length + "("+ scoreData[0].Length+")");
                foreach (string data in scoreData)
                {
                    string[] score = data.Split('-');
                    if (score.Length == 2)
                    {
                        switch (score[0])
                        {
                            case "bar":
                                LoadBar(score[1]);
                                break;
                            case "combo":
                                LoadCombo(score[1]);
                                break;
                            case "number":
                            case "numbers":
                                LoadScoreNumbers(score[1]);
                                break;
                        }
                    }
                }
            }
            static void LoadArrows(string data)
            {
                string[] arrowSprites = data.Split(',');
                arrows.Add('w', arrowSprites[0]);
                arrows.Add('a', arrowSprites[1]);
                arrows.Add('s', arrowSprites[2]);
                arrows.Add('d', arrowSprites[3]);

            }
            static void LoadPressed(string data)
            {
                string[] pressedSprites = data.Split(',');
                pressed.Add('w', pressedSprites[0]);
                pressed.Add('a', pressedSprites[1]);
                pressed.Add('s', pressedSprites[2]);
                pressed.Add('d', pressedSprites[3]);
            }
            static void LoadHits(string data)
            {
                string[] hitSprites = data.Split(',');
                hits.Add('w', hitSprites[0].Split(';'));
                hits.Add('a', hitSprites[1].Split(';'));
                hits.Add('s', hitSprites[2].Split(';'));
                hits.Add('d', hitSprites[3].Split(';'));
            }
            static void LoadBar(string data)
            {
                //GridInfo.Echo("Loading score bar sprites");
                string[] barSprites = data.Split(',');
                //GridInfo.Echo("Bar sprites " + barSprites.Length);
                comboBarFill = barSprites[0];
                comboBarBorder = barSprites[1];
            }
            static void LoadCombo(string data)
            {
                //GridInfo.Echo("Loading combo sprites");
                string[] comboSprites = data.Split(',');
                foreach (string sprite in comboSprites)
                {
                    string[] comboSprite = sprite.Split(':');
                    combo.Add(comboSprite[0], comboSprite[1]);
                    //GridInfo.Echo("Combo sprite " + comboSprite[0]);
                }
            }
            static void LoadScoreNumbers(string data)
            {
                //GridInfo.Echo("Loading score number sprites");
                string[] scoreSprites = data.Split(',');
                //GridInfo.Echo("Score sprites " + scoreSprites.Length);
                foreach (string sprite in scoreSprites)
                {
                    scoreNumbers.Add(sprite);
                    //GridInfo.Echo("Score sprite " + scoreNumbers.Count);
                }
            }
            static string[] Dancers = { "Arrow Left S", "Arrow Left D", "Arrow Right W", "Arrow Right A", "Arrow Right S", "Arrow Right D" };
            static Random rnd = new Random();
            public static void LoadDancerSprites(Dancer dancer,int index = -1)
            {
                if (index < 0 || index >= Dancers.Length) index = rnd.Next(0, Dancers.Length);
                IMyTextPanel dancerData = GridBlocks.GetTextPanel(Dancers[index]);
                dancer.LoadSprites(dancerData.CustomData);
            }
            static string[] backs = { "Sign 1", "Sign 2", "Sign 3", "Sign 4" };
            public static string GetBackground()
            {
                int b = rnd.Next(0, 100) % 4;
                if (b == 4)
                {
                    IMyTextPanel background = GridBlocks.GetTextPanel("Main Display");
                    if (rnd.Next(0, 100) % 2 == 0)
                    {
                        return background.CustomData;
                    }
                    return background.GetText();
                }
                IMyTextPanel back = GridBlocks.GetTextPanel(backs[b]);
                return back.CustomData;
            }
        }
        //----------------------------------------------------------------------
    }
}
