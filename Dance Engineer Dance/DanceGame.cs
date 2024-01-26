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
        public class DanceGame : Screen
        {
            ScreenSprite background;
            GameInput leftPlayer;
            GameInput rightPlayer;
            PlayerSide leftSide;
            PlayerSide rightSide;
            GameMenu leftMenu;
            GameMenu rightMenu;
            Dancer dancer;
            IMySoundBlock music;
            bool inGame = false;
            public static float songTime = 0; // current time in song (in seconds)
            public static float songLength = 30; // in seconds
            public static int bpm = 120; // beats per minute
            // the percentage of songLength is one beat
            public static float OneBeatIsPercentOfSong { get { return 1f / (songLength * (float)bpm / 60f); } }
            // calculate the max number of beats in the song
            public static int MaxBeats { get { return (int)(songLength * (float)bpm / 60f); } }
            // time of last update
            DateTime lastUpdate = DateTime.Now;
            public DanceGame(IMyTextSurface drawingSurface, GameInput leftPlayer, GameInput rightPlayer) : base(drawingSurface)
            {
                BackgroundColor = Color.Black;
                background = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, Vector2.Zero, 0.1f, new Vector2(Size.X, Size.Y), Color.White, "Monospace", GameSprites.GetBackground(), TextAlignment.LEFT, SpriteType.TEXT);
                AddSprite(background);
                this.leftPlayer = leftPlayer;
                this.rightPlayer = rightPlayer;
                music = GridBlocks.GetSpeaker("music");
                if(music != null) Song.LoadSongs(music.CustomData);
                leftMenu = new GameMenu(new Vector2(Size.X * -0.43f, Size.Y * 0.22f), leftPlayer,this);
                leftSide = new PlayerSide(leftPlayer, new DiscoLight("Disco Left"), this, new Vector2(Size.X * -0.47f, Size.Y * 0.05f), leftMenu);
                rightMenu = new GameMenu(new Vector2(Size.X * 0.12f, Size.Y * 0.22f), rightPlayer,this);
                rightSide = new PlayerSide(rightPlayer, new DiscoLight("Disco Right"), this, new Vector2(Size.X * 0.12f, Size.Y * 0.05f), rightMenu);
                //RandomizeArrows();
                leftSide.Visible = false;
                rightSide.Visible = false;
                dancer = new Dancer(new Vector2(0, Size.Y * 0.25f));
                dancer.RotationOrScale = 0.2f;
                AddSprite(dancer);
            }
            Random rnd = new Random();
            char[] chars = { 'w', 'a', 's', 'd' };
            void RandomizeArrows()
            {
                int maxBeats = MaxBeats;
                for (int i = 10; i < maxBeats; i++)
                {
                    int step = rnd.Next(0, 6);
                    if (step < chars.Length)
                    {
                        leftSide.AddArrowOnBeat(chars[step], i);
                        rightSide.AddArrowOnBeat(chars[step], i);
                    }
                }
                leftSide.BringScoreToFront();
                rightSide.BringScoreToFront();
            }
            void LoadArrows(string arrows)
            {
                int skip = 0;
                for (int i = 0; i < arrows.Length; i++)
                {
                    if (chars.Contains(arrows[i]))
                    {
                        leftSide.AddArrowOnBeat(arrows[i], i);
                        rightSide.AddArrowOnBeat(arrows[i], i);
                    } else if (int.TryParse(arrows[i].ToString(), out skip))
                    {
                        i += skip;
                    }
                }
            }
            override public void Draw()
            {
                UpdateSongTime();
                if (inGame)
                {
                    leftSide.Update();
                    rightSide.Update();
                }
                else
                {
                    leftMenu.Update();
                    rightMenu.Update();
                }
                base.Draw();
            }
            void UpdateSongTime()
            {
                songTime += (float)(DateTime.Now - lastUpdate).TotalSeconds;
                lastUpdate = DateTime.Now;
                if (songTime > songLength)
                {
                    //GridInfo.Echo("Song over");
                    inGame = false;
                    leftSide.Visible = false;
                    rightSide.Visible = false;
                    dancer.Visible = true;
                    songTime = 0;
                    songLength = 30;
                    //RandomizeArrows();
                    leftSide.Combo = 0f;
                    leftSide.ClearArrows();
                    rightSide.Combo = 0f;
                    rightSide.ClearArrows();
                    background.Data = GameSprites.GetBackground();
                    GameSprites.LoadDancerSprites(dancer);
                }
                // was this a beat?
                if (songTime % (60f / bpm) < 0.1f)
                {
                    leftSide.Beat();
                    rightSide.Beat();
                }
                // was this half way through a beat?
                if (songTime % (60f / bpm) > (60f / bpm) / 2f - 0.1f && songTime % (60f / bpm) < (60f / bpm) / 2f + 0.1f)
                {
                    dancer.NextSprite();
                    leftSide.MidBeat();
                    rightSide.MidBeat();
                }
            }
            public void QuickPlay()
            {
                Song song = Song.GetRandomSong();
                playSong(song);
            }
            public void PlaySong(string title)
            {
                Song song = Song.GetSong(title);
                playSong(song);
            }
            void playSong(Song song)
            {
                songLength = song.length;
                bpm = song.bpm;
                if(song.arrows.Trim() == "") RandomizeArrows();
                else LoadArrows(song.arrows);
                inGame = true;
                dancer.Visible = false;
                music.SelectedSound = song.track;
                music.Play();
            }
            public void Stop()
            {
                inGame = false;
                leftSide.Visible = false;
                rightSide.Visible = false;
                leftMenu.Visible = true;
                rightMenu.Visible = true;
            }
        }
    }
}
