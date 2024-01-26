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
        public class GameMenu : IScreenSpriteProvider
        {
            List<ScreenSprite> menuItems = new List<ScreenSprite>();
            int selectedItem = 0;
            string menuTitle = "Game Mode";
            ScreenSprite title;
            ScreenSprite selectionIndicator;
            Screen screen;
            GameInput input;
            Dictionary<string,string> menus = new Dictionary<string,string>();
            DanceGame game;
            public PlayerSide player;
            public bool Visible { get { return title.Visible; } set { title.Visible = value; selectionIndicator.Visible = value; foreach (ScreenSprite sprite in menuItems) sprite.Visible = value; } }
            public void AddToScreen(Screen screen)
            {
                this.screen = screen;
                screen.AddSprite(title);
                screen.AddSprite(selectionIndicator);
                foreach (ScreenSprite sprite in menuItems) screen.AddSprite(sprite);
            }

            public void RemoveFromScreen(Screen screen)
            {
                this.screen = screen;
                screen.RemoveSprite(title);
                screen.RemoveSprite(selectionIndicator);
                foreach (ScreenSprite sprite in menuItems) screen.RemoveSprite(sprite);
            }
            float lineHieght = 30f;
            float fontSize = 1.4f;
            public GameMenu(Vector2 position, GameInput input, DanceGame game)
            {
                title = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, fontSize * 1.1f, Vector2.Zero, Color.White, "Monospace", "Game Mode", TextAlignment.LEFT, SpriteType.TEXT);
                position.Y += lineHieght * fontSize * 1.1f;
                selectionIndicator = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, fontSize, Vector2.Zero, Color.White, "Monospace", ">", TextAlignment.RIGHT, SpriteType.TEXT);
                menuItems.Add(new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, fontSize, Vector2.Zero, Color.White, "Monospace", "Quick Play", TextAlignment.LEFT, SpriteType.TEXT));
                position.Y += lineHieght * fontSize;
                menuItems.Add(new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, fontSize, Vector2.Zero, Color.White, "Monospace", "Select Song", TextAlignment.LEFT, SpriteType.TEXT));
                //position.Y += lineHieght * fontSize;
                //menuItems.Add(new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, position, fontSize, Vector2.Zero, Color.White, "Monospace", "Multi Song", TextAlignment.LEFT, SpriteType.TEXT));
                this.input = input;
                menus.Add("Game Mode", "Quick Play\nSelect Song");
                menus.Add("Select Song", Song.SongListString);
                List<string> albums = Song.GetAlbums();
                /*string albumsString = "";
                foreach (string album in albums)
                {
                    albumsString += album + "\n";
                    menus.Add(album, Song.GetAlbumSongs(album));
                }
                */
                //menus.Add("Multi Song", "Music Comp 1\nMusic Comp 2");
                this.game = game;
            }
            void LoadMenu(string key)
            {
                string data = menus[key];
                menuTitle = key;
                title.Data = menuTitle;
                foreach (ScreenSprite sprite in menuItems) screen.RemoveSprite(sprite);
                menuItems.Clear();
                string[] items = data.Split('\n');
                float itemFontSize = fontSize;
                if (items.Length > 5) itemFontSize = fontSize * 0.7f;
                float positionY = title.Position.Y + lineHieght * fontSize * 1.1f;
                foreach (string item in items)
                {
                    if(item == "") continue;
                    menuItems.Add(new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(title.Position.X, positionY), itemFontSize, Vector2.Zero, Color.White, "Monospace", item, TextAlignment.LEFT, SpriteType.TEXT));
                    positionY += lineHieght * itemFontSize;
                    screen.AddSprite(menuItems[menuItems.Count - 1]);
                }
                selectedItem = 0;
                selectionIndicator.Position = menuItems[selectedItem].Position;
            }
            public void Update()
            {
                if (input == null) return;
                if (input.PlayerPresent)
                {
                    Visible = true;
                    title.Data = menuTitle;
                    if (input.WPressed)
                    {
                        selectedItem--;
                        if (selectedItem < 0) selectedItem = menuItems.Count - 1;
                        selectionIndicator.Position = menuItems[selectedItem].Position;
                    }
                    if (input.SPressed)
                    {
                        selectedItem++;
                        if (selectedItem >= menuItems.Count) selectedItem = 0;
                        selectionIndicator.Position = menuItems[selectedItem].Position;
                    }
                    if (input.DPressed)
                    {
                        // select menu item
                        if (menus.ContainsKey(menuItems[selectedItem].Data))
                        {
                            LoadMenu(menuItems[selectedItem].Data);
                        } 
                        else if (menuItems[selectedItem].Data == "Quick Play")
                        {
                            Visible = false;
                            player.Visible = true;
                            player.inGame = true;
                            game.QuickPlay();
                        }
                        else if (Song.SongList.ContainsKey(menuItems[selectedItem].Data))
                        {
                            Visible = false;
                            player.Visible = true;
                            player.inGame = true;
                            game.PlaySong(menuItems[selectedItem].Data);
                        }
                    }
                    if (input.APressed)
                    {
                        // go back to previous menu
                        if (menuTitle != "Game Mode")
                        {
                            LoadMenu("Game Mode");
                        }
                    }
                }
                else if (input.PlayerJoining)
                {
                    title.Visible = true;
                    title.Data = "Stand on\ncenter of\ndance pad\nto join.";
                    foreach(ScreenSprite sprite in menuItems) sprite.Visible = false;
                }
                else
                {
                    if(menuTitle != "Game Mode") LoadMenu("Game Mode");
                    Visible = false;
                }
            }
        }
    }
}
