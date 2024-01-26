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
        public class CoinTray : Screen
        {
            ScreenSprite CoinSlot;
            ScreenSprite CoinSlotHighlight;
            ScreenSprite InsertText;
            GameInput input;
            public CoinTray(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                Init();
            }
            public CoinTray(IMyTextSurface drawingSurface, GameInput input) : base(drawingSurface)
            {
                this.input = input;
                Init();
            }
            void Init()
            {
                BackgroundColor = Color.Black;
                // if landscape, draw CoinSlot on the left and InsertText on the right (use Size)
                if (Size.X > Size.Y && input != null)
                {
                    CoinSlot = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.CenterLeft, new Vector2(Size.X * 0.1f, 0f), 0f, new Vector2(Size.Y*0.25f,Size.Y*0.75f), Color.Orange, "", "SquareSimple", TextAlignment.CENTER, SpriteType.TEXTURE);
                    CoinSlotHighlight = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.CenterLeft, new Vector2(Size.X*0.1f, 0f), 0f, new Vector2(Size.Y*0.1f,Size.Y*0.5f), Color.Black, "", "SquareSimple", TextAlignment.CENTER, SpriteType.TEXTURE);
                    InsertText = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(Size.X * 0.1f, Size.Y*-0.4f), 4f, Vector2.Zero, Color.Orange, "Monospace", "INSERT COIN", TextAlignment.CENTER, SpriteType.TEXT);
                }
                // if portrait, draw CoinSlot on the bottom and InsertText on the top (use Size)
                else
                {
                    CoinSlot = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(Size.X * 0.1f, 0f), 0f, new Vector2(Size.Y * 0.1f, Size.Y * 0.4f), Color.Orange, "", "SquareSimple", TextAlignment.CENTER, SpriteType.TEXTURE);
                    CoinSlotHighlight = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(Size.X * 0.1f, 0f), 0f, new Vector2(Size.Y * 0.05f, Size.Y * 0.2f), Color.Black, "", "SquareSimple", TextAlignment.CENTER, SpriteType.TEXTURE);
                    InsertText = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0f, Size.Y * 0.1f), 1f, Vector2.Zero, Color.Orange, "Monospace", "INSERT COIN", TextAlignment.CENTER, SpriteType.TEXT);
                }
                AddSprite(CoinSlot);
                AddSprite(CoinSlotHighlight);
                AddSprite(InsertText);
            }
            override public void Draw()
            {
                if (input != null)
                {
                    if(input.PlayerPresent)
                    {
                        InsertText.Data = "";
                    }
                    else if(input.PlayerJoining)
                    {
                        InsertText.Data = "PLAYER\nJOINING";
                    }
                    else if (input.PlayerLeaving)
                    {
                        InsertText.Data = "PLAYER\nLEAVING";
                    }
                    else
                    {
                        InsertText.Data = "INSERT\nCOIN";
                    }
                    //InsertText.Data = input.PlayerPresent ? "" : "INSERT\nCREDIT";
                }
                base.Draw();
            }
        }
    }
}
